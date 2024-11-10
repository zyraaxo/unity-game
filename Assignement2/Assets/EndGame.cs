using UnityEngine;

public class EndGameTrigger : MonoBehaviour
{
    private bool playerInTriggerZone = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTriggerZone = true;
            UIManager.Instance?.UpdateExfil2Text("Press 'F' to exfil");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTriggerZone = false;
            UIManager.Instance?.UpdateExfil2Text("");
        }
    }

    private void Update()
    {
        if (playerInTriggerZone && Input.GetKeyDown(KeyCode.F))
        {
            if (Player.Instance.GetKeys() >= 3)
            {
                if (GameManager.Instance.IsDataUploaded())  // Check if data upload is complete
                {
                    UIManager.Instance?.UpdateExfil2Text("Press 'F' to exfil");
                    GameManager.Instance.EndGame();
                }
                else
                {
                    UIManager.Instance?.UpdateExfil2Text("Data not uploaded. Cannot exfil.");
                    Debug.Log("Data upload not complete, cannot exfil.");
                }
            }
            else
            {
                Debug.Log("Not enough keys. You need 3 keys to exfil.");
            }
        }
    }
}
