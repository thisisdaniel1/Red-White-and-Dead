using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<Enemy> enemies = new List<Enemy>();
    public static EnemyManager Instance;
    private Transform localPlayerTransform;

    void Awake(){
        Instance = this;
    }

    public void RemoveEnemy(Enemy enemy){
        enemies.RemoveAll(item => item == null);
        enemies.Remove(enemy);
    }

    public void AddEnemy(Enemy enemy){
        if (!enemies.Contains(enemy)){
            enemies.Add(enemy);

            if (localPlayerTransform != null){
                enemy.SetTarget(localPlayerTransform);
            }
        }
    }

    public void SetLocalPlayer(Transform playerTransform){
        localPlayerTransform = playerTransform;
        foreach (var enemy in enemies){
            enemy.SetTarget(playerTransform);
        }
    }
}
