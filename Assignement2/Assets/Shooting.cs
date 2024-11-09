using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // Import this to access scene management

public class Shooting : MonoBehaviour
{
    [SerializeField] public GunData gunData;
    [SerializeField] private Transform bulletSpawnPoint;
    private int currentBullets;
    private int totalAmmo;
    private bool isReloading = false;
    private float nextFireTime = 0f;

    void Start()
    {
        currentBullets = gunData.magazineSize;

        // Check if the active scene is the one where you want unlimited ammo
        if (SceneManager.GetActiveScene().name == "YourSceneName") // Replace "YourSceneName" with your specific scene name
        {
            totalAmmo = int.MaxValue; // Set totalAmmo to unlimited in the specified scene
        }
        else
        {
            totalAmmo = gunData.maxAmmo; // Use normal max ammo in other scenes
        }

        UIManager.Instance.UpdateAmmoCountText(currentBullets, totalAmmo);
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime && currentBullets > 0 && !isReloading)
        {
            nextFireTime = Time.time + gunData.fireRate;
            Shoot();
        }

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

        Instantiate(gunData.bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

        ParticleSystem muzzleFlash = Instantiate(gunData.muzzleFlash, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        muzzleFlash.Play();
        Destroy(muzzleFlash.gameObject, muzzleFlash.main.duration);

        AudioManager.Instance.PlayGunSound(gunData.gunSoundIndex);

        currentBullets--;
        UIManager.Instance.UpdateAmmoCountText(currentBullets, totalAmmo);
    }

    public void SetCurrentGun(GunData newGunData)
    {
        gunData = newGunData;
        currentBullets = gunData.magazineSize;

        // Ensure the ammo logic is consistent when changing guns
        if (SceneManager.GetActiveScene().name == "YourSceneName") // Replace with your scene name
        {
            totalAmmo = int.MaxValue;
        }
        else
        {
            totalAmmo = gunData.maxAmmo;
        }

        UIManager.Instance.UpdateAmmoCountText(currentBullets, totalAmmo);
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        PlayReloadSound();

        yield return new WaitForSeconds(gunData.reloadTime);

        int bulletsToReload = gunData.magazineSize;
        currentBullets = bulletsToReload;

        isReloading = false;
        Debug.Log("Reloaded!");

        UIManager.Instance.UpdateAmmoCountText(currentBullets, totalAmmo);
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

    public void SetAmmo(int newTotalAmmo)
    {
        if (SceneManager.GetActiveScene().name == "d") // Replace with your scene name
        {
            totalAmmo = int.MaxValue; // Keep ammo unlimited in the specified scene
        }
        else
        {
            totalAmmo = newTotalAmmo; // Use the provided ammo amount in other scenes
        }

        currentBullets = Mathf.Min(gunData.magazineSize, totalAmmo);
        UIManager.Instance.UpdateAmmoCountText(currentBullets, totalAmmo);
    }

    public int GetMaxAmmo()
    {
        return gunData.maxAmmo;
    }
}
