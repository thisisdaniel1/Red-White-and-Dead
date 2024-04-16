using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int health;
	
	public int maxArmor = 50;
	public int armor;

    void Start(){
        health = maxHealth;
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.RightShift)){
            TakeDamage(30);
        }
    }

    public void GiveHealth(int amount, GameObject pickup){
        if (health < maxHealth){
            health += amount;
            Destroy(pickup);
        }

        if (health > maxHealth){
            health = maxHealth;
        }
    }

    public void GiveArmor(int amount, GameObject pickup){
        if (armor < maxArmor){
            armor += amount;
            Destroy(pickup);
        }

        if (armor > maxArmor){
            armor = maxArmor;
        }
    }

    public void TakeDamage(int damage){
		
		// if player has armor, take damage on armor first
		if (armor > 0){
			
			// if player has enough armor to absorb all of the damage,
			// then only damage the armor
			if (armor >= damage){
				armor -= damage;
            }

            // if player does not have enough armor to absorb the whole hit
            // subtract the remainder from health
            else if (armor < damage){
                int remainingDamage;
                remainingDamage = damage - armor;

                armor = 0;
                health -= remainingDamage;
            }
		}
        else{
            health -= damage;
        }

        if (health <= 0){
            Debug.Log("dying");
            //Destroy(gameObject);
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.buildIndex);
        }
    }
}
