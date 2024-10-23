using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public Camera mainCamera; // Reference to the main camera
    public Camera minimapCamera; // Reference to the minimap camera
    private bool isMinimapActive = true; // To track if the minimap is active

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the player entered the trigger
        {
            SwitchCamera(); // Switch to the main camera
            Debug.Log("Player Entered door");
        }
    }

    void SwitchCamera()
    {
        // Toggle between the minimap and the main camera
        if (isMinimapActive)
        {
            minimapCamera.enabled = false; // Disable the minimap camera
            mainCamera.enabled = true;      // Enable the main camera
            isMinimapActive = false;        // Update the state
        }
        else
        {
            minimapCamera.enabled = true;  // Enable the minimap camera
            mainCamera.enabled = false;     // Disable the main camera
            isMinimapActive = true;         // Update the state
        }
    }
}
