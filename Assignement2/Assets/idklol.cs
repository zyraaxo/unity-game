using UnityEngine;
using TMPro;

public class DistanceMarker : MonoBehaviour
{
    public Transform target; // The target object (e.g., portal or key)
    public TextMeshProUGUI distanceText; // Reference to the TextMeshPro object for distance
    public Transform player; // Reference to the player object
    public string markerText = "Distance to Target"; // Default marker text
    public float heightOffset = 2.0f; // Height offset above the target
    public int requiredKeys = 3; // Number of keys needed to activate portal distance

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

            // Control visibility based on player's key count and distance
            if (Player.Instance.GetKeys() >= requiredKeys)
            {
                // Show the marker at any distance once required keys are collected
                distanceText.gameObject.SetActive(true);
            }
            else if (distance < 100f) // Show the marker only within 100 units if required keys are not yet collected
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
