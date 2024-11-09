using UnityEngine;
using System.Collections;

public class SpeedPickup : MonoBehaviour
{
    public float speedMultiplier = 3f;
    public float duration = 15f;

    void Update()
    {
        transform.Rotate(0, 100f * Time.deltaTime, 0);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
#pragma warning disable CS0436
            FirstPersonController playerController = collision.gameObject.GetComponent<FirstPersonController>();
#pragma warning restore CS0436

            if (playerController != null)
            {
                StartCoroutine(ApplySpeedBoost(playerController));

                if (AudioManager.Instance != null && AudioManager.Instance.pickupSound != null)
                {
                    AudioManager.Instance.PlaySound(AudioManager.Instance.pickupSound);
                }
                else
                {
                    Debug.LogError("AudioManager instance or pickupSound is missing!");
                }

                UIManager.Instance.ShowSpeedBoostText(15f);

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
        playerController.sprintSpeed *= speedMultiplier;
        playerController.unlimitedSprint = true;

        yield return new WaitForSeconds(duration);

        playerController.sprintSpeed /= speedMultiplier;
    }
}
