using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance; // Singleton instance

    [SerializeField]
    private int keys; // Use [SerializeField] to expose in the Inspector

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: persists through scene loads
        }
        else
        {
            Destroy(gameObject); // Prevent duplicate instances
        }
    }

    public void AddKey()
    {
        keys++;
    }

    public int GetKeys()
    {
        return keys;
    }

    // Optional: A method to set keys for testing
    public void SetKeys(int value)
    {
        keys = value;
    }
}
