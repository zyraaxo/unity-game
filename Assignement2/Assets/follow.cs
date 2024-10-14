using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapFollow : MonoBehaviour
{
    public Transform player;

    void LateUpdate()
    {
        // Follow the player position while maintaining the current Y-axis position of the camera
        Vector3 newPosition = player.position;
        newPosition.y = transform.position.y;  // Keep the minimap camera's Y position constant.
        transform.position = newPosition;
    }
}
