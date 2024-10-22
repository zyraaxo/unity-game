using UnityEngine;
using TMPro; // Make sure you import the TextMeshPro namespace
using System.Collections; // For IEnumerator

public class UIManager : MonoBehaviour
{
    public static UIManager Instance; // Singleton instance
    public TextMeshProUGUI healthRestoredText; // Reference to the TextMeshProUGUI object
    public TextMeshProUGUI speedBoostText; // Reference to the TextMeshProUGUI object for speed boost
    public TextMeshProUGUI keyCheckText; // Reference to the TextMeshProUGUI object for speed boost


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: persist through scene loads
        }
        else
        {
            Destroy(gameObject); // Prevent duplicate instances
        }
    }

    public void ShowHealthRestoredText(float duration)
    {
        StartCoroutine(ShowHealthRestoredTextRoutine(duration));
    }

    private IEnumerator ShowHealthRestoredTextRoutine(float duration)
    {
        if (healthRestoredText != null)
        {
            duration = 15.0f;
            healthRestoredText.gameObject.SetActive(true); // Activate the TextMeshPro object
            yield return new WaitForSeconds(duration);     // Wait for the specified duration
            healthRestoredText.gameObject.SetActive(false); // Deactivate the TextMeshPro object
        }
        else
        {
            Debug.LogError("healthRestoredText is not assigned in the Inspector!");
        }
    }

    // Method to show speed boost text with countdown
    public void ShowSpeedBoostText(float duration)
    {
        StartCoroutine(ShowSpeedBoostTextRoutine(duration));
    }

    private IEnumerator ShowSpeedBoostTextRoutine(float duration)
    {
        if (speedBoostText != null)
        {
            speedBoostText.gameObject.SetActive(true); // Activate the TextMeshPro object

            float timeLeft = duration; // Initialize time left
            while (timeLeft > 0)
            {
                // Update the text to show the remaining time
                speedBoostText.text = "Speed Boost Active! Time Left: " + Mathf.Ceil(timeLeft) + "s";
                yield return new WaitForSeconds(1f); // Wait for 1 second
                timeLeft -= 1f; // Decrease time left
            }

            speedBoostText.gameObject.SetActive(false); // Deactivate the TextMeshPro object after the countdown
        }
        else
        {
            Debug.LogError("speedBoostText is not assigned in the Inspector!");
        }
    }
}
