using UnityEngine;

public class PlayerKeyManager : MonoBehaviour
{
    public int playerKeys = 0;          // Track the player's keys
    public door doorScript;             // Reference to the Door script

    void OnTriggerEnter(Collider obj)
    {
        if (obj.CompareTag("Player"))
        {
            // Check if the player has 4 or more keys
            if (playerKeys >= 4)
            {
                // If the player has enough keys, allow the door to open
                doorScript.OnTriggerEnter(obj); // Call the door's OnTriggerEnter to open it
            }
            else
            {
                // If the player doesn't have enough keys, use UIManager to display the message
                if (UIManager.Instance != null)
                {
                    UIManager.Instance.keyCheckText.gameObject.SetActive(true);
                    UIManager.Instance.keyCheckText.text = "You need 4 keys to open this door!";
                }
            }
        }
    }

    void OnTriggerExit(Collider obj)
    {
        if (obj.CompareTag("Player"))
        {
            // Close the door
            doorScript.OnTriggerExit(obj);

            // Clear the message when the player leaves
            if (UIManager.Instance != null && UIManager.Instance.keyCheckText != null)
            {
                UIManager.Instance.keyCheckText.gameObject.SetActive(false);
            }
        }
    }
}
