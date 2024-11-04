using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using System.Collections;
using System.Collections.Generic; // Added to use List

public class PlayerMovementManager : MonoBehaviour
{
    public FirstPersonController playerMovement; // Reference to the existing PlayerMovement script
    public AudioSource walkingSound; // Reference to the AudioSource for walking sound
    public AudioSource staminaDepletedSound; // Reference to the AudioSource for stamina depleted sound
    public AudioSource sprintCooldownSound; // Reference to the AudioSource for sprint cooldown sound

    public GameObject regularGun; // The regular gun object
    public GameObject sprintingGun; // The gun to show when sprinting
    public GameObject currentGun; // Reference to the currently equipped gun
    private GameObject previousGun; // Reference to the previously held gun
    public GameObject switchWeaponText;

    public PostProcessVolume postProcessVolume; // Reference to the Post Processing Volume
    private DepthOfField depthOfField; // Reference to the Depth of Field effect

    private bool isWalking = false;
    private bool hasPlayedStaminaSound = false; // To track if the stamina sound has already played
    private bool hasPlayedCooldownSound = false; // To track if the cooldown sound has already played
    public GameObject gunSpawnPoint; // Reference to the gun spawn point

    // Added for toggling guns
    private bool isUsingRegularGun = true; // To track which gun is currently in use
    public GunData currentGunData; // Store the current gun's GunData

    // Added for multiple weapons management
    public List<GameObject> availableGuns = new List<GameObject>(); // List of available guns
    private int currentGunIndex = 0; // Index of the current gun

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

        // Get the Depth of Field component from the Post Process Volume
        if (postProcessVolume != null)
        {
            postProcessVolume.profile.TryGetSettings(out depthOfField);
            Debug.Log("Depth of Field component found."); // Confirm the component is found
        }
        else
        {
            Debug.LogWarning("PostProcessVolume is not assigned or found!"); // Warn if not found
        }

        // Ensure the guns are hidden at the start
        if (sprintingGun != null)
        {
            sprintingGun.SetActive(false); // Hide sprinting gun
        }

