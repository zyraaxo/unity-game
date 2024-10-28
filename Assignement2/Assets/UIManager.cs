using UnityEngine;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TextMeshProUGUI initialMessageText; // Initial welcome message text
    public TextMeshProUGUI healthRestoredText; // Text for health restored message
    public TextMeshProUGUI speedBoostText; // Text for speed boost message
    public TextMeshProUGUI keyCheckText; // Text for key check message
    public TextMeshProUGUI keyPickupText; // Text for key pickup message
    public TextMeshProUGUI keyCountText; // Display for the key count
    public TextMeshProUGUI ammoCountText; // New text for displaying ammo count
    public TextMeshProUGUI lowAmmoWarningText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        DisplayInitialUI();
    }

    void Update()
    {
        UpdateKeyCountText(); // Update the key count each frame
    }

    private void DisplayInitialUI()
    {
        if (initialMessageText != null)
        {
            initialMessageText.text = "Welcome, there has been a zombie outbreak that needs containing. Find the 3 keys throughout the land then activate the portal to eliminate the hive.";
            initialMessageText.gameObject.SetActive(true);
            StartCoroutine(HideInitialUITextAfterDelay(5f)); // Hide after 5 seconds
        }
        else
        {
            Debug.LogError("initialMessageText is not assigned in the Inspector!");
        }
    }

    private IEnumerator HideInitialUITextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (initialMessageText != null)
        {
            initialMessageText.gameObject.SetActive(false);
        }
    }

    // Update the key count display
    private void UpdateKeyCountText()
    {
        if (keyCountText != null && Player.Instance != null)
        {
            keyCountText.text = "Keys: " + Player.Instance.GetKeys() + " / 3"; // Display the current key count
        }
        else if (keyCountText == null)
        {
            Debug.LogError("keyCountText is not assigned in the Inspector!");
        }
    }

    // Method to update the ammo count display
    public void UpdateAmmoCountText(int currentAmmo, int maxAmmo)
    {
        if (ammoCountText != null)
        {
            ammoCountText.text = "Ammo: " + currentAmmo + " / " + maxAmmo;

            if (currentAmmo <= 3 && lowAmmoWarningText != null)
            {
                lowAmmoWarningText.text = "Low Ammo! Press 'R' to reload";
                lowAmmoWarningText.gameObject.SetActive(true);
            }
            else if (lowAmmoWarningText != null)
            {
                lowAmmoWarningText.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.LogError("ammoCountText is not assigned in the Inspector!");
        }
    }

    // Method to show key pickup message
    public void ShowKeyPickupText(string message)
    {
        if (keyPickupText != null)
        {
            keyPickupText.text = message;
            keyPickupText.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("keyPickupText is not assigned in the Inspector!");
        }
    }

    public void HideKeyPickupText()
    {
        if (keyPickupText != null)
        {
            keyPickupText.gameObject.SetActive(false);
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
            healthRestoredText.gameObject.SetActive(true);
            yield return new WaitForSeconds(duration);
            healthRestoredText.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("healthRestoredText is not assigned in the Inspector!");
        }
    }

    public void ShowSpeedBoostText(float duration)
    {
        StartCoroutine(ShowSpeedBoostTextRoutine(duration));
    }

    private IEnumerator ShowSpeedBoostTextRoutine(float duration)
    {
        if (speedBoostText != null)
        {
            speedBoostText.gameObject.SetActive(true);

            float timeLeft = duration;
            while (timeLeft > 0)
            {
                speedBoostText.text = "Speed Boost Active! Time Left: " + Mathf.Ceil(timeLeft) + "s";
                yield return new WaitForSeconds(1f);
                timeLeft -= 1f;
            }

            speedBoostText.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("speedBoostText is not assigned in the Inspector!");
        }
    }

    public void ShowKeyCheckText(int missingKeys)
    {
        if (keyCheckText != null)
        {
            keyCheckText.text = "You need " + missingKeys + " more key" + (missingKeys > 1 ? "s" : "") + " to activate the portal!";
            keyCheckText.gameObject.SetActive(true);
            StartCoroutine(HideKeyCheckTextAfterDelay(5f));
        }
        else
        {
            Debug.LogError("keyCheckText is not assigned in the Inspector!");
        }
    }

    private IEnumerator HideKeyCheckTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        keyCheckText.gameObject.SetActive(false);
    }

    public void ShowKeyCheckText(string message)
    {
        if (keyCheckText != null)
        {
            keyCheckText.text = message;
            keyCheckText.gameObject.SetActive(true);
            StartCoroutine(HideKeyCheckTextRoutine());
        }
        else
        {
            Debug.LogError("keyCheckText is not assigned in the Inspector!");
        }
    }

    private IEnumerator HideKeyCheckTextRoutine()
    {
        yield return new WaitForSeconds(3f);
        keyCheckText.gameObject.SetActive(false);
    }
}
