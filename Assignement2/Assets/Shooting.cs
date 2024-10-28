using UnityEngine;
using System.Collections;

public class Shooting : MonoBehaviour
{
    [SerializeField] private GunData gunData; // Assign the GunData scriptable object
    [SerializeField] private Transform bulletSpawnPoint;
    private AudioSource audioSource;
    private float nextFireTime = 0f; // Track the next time we can fire

    private int currentBullets;
    private bool isReloading = false;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        currentBullets = gunData.magazineSize; // Initialize with full magazine
        UIManager.Instance.UpdateAmmoCountText(currentBullets, gunData.magazineSize); // Initial update for ammo UI
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime && currentBullets > 0 && !isReloading)
        {
            nextFireTime = Time.time + gunData.fireRate; // Set the next fire time
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            StartCoroutine(Reload());
        }
    }

    void Shoot()
    {
        if (currentBullets <= 0)
        {
            Debug.Log("Out of ammo! Press 'R' to reload.");
            return;
        }

        Instantiate(gunData.bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

        // Instantiate the muzzle flash as a Particle System
        ParticleSystem muzzleFlashInstance = Instantiate(gunData.muzzleFlash, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        muzzleFlashInstance.Play(); // Play the muzzle flash effect
        Destroy(muzzleFlashInstance.gameObject, muzzleFlashInstance.main.duration); // Destroy it after its duration

        PlayGunSound();

        currentBullets--;

        UIManager.Instance.UpdateAmmoCountText(currentBullets, gunData.magazineSize);
    }

    void PlayGunSound()
    {
        if (gunData.gunSound != null)
        {
            audioSource.PlayOneShot(gunData.gunSound);
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        PlayReloadSound();

        yield return new WaitForSeconds(gunData.reloadTime);

        currentBullets = gunData.magazineSize;
        isReloading = false;
        Debug.Log("Reloaded!");

        UIManager.Instance.UpdateAmmoCountText(currentBullets, gunData.magazineSize);
    }

    void PlayReloadSound()
    {
        if (AudioManager.Instance == null || AudioManager.Instance.reloadSound == null)
        {
            Debug.LogError("Missing AudioManager or reload sound!");
        }
        else
        {
            AudioManager.Instance.PlaySound(AudioManager.Instance.reloadSound);
        }
    }
}
