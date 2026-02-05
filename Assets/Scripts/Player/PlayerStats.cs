using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Skills")]
    [Range(0, 100)]
    public int survivalSkill = 0;

    [Header("Encounter Flags")]
    [Tooltip("If true, next encounter roll will be guaranteed to trigger")]
    public bool guaranteedEncounter = false;

    // Calculates survival modifier [0..1] where higher skill reduces encounter chance.
    // Example: with survivalSkill=50 returns 0.5 (50% reduction). Tweak curve as needed.
    public float SurvivalModifier()
    {
        return Mathf.Clamp01(survivalSkill / 100f);
    }
}
