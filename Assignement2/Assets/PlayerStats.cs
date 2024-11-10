using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [SerializeField]
    private int keys;

    void Awake()
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

    public void AddKey()
    {
        keys++;
    }

    public int GetKeys()
    {
        return keys;
    }

    public void SetKeys(int value)
    {
        keys = value;
    }
}
