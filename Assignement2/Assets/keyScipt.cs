using UnityEngine;

public class KeyScript : MonoBehaviour
{
    public float activationDistance = 5f; // Distance within which the key can be activated
    private Player player; // Reference to the Player instance

    private void Start()
    {
        player = Player.Instance;
        UIManager.Instance.HideKeyPickupText(); // Ensure the pickup text is hidden at start
    }

    private void Update()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance < activationDistance)
        {
            ShowPickupText("Press E to pick up the key!");

            if (Input.GetKeyDown(KeyCode.E))
            {
                player.AddKey();
                Debug.Log("Key collected! Total keys: " + player.GetKeys());

                UIManager.Instance.ShowKeyCheckText("Key Collected!"); // Show key collected message

                gameObject.SetActive(false); // Deactivate the key object
                HidePickupText();
            }
        }
        else
        {
            HidePickupText();
        }
    }

    private void ShowPickupText(string message)
    {
        UIManager.Instance.ShowKeyPickupText(message); // Show the pickup text
    }

    private void HidePickupText()
    {
        UIManager.Instance.HideKeyPickupText(); // Hide the pickup text
    }
}
