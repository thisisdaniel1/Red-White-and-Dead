using UnityEngine;
using UnityEngine.AI;

public class ZombieEnemy : Enemy
{
    [Header("Zombie Attack Settings")]
    public float attackRange = 1.5f;
    public float attackCooldown = 2f;
    public float attackDamage = 10f;

    private NavMeshAgent agent;
    private float lastAttackTime = -999f;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        base.Update(); // run parent Enemy logic

        if (isAggro && PlayerController.Instance != null)
        {
            float dist = Vector3.Distance(transform.position, PlayerController.Instance.transform.position);

            // If close enough and not already in grab
            if (dist <= attackRange && !ZombieGrabManager.Instance.IsPlayerGrabbed())
            {
                TryAttackPlayer();
            }
        }
    }

    private void TryAttackPlayer()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;

            // Deal chip damage to player
            PlayerHealth.Instance.TakeDamage(attackDamage);

            Debug.Log("Zombie attacked player for " + attackDamage + " damage.");
        }
    }

    public void FreezeZombie()
    {
        if (agent != null)
            agent.isStopped = true;
    }

    public void ReleaseZombie()
    {
        if (agent != null)
            agent.isStopped = false;

        // Knockback effect
        Vector3 knockDir = (transform.position - PlayerController.Instance.transform.position).normalized;
        transform.position += knockDir * 2f;
    }
}
