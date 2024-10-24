using UnityEngine;
using TMPro; // Make sure to include this if you're using TextMeshPro

public class KeyScript : MonoBehaviour
{
    public float activationDistance = 5f; // Distance within which the key can be activated
    public TextMeshProUGUI uiText; // Reference to the TextMeshProUGUI object for displaying messages
    private Player player; // Reference to the Player instance

    private void Start()
    {
        player = Player.Instance;

        if (uiText != null)
        {
            uiText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance < activationDistance)
        {
            ShowUIText("Press E to pick up the key!");

            if (Input.GetKeyDown(KeyCode.E))
            {
                player.AddKey();
                Debug.Log("Key collected! Total keys: " + player.GetKeys());

                UIManager.Instance.ShowKeyCheckText("Key Collected!");

                gameObject.SetActive(false);
            }
        }
        else
        {
            HideUIText();
        }
    }

    private void ShowUIText(string message)
    {
        if (uiText != null)
        {
            uiText.text = message;
            uiText.gameObject.SetActive(true);
        }
    }

    private void HideUIText()
    {
        if (uiText != null)
        {
            uiText.gameObject.SetActive(false);
        }
    }
}
