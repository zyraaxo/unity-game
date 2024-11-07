using UnityEngine;

public class PlayerKeyManager : MonoBehaviour
{
    public int playerKeys = 0;
    public door doorScript;

    void OnTriggerEnter(Collider obj)
    {
        if (obj.CompareTag("Player"))
        {
            if (playerKeys >= 4)
            {
                doorScript.OnTriggerEnter(obj);
            }
            else
            {
                if (UIManager.Instance != null)
                {
                    UIManager.Instance.keyCheckText.gameObject.SetActive(true);
                    UIManager.Instance.keyCheckText.text = "You need 3 keys to open this door!";
                }
            }
        }
    }

    void OnTriggerExit(Collider obj)
    {
        if (obj.CompareTag("Player"))
        {
            doorScript.OnTriggerExit(obj);

            if (UIManager.Instance != null && UIManager.Instance.keyCheckText != null)
            {
                UIManager.Instance.keyCheckText.gameObject.SetActive(false);
            }
        }
    }
}
