using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class WorldMapController : MonoBehaviour
{
    [Header("References")]
    public Transform partyMarker; // the visual representing the party on the map
    public PlayerStats playerStats;
    public float movementSpeed = 3f; // world units per second
    public float travelTickInterval = 1f; // seconds per time-tick and encounter roll
    public float baseEncounterChancePerSecond = 0.10f; // 10% per second base chance (tweak)

    [Header("Map settings")]
    public LayerMask clickableLayer; // set to layer for MapLocation circles
    public LayerMask mapPlaneLayer; // set to layer for empty map clicks (if you want travel across empty)
    public Camera uiCamera; // camera that renders the map

    public bool isTraveling { get; private set; } = false;

    private MapLocation currentLocation;
    private MapLocation targetLocation;
    private Vector3 targetPosition;
    private Coroutine travelCoroutine;

    private void Start()
    {
        if (partyMarker == null) Debug.LogError("Party marker not assigned");
        if (playerStats == null) Debug.LogError("PlayerStats not assigned");
        // Try to find MapLocation under party position at start
        var hit = Physics2D.OverlapPointAll(partyMarker.position, clickableLayer);
        if (hit.Length > 0) currentLocation = hit[0].GetComponent<MapLocation>();
    }

    private void Update()
    {
        // Use left-mouse click. If using New Input System, bind to that and call OnPointerClick with the world pos.
        if (Mouse.current != null) // small check to avoid error if input system not present
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                OnLeftClick();
            }
        }
        else
        {
            // fallback: old Input
            if (Input.GetMouseButtonDown(0)) OnLeftClick();
        }
    }

    private void OnLeftClick()
    {
        // ignore clicks over UI
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

        Vector3 worldPoint = uiCamera.ScreenToWorldPoint(Mouse.current != null ? (Vector3)Mouse.current.position.ReadValue() : Input.mousePosition);
        worldPoint.z = 0f;

        // Raycast for location
        Collider2D locHit = Physics2D.OverlapPoint(worldPoint, clickableLayer);
        if (locHit != null)
        {
            MapLocation loc = locHit.GetComponent<MapLocation>();
            if (loc != null)
            {
                // If click landed on the party marker position (same location) -> enter
                if (Vector2.Distance(partyMarker.position, loc.transform.position) < 0.25f)
                {
                    EnterLocation(loc);
                    return;
                }

                // Otherwise, start travel to that location
                StartTravelTo(loc);
                return;
            }
        }

        // Click on empty map space: travel there (no location)
        Collider2D planeHit = Physics2D.OverlapPoint(worldPoint, mapPlaneLayer);
        if (planeHit != null)
        {
            StartTravelToPoint(worldPoint);
        }
    }

    public void StartTravelTo(MapLocation location)
    {
        targetLocation = location;
        targetPosition = location.transform.position;
        StartTravelToPoint(targetPosition);
    }

    public void StartTravelToPoint(Vector3 worldPoint)
    {
        // if currently traveling, interrupt or ignore depending on design
        if (travelCoroutine != null) StopCoroutine(travelCoroutine);
        travelCoroutine = StartCoroutine(TravelRoutine(worldPoint));
    }

    private IEnumerator TravelRoutine(Vector3 destination)
    {
        isTraveling = true;
        float tickTimer = 0f;
        bool encountered = false;

        while (Vector2.Distance(partyMarker.position, destination) > 0.01f)
        {
            // Move marker
            Vector3 prev = partyMarker.position;
            partyMarker.position = Vector3.MoveTowards(partyMarker.position, destination, movementSpeed * Time.deltaTime);

            // accumulate tick timer using unscaled delta so map UI can use timeScale differently if needed
            tickTimer += Time.deltaTime;
            if (tickTimer >= travelTickInterval)
            {
                tickTimer -= travelTickInterval;
                // increment game time by 1 second (or travelTickInterval seconds if you want more/less)
                int secondsToAdd = Mathf.RoundToInt(travelTickInterval);
                GameTime.Instance?.AddSeconds(secondsToAdd);

                // Resolve random encounter
                encountered = TryRollEncounter();
                if (encountered)
                {
                    // move marker to encounter position (leave it where it was)
                    isTraveling = false;
                    travelCoroutine = null;
                    yield break; // encounter manager will load new scene; stop travel
                }
            }

            yield return null;
        }

        // Arrived
        isTraveling = false;
        travelCoroutine = null;

        // Snap to destination precisely
        partyMarker.position = destination;

        // Update currentLocation if applicable
        if (targetLocation != null && Vector2.Distance(partyMarker.position, targetLocation.transform.position) < 0.01f)
        {
            currentLocation = targetLocation;
            targetLocation = null;
        }
    }

    /// <summary>
    /// Rolls for an encounter using the current tile's encounter pool (or fallback logic).
    /// Returns true if an encounter triggered.
    /// </summary>
    private bool TryRollEncounter()
    {
        // If player flagged for guaranteed encounter -> force it
        if (playerStats.guaranteedEncounter)
        {
            playerStats.guaranteedEncounter = false;
            TriggerRandomEncounterAtPosition(partyMarker.position);
            return true;
        }

        // Compute modified chance
        float survivalModifier = playerStats.SurvivalModifier(); // [0..1]
        float modifiedChance = baseEncounterChancePerSecond * (1f - survivalModifier);

        // clamp between 0 and 1
        modifiedChance = Mathf.Clamp01(modifiedChance);

        float roll = Random.value;
        if (roll <= modifiedChance)
        {
            TriggerRandomEncounterAtPosition(partyMarker.position);
            return true;
        }
        return false;
    }

    private void TriggerRandomEncounterAtPosition(Vector3 position)
    {
        // find nearest MapLocation within a small radius to determine encounter pool
        Collider2D[] hits = Physics2D.OverlapCircleAll(position, 1.5f, clickableLayer);
        List<RandomEncounterSO> candidates = new List<RandomEncounterSO>();
        foreach (var c in hits)
        {
            MapLocation loc = c.GetComponent<MapLocation>();
            if (loc != null && loc.possibleEncounters != null)
                candidates.AddRange(loc.possibleEncounters);
        }

        // fallback: if none found, you may have a global list to pick from
        if (candidates.Count == 0)
        {
            Debug.Log("No local encounters found; no encounter triggered. Consider adding global pool.");
            return;
        }

        // pick random encounter from candidates. You can replace with weighted pick.
        var chosen = candidates[Random.Range(0, candidates.Count)];
        EncounterManager.Instance.TriggerEncounter(chosen);
    }

    private void EnterLocation(MapLocation loc)
    {
        if (loc == null) return;
        // Optionally check allowFastTravel or travel costs
        Debug.Log($"Entering location: {loc.locationID} -> {loc.sceneToLoad}");
        if (!string.IsNullOrEmpty(loc.sceneToLoad))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(loc.sceneToLoad);
        }
    }
}
