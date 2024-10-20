using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    void Update()
    {
        // Rotate the object around the Y-axis constantly
        transform.Rotate(0, 100f * Time.deltaTime, 0);
    }


    // This function is called when the player collides with the object
    void OnCollisionEnter(Collision collision)
    {
        // Check if the object colliding with this object is the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Try to get the PlayerHealth component from the player
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                // Reset player's health to max
                playerHealth.currentHealth = playerHealth.maxHealth;
                playerHealth.UpdateHealthBar();
                Debug.Log("Player health reset to max!");

                // Check AudioManager instance and play sound
                if (AudioManager.Instance == null)
                {
                    Debug.LogError("AudioManager instance is null! Make sure the AudioManager is present in the scene.");
                }
                else if (AudioManager.Instance.pickupSound == null)
                {
                    Debug.LogError("pickupSound is null! Please assign an audio clip in the AudioManager.");
                }
                else
                {
                    AudioManager.Instance.PlaySound(AudioManager.Instance.pickupSound);
                }

                // Show the "Health Restored" text using UIManager
                UIManager.Instance.ShowHealthRestoredText(2f); // Pass the duration here

                // Destroy the health pickup (make it disappear)
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("PlayerHealth component not found on player!");
            }
        }
    }
}
