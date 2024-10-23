using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] GameObject muzzleFlashPrefab; // Muzzle flash prefab
    [SerializeField] AudioClip gunSound; // Gun sound audio clip
    private AudioSource audioSource; // Reference to the AudioSource
    [SerializeField] private float fireRate = 0.2f; // Fire rate in seconds
    private float nextFireTime = 0f; // Track the next time we can fire
#pragma warning disable CS0436 // Type conflicts with imported type
    FirstPersonController check;
#pragma warning restore CS0436 // Type conflicts with imported type

    void Start()
    {
        // Get the AudioSource component attached to the GameObject
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        // Check if the player is pressing the fire button (mouse button 0) and if enough time has passed
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate; // Set the next fire time
            Shoot(); // Call the shoot method
        }
    }

    void Shoot()
    {

        Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

        // Instantiate the muzzle flash
        GameObject muzzleFlash = Instantiate(muzzleFlashPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Destroy(muzzleFlash, 1f); // Destroy the muzzle flash after 1 second (adjust as needed)


        // Play the gun sound
        PlayGunSound();
    }

    void PlayGunSound()
    {
        if (gunSound != null)
        {
            audioSource.PlayOneShot(gunSound);
        }
    }
}
