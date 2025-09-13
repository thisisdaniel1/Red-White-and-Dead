using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;

    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("UI")]
    public Image bloodOverlay; // UI image with transparent red
    public Gradient bloodColor; // Optional gradient from light red -> dark red
    public float maxOverlayAlpha = 0.8f; // Max blood intensity

    private bool isDead = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        currentHealth = maxHealth;
        UpdateBloodOverlay();
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateBloodOverlay();

        if (currentHealth <= 0)
        {
            KillPlayer();
        }
    }

    public void Heal(float amount)
    {
        if (isDead) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateBloodOverlay();
    }

    public void KillPlayer()
    {
        if (isDead) return;
        isDead = true;

        currentHealth = 0;
        UpdateBloodOverlay();

        Debug.Log("Player has died!");
        // TODO: Add death animation, game over UI, etc.
    }

    private void UpdateBloodOverlay()
    {
        if (bloodOverlay != null)
        {
            float healthPercent = currentHealth / maxHealth;
            float alpha = Mathf.Lerp(maxOverlayAlpha, 0f, healthPercent);
            bloodOverlay.color = new Color(
                bloodOverlay.color.r,
                bloodOverlay.color.g,
                bloodOverlay.color.b,
                alpha
            );

            // Optional: adjust color by gradient
            if (bloodColor != null)
            {
                bloodOverlay.color = bloodColor.Evaluate(1 - healthPercent);
                bloodOverlay.color = new Color(
                    bloodOverlay.color.r,
                    bloodOverlay.color.g,
                    bloodOverlay.color.b,
                    alpha
                );
            }
        }
    }
}
