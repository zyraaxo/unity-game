using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // Add this to use SceneManager

public class Portal : MonoBehaviour
{
    public Material newPortalMaterial; // Reference to the new material for the portal
    private Renderer portalRenderer; // Renderer component of the portal
    private bool canActivatePortal = false; // Flag to track if the player can activate the portal
    public float activationDistance = 5f; // Distance within which the portal can be activated

    private bool gameEnded = false; // Flag to ensure the game ends only once
    private bool isUploading = false; // Flag to track upload state
    private bool isHoldingE = false; // Flag to track if the player is holding 'E'

    // Reference to the TerrainObjectSpawner script
    public TerrainObjectSpawner terrainObjectSpawner;

    void Start()
    {
        // Get the Renderer component attached to the portal
        portalRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        // Check if the player is close enough to the portal
        if (Player.Instance != null)
        {
            float distance = Vector3.Distance(Player.Instance.transform.position, transform.position);

            if (distance <= activationDistance)
            {
                // Player is close enough, check for activation
                if (Player.Instance.GetKeys() >= 3 && !gameEnded)
                {
                    canActivatePortal = true; // Allow activation of the portal
                    UIManager.Instance.ShowKeyCheckText("Hold E to upload data");

                    // Check for 'E' key hold to start the upload process
                    if (Input.GetKey(KeyCode.E))
                    {
                        if (!isUploading && !isHoldingE)
                        {
                            isHoldingE = true; // Start tracking the hold
                            StartCoroutine(UploadDataRoutine()); // Start the upload process
                        }
                    }
                    else if (isHoldingE)
                    {
                        isHoldingE = false; // Reset if 'E' is released before 3 seconds
                    }
                }
            }
            else
            {
                canActivatePortal = false; // Disable portal activation when the player moves away
            }
        }
    }

    private IEnumerator UploadDataRoutine()
    {
        UIManager.Instance.ShowKeyCheckText("Uploading data..."); // Show Uploading message

        // Wait for the player to hold 'E' for 3 seconds
        float holdTime = 0f;
        while (holdTime < 3f && Input.GetKey(KeyCode.E))
        {
            holdTime += Time.deltaTime; // Increment time while 'E' is held
            yield return null; // Wait for the next frame
        }

        // If the player let go of 'E' before 3 seconds, stop the process
        if (holdTime < 3f)
        {
            UIManager.Instance.ShowKeyCheckText("Upload Cancelled"); // Optionally show a cancel message
            yield break; // Exit the coroutine if the upload was cancelled
        }

        // Skip the loading time and immediately show Exfil message
        UIManager.Instance.ShowExfilMessage(); // Show Exfil message
        gameEnded = true; // Ensure it only happens once

        // Call the function to spawn objects after the upload is complete
        if (terrainObjectSpawner != null)
        {
            terrainObjectSpawner.SpawnObjects(); // Call SpawnObjects method from TerrainObjectSpawner
        }

        // Start the 3-minute countdown timer
        StartCoroutine(StartCountdownTimer(180f)); // 3 minutes = 180 seconds
    }

    private IEnumerator StartCountdownTimer(float countdownTime)
    {
        float remainingTime = countdownTime;
        while (remainingTime > 0)
        {
            // Display the countdown on the UI
            UIManager.Instance.ShowCountdownTimer(remainingTime);

            // Log the countdown to the console
            int minutes = Mathf.FloorToInt(remainingTime / 60f);
            int seconds = Mathf.FloorToInt(remainingTime % 60f);
            Debug.Log($"Time Remaining: {minutes:0}:{seconds:00}");

            // Decrement the remaining time
            remainingTime -= Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Timer has completed, take any necessary action here
        UIManager.Instance.ShowKeyCheckText("Time's up!"); // Display a message when the timer ends

        // Call EndGame method from GameManager after timer ends
        GameManager.Instance.EndGame(); // End the game after the timer is up
    }

}
