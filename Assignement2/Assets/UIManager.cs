using UnityEngine;
using TMPro; // Make sure to import the TextMeshPro namespace
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance; // Singleton instance
    public TextMeshProUGUI healthRestoredText; // Reference to the TextMeshProUGUI object
    public TextMeshProUGUI speedBoostText; // Reference to the TextMeshProUGUI object for speed boost
    public TextMeshProUGUI keyCheckText; // Reference to the TextMeshProUGUI object for key check
    public TextMeshProUGUI keyPickupText; // Reference to the TextMeshProUGUI object for key pickup

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

    // Method to show key pickup message
    public void ShowKeyPickupText(string message)
    {
        if (keyPickupText != null)
        {
            keyPickupText.text = message; // Set the text to the provided message
            keyPickupText.gameObject.SetActive(true); // Show the text
        }
        else
        {
            Debug.LogError("keyPickupText is not assigned in the Inspector!");
        }
    }

    // Method to hide the key pickup message
    public void HideKeyPickupText()
    {
        if (keyPickupText != null)
        {
            keyPickupText.gameObject.SetActive(false); // Deactivate the TextMeshPro object
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

    // Method to show key check message
    public void ShowKeyCheckText(int missingKeys)
    {
        if (keyCheckText != null)
        {
            keyCheckText.text = "You need " + missingKeys + " more key" + (missingKeys > 1 ? "s" : "") + " to activate portal!";
            keyCheckText.gameObject.SetActive(true); // Activate the TextMeshPro object

            // Optionally, you can hide this message after a certain duration
            StartCoroutine(HideKeyCheckTextAfterDelay(5f)); // Hide after 5 seconds
        }
        else
        {
            Debug.LogError("keyCheckText is not assigned in the Inspector!");
        }
    }

    private IEnumerator HideKeyCheckTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        keyCheckText.gameObject.SetActive(false); // Deactivate the TextMeshPro object
    }

    public void ShowKeyCheckText(string message)
    {
        if (keyCheckText != null)
        {
            keyCheckText.text = message; // Set the text to the provided message
            keyCheckText.gameObject.SetActive(true); // Show the text
            StartCoroutine(HideKeyCheckTextRoutine()); // Optionally hide it after some time
        }
        else
        {
            Debug.LogError("keyCheckText is not assigned in the Inspector!");
        }
    }

    private IEnumerator HideKeyCheckTextRoutine()
    {
        yield return new WaitForSeconds(3f); // Adjust duration as needed
        keyCheckText.gameObject.SetActive(false); // Hide the text
    }
}
