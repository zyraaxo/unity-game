using UnityEngine;
using TMPro; // Ensure you have this namespace imported

public class DistanceMarker : MonoBehaviour
{
    public Transform target; // The target object (e.g., portal or key)
    public TextMeshProUGUI distanceText; // Reference to the TextMeshPro object for distance
    public Transform player; // Reference to the player object
    public string markerText = "Distance to Target"; // Default marker text
    public float heightOffset = 2.0f; // Height offset above the target

    void Update()
    {
        if (target != null && distanceText != null && player != null)
        {
            // Calculate distance to the target
            float distance = Vector3.Distance(player.position, target.position);

            // Update the distance text with markerText and distance
            distanceText.text = markerText + ": " + Mathf.Round(distance) + " units";

            // Position the marker above the target
            Vector3 targetPosition = target.position + new Vector3(0, heightOffset, 0);
            transform.position = targetPosition;

            // Control visibility based on distance (optional)
            if (distance < 100f) // Adjust this value as needed
            {
                distanceText.gameObject.SetActive(true);
            }
            else
            {
                distanceText.gameObject.SetActive(false);
            }
        }
    }
}
