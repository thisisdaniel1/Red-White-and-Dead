using UnityEngine;

[CreateAssetMenu(fileName = "RandomEncounter", menuName = "Encounters/RandomEncounter")]
public class RandomEncounterSO : ScriptableObject
{
    public string encounterID;
    public string displayName;
    [Tooltip("Scene name to load when this encounter triggers. Can be a prefab or an additive scene depending on your flow.")]
    public string sceneToLoad;
    [Tooltip("Optional slot index if you use slots inside a scene (0,1,2...)")]
    public int sceneSlot = 0;
}
