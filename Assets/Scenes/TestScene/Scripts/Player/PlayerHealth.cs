using UnityEngine;
using Photon.Pun;

public class PlayerHealth : MonoBehaviour
{
    public int health;

    [PunRPC]
    public void TakeDamage(int damage){
        health -= damage;
        
        if (health <= 0){
            Destroy(gameObject);
        }
    }
}
