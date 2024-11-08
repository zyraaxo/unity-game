using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public Slider healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(float damageAmount)
    {
        Debug.Log("Called " + currentHealth);
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();

        AudioManager.Instance.PlaySound(AudioManager.Instance.hitSound);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }
    }

    void Die()
    {
        if (UIManager.Instance != null && UIManager.Instance.deathText != null)
        {
            UIManager.Instance.deathText.text = "You Died! Press ESC to restart.";
            UIManager.Instance.deathText.gameObject.SetActive(true);
        }

        Debug.Log("Player has died!");
        StartCoroutine(WaitForRestart());
    }

    IEnumerator WaitForRestart()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // Reset player's health back to maxHealth before restarting
                currentHealth = maxHealth;
                UpdateHealthBar();

                // Reload the current scene
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
            }
            yield return null;
        }
    }
}
