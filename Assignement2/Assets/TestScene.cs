using UnityEngine;
using UnityEngine.SceneManagement; // For scene management

public class CubePortal : MonoBehaviour
{
    public float activationDistance = 5f; // Distance within which the portal can be activated
    public string sceneToLoad = "Map"; // Name of the scene to load

    private void Update()
    {
        // Check if the player is within the activation distance
        if (Player.Instance != null)  // Assuming you have a singleton Player class
        {
            float distance = Vector3.Distance(Player.Instance.transform.position, transform.position);

            // Check if player is close enough to the cube and presses "E"
            if (distance <= activationDistance)
            {
                // Show message to the player (e.g., with UIManager)

                if (Input.GetKeyDown(KeyCode.E))
                {
                    // Load the new scene when E is pressed
                    LoadNewScene();
                }
            }
            else
            {
                // Hide the activation message when the player is too far
                UIManager.Instance.ShowKeyCheckText("");
            }
        }
    }

    private void LoadNewScene()
    {
        // Load the scene specified in the `sceneToLoad` variable
        SceneManager.LoadScene(sceneToLoad);
    }
}
