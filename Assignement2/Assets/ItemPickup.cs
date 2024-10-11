using UnityEngine;
using UnityEngine.UI; // Make sure to include this for UI functionality

public class GunPickup : MonoBehaviour
{
    private bool isPlayerInRange = false; // To check if the player is in range
    public GameObject gunModel; // The gun model to pick up
    public GameObject positionReference; // The reference GameObject for positioning the new gun

    // Reference to the UI Text
    public GameObject pickupText; // Assign this in the Inspector

    void Update()
    {
        // Check if the player is in range and pressing the "E" key
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            PickUpGun();
        }

        // Update the pickup text visibility
        pickupText.SetActive(isPlayerInRange);
    }

    // Trigger when the player enters the pickup range
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure the player has the "Player" tag
        {
            isPlayerInRange = true;
        }
    }

    // Trigger when the player leaves the pickup range
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    // Function to handle picking up the gun
    private void PickUpGun()
    {
        // Get the current gun reference from the player
        PlayerMovementManager playerMovement = FindObjectOfType<PlayerMovementManager>();

        if (playerMovement != null)
        {
            GameObject currentGun = playerMovement.currentGun; // Get the current gun

            if (currentGun != null)
            {
                // Deactivate the current gun
                currentGun.SetActive(false);

                // Calculate the spawn position in front of the camera
                Vector3 spawnPosition = Camera.main.transform.position + Camera.main.transform.forward * 1.5f; // Adjust the multiplier as needed

                // Instantiate the new gun at the spawn position
                GameObject newGun = Instantiate(gunModel, spawnPosition, Camera.main.transform.rotation);

                // Set the new gun as a child of the camera
                newGun.transform.SetParent(Camera.main.transform); // Make the new gun a child of the camera

                // Adjust the local position for proper alignment
                newGun.transform.localPosition = new Vector3(0.419999987f, -0.800000012f, 1.14999998f);

                newGun.SetActive(true); // Make the new gun active

                // Update the player's current gun reference
                playerMovement.currentGun = newGun; // Update the player's current gun

                Debug.Log("Gun picked up and replaced!");
            }
        }

        // Optionally, destroy or hide the pickup object
        Destroy(gameObject); // Destroy the pickup object
    }
}
