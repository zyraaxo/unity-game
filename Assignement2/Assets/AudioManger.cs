using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // Singleton instance

    // Define sounds for different game actions
    public AudioClip pickupSound; // Sound for pickups
    public AudioClip jumpSound; // Sound for jumping
    public AudioClip shootSound; // Sound for shooting
    public AudioClip explosionSound; // Sound for explosions
    public AudioClip growlSound; // Sound for growls
    public AudioClip deathSound; // Sound for death
    public AudioClip reloadSound; // Sound for reloading
    public AudioClip bossMusic; // Sound for boss music
    public AudioClip slamSound; // Sound for slam
    public AudioClip zombieSound; // Sound for zombie sounds
    public AudioClip attackAudio; // Sound for boss attack
    public AudioClip hitSound; // Sound for boss attack


    // Additional audio clips for guns
    public AudioClip[] gunSounds; // Array to hold gun sounds

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

    // Method to play a gun sound by index
    public void PlayGunSound(int gunIndex)
    {
        if (gunIndex >= 0 && gunIndex < gunSounds.Length && gunSounds[gunIndex] != null)
        {
            audioSource.PlayOneShot(gunSounds[gunIndex]); // Play the specific gun sound
        }
        else
        {
            Debug.LogWarning("Gun index is out of range or the sound is null.");
        }
    }

    // Method to play a sound
    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    // Method to play looping background music
    public void PlayMusic(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    // Method to stop music
    public void StopMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    // Play boss music when the scene starts
    void Start()
    {
        // Check if bossMusic is assigned and play it
        if (bossMusic != null)
        {
            PlayMusic(bossMusic); // Play boss music on scene start
        }
        else
        {
            Debug.LogError("Boss music AudioClip is not assigned in the AudioManager!");
        }
    }
}
