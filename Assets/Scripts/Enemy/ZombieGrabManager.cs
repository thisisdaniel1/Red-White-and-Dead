using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieGrabManager : MonoBehaviour
{
    public static ZombieGrabManager Instance;

    [Header("Grab Settings")]
    public float grabRange = 1f;
    public float baseDamagePerZombie = 5f; // damage per second per zombie
    public GameObject bloodSplatterPrefab;

    private List<ZombieEnemy> grabbingZombies = new List<ZombieEnemy>();
    private bool playerGrabbed = false;
    private PlayerController player;
    private PlayerHealth playerHealth;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        player = PlayerController.Instance;
        playerHealth = PlayerHealth.Instance;
    }

    void Update()
    {
        ZombieEnemy[] allZombies = FindObjectsOfType<ZombieEnemy>();

        if (playerGrabbed)
        {
            // During a grab, keep checking for new zombies
            foreach (ZombieEnemy zombie in allZombies)
            {
                if (!grabbingZombies.Contains(zombie))
                {
                    float dist = Vector3.Distance(zombie.transform.position, player.transform.position);
                    if (dist <= grabRange && zombie.isAggro)
                    {
                        grabbingZombies.Add(zombie);
                        zombie.FreezeZombie();

                        // If 5 zombies pile on, player dies instantly
                        if (grabbingZombies.Count >= 5)
                        {
                            playerHealth.KillPlayer();
                            return;
                        }
                    }
                }
            }

            // Apply damage over time based on how many zombies are grabbing
            float dps = baseDamagePerZombie * grabbingZombies.Count;
            playerHealth.TakeDamage(dps * Time.deltaTime);
        }
        else
        {
            // Not grabbed yet, check for initial grab
            List<ZombieEnemy> closeZombies = new List<ZombieEnemy>();
            foreach (ZombieEnemy zombie in allZombies)
            {
                float dist = Vector3.Distance(zombie.transform.position, player.transform.position);
                if (dist <= grabRange && zombie.isAggro)
                {
                    closeZombies.Add(zombie);
                }
            }

            if (closeZombies.Count >= 2)
            {
                StartGrab(closeZombies);
            }
        }
    }

    public bool IsPlayerGrabbed()
    {
        return playerGrabbed;
    }

    private void StartGrab(List<ZombieEnemy> zombies)
    {
        playerGrabbed = true;
        grabbingZombies = zombies;

        player.FreezePlayer();

        foreach (ZombieEnemy z in grabbingZombies)
        {
            z.FreezeZombie();
        }

        if (bloodSplatterPrefab != null)
        {
            Instantiate(bloodSplatterPrefab, player.transform.position, Quaternion.identity);
        }
    }

    public void ReleasePlayer()
    {
        player.UnFreezePlayer();

        foreach (ZombieEnemy z in grabbingZombies)
        {
            if (z != null)
            {
                z.ReleaseZombie();
            }
        }

        grabbingZombies.Clear();
        playerGrabbed = false;
    }
}
