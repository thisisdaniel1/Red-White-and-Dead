using UnityEngine;

public class BillboardingOld : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Camera is always updated first
    void LateUpdate()
    {
        if (Camera.main != null){
            GameObject nearestPlayer = FindNearestPlayer();
            
            if (nearestPlayer != null){
                Vector3 directionToPlayer = nearestPlayer.transform.position - transform.position;
                // This ensures that the zombie only rotates around the y-axis
                directionToPlayer.y = 0; 

                // Smoothly rotate towards the player
                Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }
        }
    }

    GameObject FindNearestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject nearestPlayer = null;
        float minDistance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach(GameObject player in players)
        {
            float distance = Vector3.Distance(player.transform.position, position);
            if(distance < minDistance)
            {
                nearestPlayer = player;
                minDistance = distance;
            }
        }
        return nearestPlayer;
    }
}
