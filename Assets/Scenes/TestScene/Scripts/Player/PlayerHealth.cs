using UnityEngine;
using Photon.Pun;

public class PlayerHealth : MonoBehaviour
{
    public int health;

    [PunRPC]
    public void TakeDamage(int damage){
        Debug.Log("taking damage");
        health -= damage;
        
        if (health <= 0){
            Debug.Log("dying");
            Destroy(gameObject);
        }
    }
}
