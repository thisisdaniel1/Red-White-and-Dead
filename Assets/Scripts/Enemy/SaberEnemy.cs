using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SaberEnemy : MonoBehaviour
{
    public float lungeDistance = 2f;
    public float lungeSpeed = 8f;
    public float backoffDistance = 4f;
    public float backoffSpeed = 5f;
    public float minAttackDelay = 1f;
    public float maxAttackDelay = 3f;
    public float dashBackDistance = 3f;
    public float dashBackSpeed = 10f;
    public float playerThreatRange = 2f; // triggers backward dash

    private NavMeshAgent agent;
    private Transform player;
    private bool isAttacking = false;
    private bool hasDashedBack = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = PlayerController.Instance.transform;
        StartCoroutine(AttackLoop());
    }

    void Update()
    {
        // If player comes too close and dash not used yet
        if (!hasDashedBack && Vector3.Distance(transform.position, player.position) <= playerThreatRange)
        {
            StartCoroutine(DashBackwards());
            hasDashedBack = true;
        }
    }

    private IEnumerator AttackLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minAttackDelay, maxAttackDelay));

            if (!isAttacking)
            {
                int attackCount = Random.Range(1, 4); // 1, 2, or 3 lunges
                yield return StartCoroutine(PerformLungeCombo(attackCount));

                // Back off after combo
                yield return StartCoroutine(BackOff());
            }
        }
    }

    private IEnumerator PerformLungeCombo(int count)
    {
        isAttacking = true;

        for (int i = 0; i < count; i++)
        {
            Vector3 targetPos = player.position;
            float elapsed = 0f;
            Vector3 start = transform.position;

            while (elapsed < 1f)
            {
                elapsed += Time.deltaTime * lungeSpeed;
                transform.position = Vector3.Lerp(start, targetPos, elapsed);
                yield return null;
            }

            yield return new WaitForSeconds(0.2f);
        }

        isAttacking = false;
    }

    private IEnumerator BackOff()
    {
        Vector3 dir = (transform.position - player.position).normalized;
        Vector3 targetPos = transform.position + dir * backoffDistance;

        float elapsed = 0f;
        Vector3 start = transform.position;

        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * backoffSpeed * Time.deltaTime;
            transform.position = Vector3.Lerp(start, targetPos, elapsed);
            yield return null;
        }
    }

    private IEnumerator DashBackwards()
    {
        Vector3 dir = (transform.position - player.position).normalized;
        Vector3 targetPos = transform.position + dir * dashBackDistance;

        float elapsed = 0f;
        Vector3 start = transform.position;

        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * dashBackSpeed * Time.deltaTime;
            transform.position = Vector3.Lerp(start, targetPos, elapsed);
            yield return null;
        }
    }
}
