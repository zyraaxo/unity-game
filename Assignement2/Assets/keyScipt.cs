using UnityEngine;

public class KeyScript : MonoBehaviour
{
    public float activationDistance = 5f;
    private Player player;

    private void Start()
    {
        player = Player.Instance;
        UIManager.Instance.HideKeyPickupText();
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

                UIManager.Instance.ShowKeyCheckText("Key Collected!");

                gameObject.SetActive(false);
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
        UIManager.Instance.ShowKeyPickupText(message);
    }

    private void HidePickupText()
    {
        UIManager.Instance.HideKeyPickupText();
    }
}
