using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Add this to use SceneManager

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
    public TextMeshProUGUI currentWeapon;
    public TextMeshProUGUI esc;
    public TextMeshProUGUI deathText;
    public TextMeshProUGUI exfilText; // New text for exfil message



    public TextMeshProUGUI pickUpText;
    public TextMeshProUGUI switchWeaponText;
    public TextMeshProUGUI switchWeaponHintText; // New text for the "Press Tab to switch weapons" hint



    [SerializeField] private Text[] ammoCountTexts; // Assign different Text components in the inspector

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
        CheckSceneAndDisableUI(); // Call to disable UI based on the scene
    }

    void Update()
    {
        UpdateKeyCountText(); // Update the key count each frame
    }
    public void ShowExfilMessage()
    {
        if (exfilText != null)
        {
            exfilText.text = "Data Upload complete. Head back to chopper for exfil";
            exfilText.gameObject.SetActive(true); // Display the message

            // Hide the message after 3 seconds
            StartCoroutine(HideExfilMessageAfterDelay(3f));
        }
        else
        {
            Debug.LogError("exfilText is not assigned in the Inspector!");
        }
    }
    private IEnumerator HideExfilMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (exfilText != null)
        {
            exfilText.gameObject.SetActive(false); // Hide the message after the delay
        }
    }


    private void CheckSceneAndDisableUI()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "d")
        {
            // Check if UI elements are assigned before modifying them
            if (initialMessageText != null) initialMessageText.gameObject.SetActive(false);
            if (healthRestoredText != null) healthRestoredText.gameObject.SetActive(true);
            if (pickUpText != null) pickUpText.gameObject.SetActive(true);

            if (speedBoostText != null) speedBoostText.gameObject.SetActive(true);
            if (ammoCountText != null) ammoCountText.gameObject.SetActive(true);
            if (lowAmmoWarningText != null) lowAmmoWarningText.gameObject.SetActive(true);
            if (keyCountText != null) keyCountText.gameObject.SetActive(false);
        }
        else if (sceneName == "Map")
        {
            if (initialMessageText != null) initialMessageText.gameObject.SetActive(true);
            if (keyCountText != null) keyCountText.gameObject.SetActive(true);
        }
    }


    private void DisplayInitialUI()
    {
        if (initialMessageText != null)
        {
            initialMessageText.text = "Welcome, there has been a zombie outbreak that needs containing. Find the 3 Data devices throughout the land then update data at the console to send back to HQ.";
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

    public void UpdateAmmoCountText(int currentAmmo, int totalAmmo)
    {
        if (ammoCountText != null)
        {
            ammoCountText.text = $"{currentAmmo} / {totalAmmo}"; // Display current ammo over total ammo

            // Low ammo warning
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
    public void UpdateWeaponText(string weaponName)
    {
        if (currentWeapon != null)
        {
            currentWeapon.text = "Current Weapon: " + weaponName;
        }
    }

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
    public void DisplaySwitchWeaponText(string weaponName)
    {
        if (switchWeaponText != null)
        {
            switchWeaponText.text = "Switched to: " + weaponName; // Display the name of the new weapon
            switchWeaponText.gameObject.SetActive(true);

            // Hide the text after a delay
            StartCoroutine(HideSwitchWeaponTextAfterDelay(2f)); // Adjust delay as needed
        }
        else
        {
            Debug.LogError("switchWeaponText is not assigned in the Inspector!");
        }
    }

    private IEnumerator HideSwitchWeaponTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (switchWeaponText != null)
        {
            switchWeaponText.gameObject.SetActive(false);
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
    public void UpdatePickupText(string message)
    {
        if (pickUpText != null)
        {
            pickUpText.text = message;
        }
        else
        {
            Debug.LogError("pickUpText is not assigned in the Inspector!");
        }
    }
    public void DisplaySwitchWeaponHint(string hint)
    {
        if (switchWeaponHintText != null)
        {
            switchWeaponHintText.text = hint;
            switchWeaponHintText.gameObject.SetActive(true);
            StartCoroutine(HideTextAfterDelay(switchWeaponHintText, 5f)); // Hide after 5 seconds
        }
    }

    private IEnumerator HideTextAfterDelay(TextMeshProUGUI text, float delay)
    {
        yield return new WaitForSeconds(delay);
        text.gameObject.SetActive(false);
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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckSceneAndDisableUI(); // Re-evaluate which UI elements to enable/disable
    }
    public void TogglePickupText(bool show)
    {
        if (pickUpText != null)
        {
            pickUpText.gameObject.SetActive(show);
        }
    }
}