        // Initialize the current gun to the regular gun
        currentGun = regularGun;
        currentGun.SetActive(true); // Show the regular gun at the start
        availableGuns.Add(currentGun); // Add the regular gun to the list
    }

    void Update()
    {
        // Check if the player can move
        if (playerMovement != null && playerMovement.playerCanMove)
        {
            Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            // Determine if the player is walking
            if ((targetVelocity.x != 0 || targetVelocity.z != 0) && playerMovement.isGrounded)
            {
                isWalking = true;
            }
            else
            {
                isWalking = false;
            }

            // Play or stop the walking sound
            if (isWalking && !walkingSound.isPlaying)
            {
                walkingSound.Play(); // Play the walking sound
            }
            else if (!isWalking && walkingSound.isPlaying)
            {
                walkingSound.Stop(); // Stop the walking sound
            }

            // Check if sprinting
            if (playerMovement.enableSprint && playerMovement.isSprinting)
            {
                // Hide both guns when sprinting
                if (currentGun != null)
                {
                    currentGun.SetActive(false); // Hide the current gun
                }

                if (sprintingGun != null)
                {
                    sprintingGun.SetActive(false); // Ensure the sprinting gun is also hidden
                }

                // Check if stamina is depleted
                if (playerMovement.sprintRemaining <= 0)
                {
                    if (!hasPlayedStaminaSound)
                    {
                        staminaDepletedSound.Play(); // Play stamina depleted sound
                        hasPlayedStaminaSound = true; // Prevent replaying the sound
                        Debug.Log("Stamina depleted! Playing sound."); // Log message
                    }

                    // Apply blur effect to indicate low stamina
                    if (depthOfField != null)
                    {
                        depthOfField.active = true; // Activate depth of field
                        depthOfField.focusDistance.value = 0.5f; // Adjust focus distance for blur
                        depthOfField.aperture.value = 20f; // Increase aperture for more blur
                        Debug.Log("Depth of Field activated for low stamina."); // Log message
                    }
                }
                else
                {
                    hasPlayedStaminaSound = false; // Reset when stamina is regained

                    // Reset blur effect when stamina is sufficient
                    if (depthOfField != null)
                    {
                        depthOfField.active = false; // Deactivate depth of field
                        Debug.Log("Depth of Field deactivated; stamina is sufficient."); // Log message
                    }
                }
            }
            else
            {
                // Show the current gun when not sprinting
                if (currentGun != null)
                {
                    currentGun.SetActive(true); // Show the current gun
                }

                // Ensure the sprinting gun is hidden
                if (sprintingGun != null)
                {
                    sprintingGun.SetActive(false); // Hide the sprinting gun
                }

                hasPlayedStaminaSound = false; // Reset when sprinting stops
            }

            // Check if sprint cooldown is active
            if (playerMovement.isSprintCooldown)
            {
                if (!hasPlayedCooldownSound)
                {
                    sprintCooldownSound.Play(); // Play sprint cooldown sound
                    hasPlayedCooldownSound = true; // Prevent replaying the sound
                    Debug.Log("Sprint cooldown active! Playing sound."); // Log message
                }

                // Activate blur effect during cooldown
                if (depthOfField != null)
                {
                    depthOfField.active = true; // Activate depth of field
                    depthOfField.focusDistance.value = 0.5f; // Adjust focus distance for blur
                    depthOfField.aperture.value = 20f; // Increase aperture for more blur
                    Debug.Log("Depth of Field activated for sprint cooldown."); // Log message
                }
            }
            else
            {
                hasPlayedCooldownSound = false; // Reset when cooldown is finished

                // Stop the sprint cooldown sound when cooldown ends
                if (sprintCooldownSound.isPlaying)
                {
                    sprintCooldownSound.Stop(); // Stop cooldown sound
                    Debug.Log("Sprint cooldown ended! Stopping sound."); // Log message
                }

                // Reset blur effect when cooldown is not active
                if (depthOfField != null)
                {
                    depthOfField.active = false; // Deactivate depth of field
                    Debug.Log("Depth of Field deactivated; cooldown finished."); // Log message
                }
            }

            // Check for Tab key press to switch guns
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                SwitchGuns();
                switchWeaponText.SetActive(false);
            }
        }
    }

    public void SetNewGun(GameObject newGun)
    {
        if (newGun != null)
        {
            // If there is a current gun, deactivate it
            if (currentGun != null)
            {
                currentGun.SetActive(false);
            }

            currentGun = newGun; // Set the new gun as the current gun
            currentGun.SetActive(true); // Activate the new gun
            availableGuns.Add(newGun); // Add the new gun to the list
            Debug.Log("Picked up and switched to new gun: " + newGun.name);
        }
    }

    public void PickUpGun(GameObject newGunPickup)
    {
        if (newGunPickup != null)
        {
            // Add the new gun to the list of available guns
            availableGuns.Add(newGunPickup);

            // Set the new gun as the current gun and deactivate the previous one
            if (currentGun != null)
            {
                currentGun.SetActive(false);
            }

            currentGun = newGunPickup;
            currentGun.SetActive(true);
            currentGun.transform.position = gunSpawnPoint.transform.position;
            currentGun.transform.rotation = gunSpawnPoint.transform.rotation;

            Debug.Log("Picked up new gun: " + newGunPickup.name);
        }
    }

    public void SwitchGuns()
    {
        if (availableGuns.Count > 1)
        {
            currentGun.SetActive(false); // Deactivate current gun

            // Cycle to the next gun in the list
            currentGunIndex = (currentGunIndex + 1) % availableGuns.Count;
            currentGun = availableGuns[currentGunIndex];
            currentGun.SetActive(true); // Activate the new current gun

            Debug.Log("Switched to gun: " + currentGun.name);
        }
    }
}
