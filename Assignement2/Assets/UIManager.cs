using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//this class handles all the UI elements in the game, makes it easy to manage as there are alot of different UI elements

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TextMeshProUGUI initialMessageText;
    public TextMeshProUGUI healthRestoredText;
    public TextMeshProUGUI speedBoostText;
    public TextMeshProUGUI keyCheckText;
    public TextMeshProUGUI keyPickupText;
    public TextMeshProUGUI keyCountText;
    public TextMeshProUGUI ammoCountText;
    public TextMeshProUGUI lowAmmoWarningText;
    public TextMeshProUGUI currentWeapon;
    public TextMeshProUGUI countdownTimerText;


    public TextMeshProUGUI esc;
    public TextMeshProUGUI exfil2;

    public TextMeshProUGUI deathText;
    public TextMeshProUGUI exfilText;


    public TextMeshProUGUI pickUpText;
    public TextMeshProUGUI switchWeaponText;
    public TextMeshProUGUI switchWeaponHintText;



    [SerializeField] private Text[] ammoCountTexts;

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
        CheckSceneAndDisableUI();
    }

    void Update()
    {
        UpdateKeyCountText();
    }
    public void ShowExfilMessage()
    {
        if (exfilText != null)
        {
            exfilText.text = "Data Upload complete. Head back to chopper for exfil";
            exfilText.gameObject.SetActive(true);

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
            exfilText.gameObject.SetActive(false);
        }
    }


    private void CheckSceneAndDisableUI()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "d")
        {
            if (initialMessageText != null) initialMessageText.gameObject.SetActive(false);
            if (healthRestoredText != null) healthRestoredText.gameObject.SetActive(true);
            if (pickUpText != null) pickUpText.gameObject.SetActive(false);

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
            StartCoroutine(HideInitialUITextAfterDelay(5f));
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
            ammoCountText.text = $"{currentAmmo} / {totalAmmo}";

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
            switchWeaponText.text = "Switched to: " + weaponName;
            switchWeaponText.gameObject.SetActive(true);

            StartCoroutine(HideSwitchWeaponTextAfterDelay(2f));
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
            StartCoroutine(HideTextAfterDelay(switchWeaponHintText, 5f));
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
        CheckSceneAndDisableUI();
    }
    public void TogglePickupText(bool show)
    {
        if (pickUpText != null)
        {
            pickUpText.gameObject.SetActive(show);
        }
    }
    public void ShowCountdownTimer(float remainingTime)
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);
        string timerText = string.Format("{0:0}:{1:00}", minutes, seconds);

        if (countdownTimerText != null)
        {
            countdownTimerText.text = "Time Remaining: " + timerText;
        }
    }
    public void UpdateExfil2Text(string message)
    {
        if (exfil2 != null)
        {
            exfil2.text = message;
            exfil2.gameObject.SetActive(true);

            // Hide the message after a delay (5 seconds in this case)
            StartCoroutine(HideExfil2MessageAfterDelay(5f));
        }
        else
        {
            Debug.LogError("exfil2 TextMeshProUGUI is not assigned in the Inspector!");
        }
    }
    private IEnumerator HideExfil2MessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (exfil2 != null)
        {
            exfil2.gameObject.SetActive(false);  // Hide the message
        }
    }



}

