using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f; // Maximum health of the player
    public float currentHealth; // Current health of the player
    public Slider healthBar; // Reference to the health bar slider

    void Start()
    {
        currentHealth = maxHealth; // Set current health to max health
        UpdateHealthBar();
    }

    public void TakeDamage(float damageAmount)
    {

        Debug.Log("Called" + currentHealth);
        currentHealth -= damageAmount; // Reduce health
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Clamp health to valid range
        UpdateHealthBar();

        // Optional: Check for death
        if (currentHealth <= 0)
        {
            Die(); // Call a method to handle player death
        }
    }

    public void Heal(float healAmount)
    {
        currentHealth += healAmount; // Increase health
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Clamp health to valid range
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth; // Set the slider value to current health
        }
    }

    void Die()
    {
        // Handle player death (e.g., reload the scene, show a death screen, etc.)
        Debug.Log("Player has died!");
    }
}
