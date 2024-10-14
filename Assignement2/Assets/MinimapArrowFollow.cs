using UnityEngine;

public class MinimapArrowFollow : MonoBehaviour
{
    public Transform player;  // Reference to the player object

    void LateUpdate()
    {
        // Set the position of the arrow to match the player’s position
        Vector3 newPosition = player.position;
        newPosition.y = transform.position.y;  // Keep the arrow on the same Y plane as the minimap
        transform.position = newPosition;

        // Rotate the arrow to match the player's forward direction
        transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
    }
}
