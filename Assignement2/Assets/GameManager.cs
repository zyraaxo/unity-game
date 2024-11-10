using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Player player;

    private bool gameIsOver = false;
    private bool dataUploaded = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !gameIsOver)
        {
            EndGame();
        }
    }

    public void EndGame()
    {
        if (player.GetKeys() >= 3 && dataUploaded)
        {
            gameIsOver = true;
            Debug.Log("Game Over!");

            UIManager.Instance.ShowKeyCheckText("Congrats! You've collected all keys and completed the mission.");
            UIManager.Instance.ShowKeyCheckText("Game Over");

            Invoke("QuitGame", 3f);
        }
        else if (player.GetKeys() < 3)
        {
            UIManager.Instance.ShowKeyCheckText("You need all 3 keys to complete the mission!");
        }
        else if (!dataUploaded)
        {
            UIManager.Instance.ShowKeyCheckText("Data not uploaded yet. Cannot exfil.");
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); 
#endif
    }

    public void RestartGame()
    {
        Debug.Log("Restarting game...");
        gameIsOver = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadScene(string sceneName)
    {
        Debug.Log($"Loading scene: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }

    public void SetDataUploaded(bool value)
    {
        dataUploaded = value;
    }

    public bool IsDataUploaded()
    {
        return dataUploaded;
    }
}
