using UnityEngine;

public class DoorAnimationControl : MonoBehaviour
{
    private Animator doorAnimator;

    void Start()
    {
        // Get the Animator component
        doorAnimator = GetComponent<Animator>();
        // Disable the Animator so it doesn't play automatically
        doorAnimator.enabled = false;
    }

    // Example method to trigger the animation (like after interacting with the door)
    public void OpenDoor()
    {
        doorAnimator.enabled = true; // Enable the animation to play
    }
}
