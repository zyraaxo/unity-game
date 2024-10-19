using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // Singleton instance

    public AudioClip pickupSound; // Sound for pickups
    public AudioClip jumpSound; // Sound for jumping
    public AudioClip shootSound; // Sound for shooting
    // Add more audio clips as needed

    private AudioSource audioSource; // AudioSource to play sounds

    void Awake()
    {
        // Implement singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: persist through scene loads
        }
        else
        {
            Destroy(gameObject);
        }

        // Get or add an AudioSource component
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    // Method to play a sound
    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
