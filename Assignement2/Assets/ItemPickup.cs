using UnityEngine;
using UnityEngine.UI; // Ensure to include this for UI functionality

public class GunPickup : MonoBehaviour
{
    private bool isPlayerInRange = false; // To check if the player is in range
    public GameObject gunModel; // The gun model to pick up
    public GameObject positionReference; // The reference GameObject for positioning the new gun

    // Reference to the UI Text
    public GameObject pickupText;

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
            pickupText.SetActive(true); // Show pickup text
        }
    }

    // Trigger when the player leaves the pickup range
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            pickupText.SetActive(false); // Hide pickup text
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
                // Calculate the spawn position in front of the camera
                Vector3 spawnPosition = positionReference.transform.position; // Use positionReference for the new gun spawn

                // Instantiate the new gun at the spawn position
                GameObject newGun = Instantiate(gunModel, spawnPosition, currentGun.transform.rotation);

                // Set the new gun as a child of the camera
                newGun.transform.SetParent(Camera.main.transform); // Make the new gun a child of the camera

                // Adjust the local position for proper alignment
                newGun.transform.localPosition = new Vector3(0.42f, -0.8f, 1.15f); // Adjust as necessary

                // Deactivate the current gun
                currentGun.SetActive(false);

                // Update the player's current gun reference
                playerMovement.SetNewGun(newGun); // Store the new gun reference
                playerMovement.currentGun = newGun; // Update the player's current gun

                Debug.Log("Gun picked up and replaced!");
            }
        }

        // Destroy the pickup object
        Destroy(gameObject); // Optionally, you can choose to keep the pickup object
    }
}
