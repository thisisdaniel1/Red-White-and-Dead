using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesInRange : MonoBehaviour
{
    public List<Enemy> enemiesInRangeList = new List<Enemy>();

    public void RemoveEnemy(Enemy enemy){
        enemiesInRangeList.RemoveAll(item => item == null);
        enemiesInRangeList.Remove(enemy);
    }

    public void AddEnemy(Enemy enemy){
        if (!enemiesInRangeList.Contains(enemy)){
            enemiesInRangeList.Add(enemy);
        }
    }
}
