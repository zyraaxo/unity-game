using UnityEngine;
using System.Collections;

public class SpeedPickup : MonoBehaviour
{
    public float speedMultiplier = 3f; // Multiplier for the player's speed
    public float duration = 15f; // Duration for which the speed boost will last

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
            // Try to get the FirstPersonController component from the player
#pragma warning disable CS0436 // Type conflicts with imported type
            FirstPersonController playerController = collision.gameObject.GetComponent<FirstPersonController>();
#pragma warning restore CS0436 // Type conflicts with imported type

            if (playerController != null)
            {
                // Apply the speed boost
                StartCoroutine(ApplySpeedBoost(playerController));

                // Play pickup sound
                if (AudioManager.Instance != null && AudioManager.Instance.pickupSound != null)
                {
                    AudioManager.Instance.PlaySound(AudioManager.Instance.pickupSound);
                }
                else
                {
                    Debug.LogError("AudioManager instance or pickupSound is missing!");
                }

                // Show the "Speed Boost" text using UIManager (if you have UIManager setup)
                UIManager.Instance.ShowSpeedBoostText(15f); // Optional: If you have UIManager

                // Destroy the speed pickup (make it disappear)
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("FirstPersonController component not found on player!");
            }
        }
    }

#pragma warning disable CS0436 
    private IEnumerator ApplySpeedBoost(FirstPersonController playerController)
#pragma warning restore CS0436 
    {
        // Triple the player's sprint speed
        playerController.sprintSpeed *= speedMultiplier;
        playerController.unlimitedSprint = true;

        // Wait for the duration of the speed boost
        yield return new WaitForSeconds(duration);

        // Reset the player's sprint speed to its normal value
        playerController.sprintSpeed /= speedMultiplier;
    }
}
