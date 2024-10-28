using System.Collections;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private GunData gunData; // Reference to the GunData ScriptableObject
    [SerializeField] private Transform bulletSpawnPoint; // Bullet spawn point
    private int currentBullets;
    private bool isReloading = false; // Prevents shooting while reloading
    private float nextFireTime = 0f; // Track the next time we can fire

    void Start()
    {
        currentBullets = gunData.magazineSize; // Initialize with full magazine
        UIManager.Instance.UpdateAmmoCountText(currentBullets, gunData.magazineSize); // Initial update for ammo UI
    }

    void Update()
    {
        // Check if the player is pressing the fire button (mouse button 0) and if enough time has passed
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime && currentBullets > 0 && !isReloading)
        {
            nextFireTime = Time.time + gunData.fireRate; // Set the next fire time
            Shoot();
        }

        // Check for reload input (R key) and if not already reloading
        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            StartCoroutine(Reload());
        }
    }

    public void Shoot()
    {
        if (currentBullets <= 0)
        {
            Debug.Log("Out of ammo! Press 'R' to reload.");
            return;
        }

        // Spawn bullet
        Instantiate(gunData.bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

        // Play muzzle flash
        ParticleSystem muzzleFlash = Instantiate(gunData.muzzleFlash, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        muzzleFlash.Play();
        Destroy(muzzleFlash.gameObject, muzzleFlash.main.duration);

        // Play the specific gun sound
        AudioManager.Instance.PlayGunSound(gunData.gunSoundIndex); // Ensure gunSoundIndex is set for the current gun

        currentBullets--;
        UIManager.Instance.UpdateAmmoCountText(currentBullets, gunData.magazineSize); // Initial update for ammo UI
    }





    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        PlayReloadSound();

        yield return new WaitForSeconds(gunData.reloadTime); // Wait for reload time

        currentBullets = gunData.magazineSize; // Refill magazine
        isReloading = false;
        Debug.Log("Reloaded!");

        // Update the ammo UI after reloading
        UIManager.Instance.UpdateAmmoCountText(currentBullets, gunData.magazineSize); // Initial update for ammo UI
    }

    void PlayReloadSound()
    {
        if (AudioManager.Instance == null)
        {
            Debug.LogError("AudioManager instance is null! Make sure the AudioManager is present in the scene.");
        }
        else if (AudioManager.Instance.reloadSound == null)
        {
            Debug.LogError("reloadSound is null! Please assign an audio clip in the AudioManager.");
        }
        else
        {
            AudioManager.Instance.PlaySound(AudioManager.Instance.reloadSound);
        }
    }
}
