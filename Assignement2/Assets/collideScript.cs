using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public Camera mainCamera;
    public Camera minimapCamera;
    private bool isMinimapActive = true;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SwitchCamera();
            Debug.Log("Player Entered door");
        }
    }

    void SwitchCamera()
    {
        if (isMinimapActive)
        {
            minimapCamera.enabled = false;
            mainCamera.enabled = true;
            isMinimapActive = false;
        }
        else
        {
            minimapCamera.enabled = true;
            mainCamera.enabled = false;
            isMinimapActive = true;
        }
    }
}
