using UnityEngine;
using TMPro; // Make sure you import the TextMeshPro namespace
using System.Collections; // For IEnumerator

public class UIManager : MonoBehaviour
{
    public static UIManager Instance; // Singleton instance
    public TextMeshProUGUI healthRestoredText; // Reference to the TextMeshProUGUI object

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
        // Check if healthRestoredText is assigned
        if (healthRestoredText != null)
        {
            // Activate the TextMeshPro object
            healthRestoredText.gameObject.SetActive(true);

            // Wait for the specified duration
            yield return new WaitForSeconds(duration);

            // Deactivate the TextMeshPro object
            healthRestoredText.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("healthRestoredText is not assigned in the Inspector!");
        }
    }
}
