using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(0, 100f * Time.deltaTime, 0);
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.currentHealth = playerHealth.maxHealth;
                playerHealth.UpdateHealthBar();
                Debug.Log("Player health reset to max!");

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

                UIManager.Instance.ShowHealthRestoredText(2f);

                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("PlayerHealth component not found on player!");
            }
        }
    }
}
