using UnityEngine;

public class Portal : MonoBehaviour
{
    public Material newPortalMaterial; // Reference to the new material for the portal
    private Renderer portalRenderer; // Renderer component of the portal
    private bool canActivatePortal = false; // Flag to track if the player can activate the portal
    public float activationDistance = 5f; // Distance within which the portal can be activated

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
                if (Player.Instance.GetKeys() >= 3)
                {
                    canActivatePortal = true; // Allow activation of the portal
                    UIManager.Instance.ShowKeyCheckText("Press E to activate portal"); // Show activation message

                    // Check for player input to activate the portal
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        ChangePortalMaterial();
                    }
                }
                else
                {
                    canActivatePortal = false; // Not enough keys, cannot activate
                    UIManager.Instance.ShowKeyCheckText("Not enough keys!"); // Show message for insufficient keys
                }
            }
            else
            {
                // Player is too far away, clear activation message
                canActivatePortal = false;
                UIManager.Instance.ShowKeyCheckText(""); // Clear the text when too far away
            }
        }
    }

    private void ChangePortalMaterial()
    {
        if (portalRenderer != null && newPortalMaterial != null)
        {
            portalRenderer.material = newPortalMaterial; // Change the portal's material
            Debug.Log("Portal material changed!");
        }
        else
        {
            Debug.LogError("Portal Renderer or newPortalMaterial not assigned!");
        }
    }
}
