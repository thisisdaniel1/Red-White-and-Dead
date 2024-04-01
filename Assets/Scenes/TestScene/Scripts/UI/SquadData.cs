using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Squad", menuName = "Squad")]
public class SquadData : ScriptableObject
{
    public string squadName;
    public Sprite squadIcon;
    public List<Unit> units = new List<Unit>();
}
