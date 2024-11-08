using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private bool gameIsOver = false;

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

    public void EndGame()
    {
        if (!gameIsOver)
        {
            gameIsOver = true;
            Debug.Log("Game Over!");
            UIManager.Instance.ShowKeyCheckText("Game Over");

            Invoke("QuitGame", 3f);
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
}
