using UnityEngine;

public class EndGameTrigger : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        // Check if the player is within the trigger area
        if (other.CompareTag("Player"))
        {
            // Check if the player presses the 'E' key
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Check if the player has 3 or more keys
                if (Player.Instance != null && Player.Instance.GetKeys() >= 3)
                {
                    // Call the EndGame method from the GameManager
                    GameManager.Instance.EndGame();
                }
                else
                {
                    Debug.Log("Not enough keys! You need 3 keys to end the game.");
                }
            }
        }
    }
}
