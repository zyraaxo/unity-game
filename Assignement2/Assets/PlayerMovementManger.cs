using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovementManager : MonoBehaviour
{
    public FirstPersonController playerMovement;
    public AudioSource walkingSound;
    public AudioSource staminaDepletedSound;
    public AudioSource sprintCooldownSound;
    public GameObject regularGun;
    public GameObject sprintingGun;
    public GameObject currentGun;
    private GameObject previousGun;
    public GameObject switchWeaponText;
    public PostProcessVolume postProcessVolume;
    private DepthOfField depthOfField;
    private bool isWalking = false;
    private bool hasPlayedStaminaSound = false;
    private bool hasPlayedCooldownSound = false;
    public GameObject gunSpawnPoint;
    private bool isUsingRegularGun = true;
    public GunData currentGunData;
    public List<GameObject> availableGuns = new List<GameObject>();
    private int currentGunIndex = 0;

    public GunData GetCurrentGunData()
    {
        return currentGunData;
    }

    void Start()
    {
        if (playerMovement == null)
        {
            playerMovement = GetComponent<FirstPersonController>();
        }

        if (postProcessVolume != null)
        {
            postProcessVolume.profile.TryGetSettings(out depthOfField);
        }

        if (sprintingGun != null)
        {
            sprintingGun.SetActive(false);
        }

        currentGun = regularGun;
        currentGun.SetActive(true);
        availableGuns.Add(currentGun);

        UpdateWeaponText();  // Update weapon text on start
    }

    void Update() {

        if (Input.GetKeyDown(KeyCode.Tab)) {
           // SwitchGuns();
        }

        if (playerMovement != null && playerMovement.playerCanMove)
        {
            Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            if ((targetVelocity.x != 0 || targetVelocity.z != 0) && playerMovement.isGrounded)
            {
                isWalking = true;
            }
            else
            {
                isWalking = false;
            }

            if (isWalking)
            {
                if (walkingSound != null && !walkingSound.isPlaying)
                {
                    walkingSound.Play();
                }
            }
            else
            {
                if (walkingSound != null && walkingSound.isPlaying)
                {
                    walkingSound.Stop();
                }
            }

            if (playerMovement.enableSprint && playerMovement.isSprinting)
            {
                if (currentGun != null) currentGun.SetActive(false);
                if (sprintingGun != null) sprintingGun.SetActive(false);

                if (playerMovement.sprintRemaining <= 0)
                {
                    if (!hasPlayedStaminaSound && staminaDepletedSound != null)
                    {
                        staminaDepletedSound.Play();
                        hasPlayedStaminaSound = true;
                    }

                    if (depthOfField != null)
                    {
                        depthOfField.active = true;
                        depthOfField.focusDistance.value = 0.5f;
                        depthOfField.aperture.value = 20f;
                    }
                }
                else
                {
                    hasPlayedStaminaSound = false;

                    if (depthOfField != null)
                    {
                        depthOfField.active = false;
                    }
                }
            }
            else
            {
                if (currentGun != null) currentGun.SetActive(true);
                if (sprintingGun != null) sprintingGun.SetActive(false);

                hasPlayedStaminaSound = false;
            }
        }
    }


    public void SetNewGun(GameObject newGun)
    {
        if (newGun != null)
        {
            if (currentGun != null)
            {
                currentGun.SetActive(false);
            }

            currentGun = newGun;
            currentGun.SetActive(true);
            availableGuns.Add(newGun);

            UpdateWeaponText();  // Update weapon text when a new gun is picked up
        }
    }

    public void PickUpGun(GameObject newGunPickup)
    {
        if (newGunPickup != null)
        {
            availableGuns.Add(newGunPickup);

            if (currentGun != null)
            {
                currentGun.SetActive(false);
            }

            currentGun = newGunPickup;
            currentGun.SetActive(true);
            currentGun.transform.position = gunSpawnPoint.transform.position;
            currentGun.transform.rotation = gunSpawnPoint.transform.rotation;

            UpdateWeaponText();  // Update weapon text when a new gun is picked up
        }
    }

    public void SwitchGuns()
    {
        if (availableGuns.Count > 1)
        {
            // Disable current gun
            currentGun.SetActive(false);

            // Update current gun index
            currentGunIndex = (currentGunIndex + 1) % availableGuns.Count;
            currentGun = availableGuns[currentGunIndex];
            currentGun.SetActive(true);

            // Update ammo for the new gun
            Shooting shootingScript = currentGun.GetComponent<Shooting>();
            if (shootingScript != null)
            {
                shootingScript.SetCurrentGun(shootingScript.gunData); // Update the new gun's ammo
            }

            // Update weapon text
            UpdateWeaponText();  // Update weapon text after switching
        }

    }

    private void UpdateWeaponText()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateWeaponText(currentGun.name);  // Use UIManager to update the weapon text
        }
    }
}
