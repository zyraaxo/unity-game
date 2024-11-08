using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // For scene management

public class loadScene : MonoBehaviour
{
    public string sceneToLoad = "d"; // The name of the scene you want to load after 3 seconds

    // Start is called before the first frame update
    void Start()
    {
        // Start the coroutine to load the scene after 3 seconds
        StartCoroutine(LoadSceneAfterDelay(3f)); // 3 seconds delay
    }

    // Coroutine to handle loading the scene after the specified delay
    private IEnumerator LoadSceneAfterDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Log message (optional, for debugging purposes)
        Debug.Log("Loading scene: " + sceneToLoad);

        // Load the specified scene
        SceneManager.LoadScene(sceneToLoad);
    }
}
