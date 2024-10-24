using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Include this for UI functionality

public class MinimapFollow : MonoBehaviour
{
    public Transform player;                // Reference to the player
    private Camera minimapCamera;           // Reference to the minimap camera
    public RawImage minimapImage;           // Reference to the RawImage UI element
    public GameObject minimapIcon;          // Reference to the minimap icon GameObject
    public Camera mainCamera;               // Reference to the main camera
    private bool isMinimapActive = true;    // To track if the minimap is active

    void Start()
    {
        minimapCamera = GetComponent<Camera>(); // Get the Camera component attached to this GameObject
        minimapCamera.orthographic = true;       // Ensure the camera starts in orthographic mode
        minimapCamera.orthographicSize = 27;     // Set initial size
        mainCamera.enabled = true;                // Ensure the main camera is enabled initially
    }

    void LateUpdate()
    {
        // Follow the player position while maintaining the current Y-axis position of the camera
        Vector3 newPosition = player.position;
        newPosition.y = transform.position.y; // Keep the minimap camera's Y position constant
        transform.position = newPosition;

        // Check if the "M" key is pressed to toggle the minimap view
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMinimapView();
        }
    }

    void ToggleMinimapView()
    {
        // Toggle between orthographic sizes
        if (minimapCamera.orthographicSize == 27)
        {
            minimapCamera.orthographicSize = 400; // Set the orthographic size to 400
            minimapImage.rectTransform.sizeDelta = new Vector2(400, 400); // Adjust size for the large minimap
            minimapImage.rectTransform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
            minimapIcon.transform.localScale = new Vector3(100f, 100f, 1f); // Scale the icon to 10 times its size

        }
        else
        {
            minimapImage.rectTransform.sizeDelta = new Vector2(180, 180); // Adjust size for the normal minimap
            minimapImage.rectTransform.position = new Vector3(Screen.width - (minimapImage.rectTransform.sizeDelta.x / 2) - 20, Screen.height - (minimapImage.rectTransform.sizeDelta.y / 2) - 20, 0);
            minimapCamera.orthographicSize = 27;
            minimapIcon.transform.localScale = new Vector3(10f, 10f, 10f); // Reset the icon size to original

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

    // Method to be called when the player enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the player entered the trigger
        {
            SwitchCamera(); // Switch to the main camera
        }
    }
}
