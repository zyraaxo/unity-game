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

    private void Update()
    {
        // Check if the player presses the "E" key
        if (Input.GetKeyDown(KeyCode.E) && !gameIsOver)
        {
            EndGame(); // Call EndGame method when "E" is pressed
        }
    }

    public void EndGame()
    {
        if (!gameIsOver)
        {
            gameIsOver = true;
            Debug.Log("Game Over!");
            UIManager.Instance.ShowKeyCheckText("Game Over"); // Assuming UIManager handles showing text

            // Show Congrats message
            UIManager.Instance.ShowKeyCheckText("Congrats!");

            Invoke("QuitGame", 3f); // Wait for 3 seconds before quitting the game
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in the Unity Editor
#else
        Application.Quit(); // Quit the application in a build
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
