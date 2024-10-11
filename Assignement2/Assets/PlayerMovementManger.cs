using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerMovementManager : MonoBehaviour
{
    public FirstPersonController playerMovement; // Reference to the existing PlayerMovement script
    public AudioSource walkingSound; // Reference to the AudioSource for walking sound
    public AudioSource staminaDepletedSound; // Reference to the AudioSource for stamina depleted sound
    public AudioSource sprintCooldownSound; // Reference to the AudioSource for sprint cooldown sound

    public GameObject regularGun; // The regular gun object
    public GameObject sprintingGun; // The gun to show when sprinting

    public PostProcessVolume postProcessVolume; // Reference to the Post Processing Volume
    private DepthOfField depthOfField; // Reference to the Depth of Field effect

    private bool isWalking = false;
    private bool hasPlayedStaminaSound = false; // To track if the stamina sound has already played
    private bool hasPlayedCooldownSound = false; // To track if the cooldown sound has already played

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

        // Ensure the sprinting gun is hidden at the start
        if (sprintingGun != null)
        {
            sprintingGun.SetActive(false); // Hide sprinting gun
        }
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

            // Check if sprinting and handle gun visibility
            if (playerMovement.enableSprint && playerMovement.isSprinting)
            {
                if (regularGun != null)
                {
                    regularGun.SetActive(false); // Hide the regular gun
                }

                if (sprintingGun != null)
                {
                    sprintingGun.SetActive(true); // Show the sprinting gun
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
                if (regularGun != null)
                {
                    regularGun.SetActive(true); // Show the regular gun
                }

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
        }
    }
}
