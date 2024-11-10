using UnityEngine;
using UnityEngine.SceneManagement;

//Same as UI manager but for audio in the game, all in one audio controller
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioClip pickupSound;
    public AudioClip jumpSound;
    public AudioClip shootSound;
    public AudioClip explosionSound;
    public AudioClip growlSound;
    public AudioClip deathSound;
    public AudioClip reloadSound;
    public AudioClip bossMusic;
    public AudioClip slamSound;
    public AudioClip zombieSound;
    public AudioClip attackAudio;
    public AudioClip hitSound;

    public AudioClip[] gunSounds;

    public AudioSource audioSource;

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

        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlayGunSound(int gunIndex)
    {
        if (gunIndex >= 0 && gunIndex < gunSounds.Length && gunSounds[gunIndex] != null)
        {
            audioSource.PlayOneShot(gunSounds[gunIndex]);
        }
        else
        {
            Debug.LogWarning("Gun index is out of range or the sound is null.");
        }
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
    public void StopSound(AudioClip clip)
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void StopMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "d")
        {
            if (bossMusic != null)
            {
                PlayMusic(bossMusic);
            }
            else
            {
                Debug.LogError("Boss music AudioClip is not assigned in the AudioManager!");
            }
        }
        else
        {
            StopMusic();
        }
    }
}
