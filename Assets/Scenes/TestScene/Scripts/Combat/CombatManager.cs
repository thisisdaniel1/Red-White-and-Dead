using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public Army attacker;
    public Army defender;

    public List<Unit> unitsAttacker, unitsDefender;

    // Start is called before the first frame update
    void Start()
    {
        attacker.InstantiateArmy();
        //defender.InstantiateArmy();

        unitsAttacker = attacker.units;
        //unitsDefender = defender.units;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
