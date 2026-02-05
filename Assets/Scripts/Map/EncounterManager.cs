using UnityEngine;
using UnityEngine.SceneManagement;

public class EncounterManager : MonoBehaviour
{
    public static EncounterManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Trigger an encounter: either load a separate encounter scene, or route through your encounter system.
    /// </summary>
    public void TriggerEncounter(RandomEncounterSO encounter)
    {
        if (encounter == null)
        {
            Debug.LogWarning("EncounterManager.TriggerEncounter called with null encounter");
            return;
        }

        Debug.Log($"Triggering encounter: {encounter.displayName} -> {encounter.sceneToLoad}");
        // Option A: load a dedicated scene (single)
        if (!string.IsNullOrEmpty(encounter.sceneToLoad))
        {
            // Consider additive loading if you want to preserve map scene state
            SceneManager.LoadScene(encounter.sceneToLoad);
        }
        else
        {
            // Option B: raise an event to your encounter UI and pass the SO
            // e.g. OnEncounterTriggered?.Invoke(encounter);
        }
    }
}
