using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public Material newPortalMaterial;
    private Renderer portalRenderer;
    private bool canActivatePortal = false;
    public float activationDistance = 5f;

    private bool gameEnded = false;
    private bool isUploading = false;
    private bool isHoldingE = false;
    public TerrainObjectSpawner terrainObjectSpawner;

    public GameObject exfilSpot;
    private bool canExfil = false;

    void Start()
    {
        portalRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (Player.Instance != null)
        {
            float distance = Vector3.Distance(Player.Instance.transform.position, transform.position);

            if (distance <= activationDistance)
            {
                if (Player.Instance.GetKeys() >= 3 && !gameEnded)
                {
                    canActivatePortal = true;
                    UIManager.Instance.ShowKeyCheckText("Hold E to upload data");

                    if (Input.GetKey(KeyCode.E))
                    {
                        if (!isUploading && !isHoldingE)
                        {
                            isHoldingE = true;
                            StartCoroutine(UploadDataRoutine());
                        }
                    }
                    else if (isHoldingE)
                    {
                        isHoldingE = false;
                    }
                }
            }
            else
            {
                canActivatePortal = false;
            }

            if (canExfil && Input.GetKeyDown(KeyCode.E) && exfilSpot != null)
            {
                if (GameManager.Instance.IsDataUploaded())
                {
                    EndGameViaExfil();
                }
                else
                {
                    UIManager.Instance.ShowKeyCheckText("You must finish the data upload first!");
                }
            }
        }
    }

    private IEnumerator UploadDataRoutine()
    {
        UIManager.Instance.ShowKeyCheckText("Uploading data...");
        float holdTime = 0f;
        while (holdTime < 3f && Input.GetKey(KeyCode.E))
        {
            holdTime += Time.deltaTime;
            yield return null;
        }

        if (holdTime < 3f)
        {
            UIManager.Instance.ShowKeyCheckText("Upload Cancelled");
            yield break;
        }

        UIManager.Instance.ShowExfilMessage();
        gameEnded = true;

        GameManager.Instance.SetDataUploaded(true);

        if (terrainObjectSpawner != null)
        {
            terrainObjectSpawner.SpawnObjects();
        }

        canExfil = true;
        exfilSpot.SetActive(true);
        StartCoroutine(StartCountdownTimer(180f));
    }

    private IEnumerator StartCountdownTimer(float countdownTime)
    {
        float remainingTime = countdownTime;
        while (remainingTime > 0)
        {
            UIManager.Instance.ShowCountdownTimer(remainingTime);

            int minutes = Mathf.FloorToInt(remainingTime / 60f);
            int seconds = Mathf.FloorToInt(remainingTime % 60f);
            Debug.Log($"Time Remaining: {minutes:0}:{seconds:00}");

            remainingTime -= Time.deltaTime;
            yield return null;
        }

        UIManager.Instance.ShowKeyCheckText("Time's up!");
        GameManager.Instance.EndGame();
    }

    private void EndGameViaExfil()
    {
        UIManager.Instance.ShowKeyCheckText("Exfil Successful!");
        GameManager.Instance.EndGame();
    }
}
