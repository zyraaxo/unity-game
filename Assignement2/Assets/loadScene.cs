using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadScene : MonoBehaviour
{
    public string sceneToLoad = "d";

    void Start()
    {
        StartCoroutine(LoadSceneAfterDelay(3f));
    }

    private IEnumerator LoadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Debug.Log("Loading scene: " + sceneToLoad);

        SceneManager.LoadScene(sceneToLoad);
    }
}
