using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Make sure to include this for UI functionality

public class MinimapFollow : MonoBehaviour
{
    public Transform player;
    private Camera minimapCamera; // Reference to the camera
    public RawImage minimapImage; // Reference to the RawImage UI element

    public GameObject minimapIcon; // Reference to the minimap icon GameObject



    void Start()
    {
        minimapCamera = GetComponent<Camera>(); // Get the Camera component attached to this GameObject
        minimapCamera.orthographic = true; // Ensure the camera starts in orthographic mode
        minimapCamera.orthographicSize = 27; // Set initial size

    }

    void LateUpdate()
    {
        // Follow the player position while maintaining the current Y-axis position of the camera
        Vector3 newPosition = player.position;
        newPosition.y = transform.position.y; // Keep the minimap camera's Y position constant
        transform.position = newPosition;

        // Check if the "M" key is pressed
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
            minimapImage.rectTransform.sizeDelta = new Vector2(180, 180); // Adjust size for the large minimap

            minimapImage.rectTransform.position = new Vector3(Screen.width - (minimapImage.rectTransform.sizeDelta.x / 2) - 20, Screen.height - (minimapImage.rectTransform.sizeDelta.y / 2) - 20, 0);

            minimapCamera.orthographicSize = 27;
            minimapIcon.transform.localScale = new Vector3(10f, 10f, 10f); // Reset the icon size to original



        }
    }
}
