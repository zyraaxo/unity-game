using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using System.Collections;


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


    public PostProcessVolume postProcessVolume; // Reference to the Post Processing Volume
    private DepthOfField depthOfField; // Reference to the Depth of Field effect

    private bool isWalking = false;
    private bool hasPlayedStaminaSound = false; // To track if the stamina sound has already played
    private bool hasPlayedCooldownSound = false; // To track if the cooldown sound has already played
    public GameObject gunSpawnPoint; // Reference to the gun spawn point

    // Added for toggling guns
    private bool isUsingRegularGun = true; // To track which gun is currently in use

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
            }
        }
    }
    public void SetNewGun(GameObject newGun)
    {
        if (newGun != null)
        {
            // If there is a current gun, deactivate it and store it as previous gun
            if (currentGun != null)
            {
                previousGun = currentGun; // Save the old gun
                currentGun.SetActive(false); // Deactivate the current gun
            }

            currentGun = newGun; // Set the new gun as the current gun
            currentGun.SetActive(true); // Activate the new gun
            Debug.Log("Switched to new gun: " + newGun.name);
        }
    }

    // Call this method to switch back to the previous gun
    public void SwitchGuns()
    {
        StartCoroutine(SwitchGunsCoroutine());
    }

    private IEnumerator SwitchGunsCoroutine()
    {
        // Get the Animator component of the current gun
        Animator currentGunAnimator = currentGun.GetComponent<Animator>();

        // Trigger the gun switch animation
        if (currentGunAnimator != null)
        {
            currentGunAnimator.SetBool("isSwitching", true);
        }

        // Wait for 2 seconds before switching
        yield return new WaitForSeconds(2f);

        // Swap the guns
        previousGun.SetActive(true); // Activate previous gun
        currentGun.SetActive(false); // Deactivate current gun

        // Swap references
        GameObject temp = currentGun;
        currentGun = previousGun;
        previousGun = temp;

        Debug.Log("Switched back to previous gun: " + currentGun.name);

        // Reset the animation trigger after the switch
        if (currentGunAnimator != null)
        {
            currentGunAnimator.SetBool("isSwitching", false);
        }
    }


    public void PickUpGun(GameObject newGunPickup)
    {
        if (currentGun != null)
        {
            currentGun.SetActive(false); // Deactivate the current gun
        }

        currentGun = newGunPickup; // Set the new gun pickup as the current gun
        currentGun.SetActive(true); // Activate the new gun

        // Set the position and rotation of the new gun to match the pickup's position and rotation
        currentGun.transform.position = transform.position; // Assuming the PlayerMovementManager is on the player
        currentGun.transform.rotation = transform.rotation; // Set rotation to match the player

        Debug.Log("Picked up a new gun: " + newGunPickup.name);
    }
}
