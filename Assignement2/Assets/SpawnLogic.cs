using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawnManager : MonoBehaviour
{
    public Transform spawnPoint; // Assign the spawn point in the new scene

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (spawnPoint != null)
        {
            Transform player = GameObject.FindWithTag("Player").transform;
            if (player != null)
            {
                player.position = spawnPoint.position;
                player.rotation = spawnPoint.rotation;
            }
        }
    }
}
