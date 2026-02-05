using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class MapLocation : MonoBehaviour
{
    [Header("Location Data")]
    public string locationID;
    public string sceneToLoad; // scene to load when player enters this location
    public bool allowFastTravel = true;

    [Header("Encounters")]
    [Tooltip("Encounters that can trigger while traveling on/near this tile. Weighted system not included but can be added.")]
    public List<RandomEncounterSO> possibleEncounters = new List<RandomEncounterSO>();

    [Header("Optional")]
    public Color gizmoColor = Color.cyan;

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, 0.35f);
    }
}
