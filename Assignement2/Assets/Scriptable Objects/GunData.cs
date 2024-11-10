using UnityEngine;

[CreateAssetMenu(fileName = "NewGunData", menuName = "Guns/GunData")]
public class GunData : ScriptableObject
{
    public string gunName;
    public GameObject bulletPrefab;
    public ParticleSystem muzzleFlash;
    public int magazineSize = 10;
    public float fireRate = 0.2f;
    public float reloadTime = 2f;
    public float bulletSpread = 0.25f;

    public int gunSoundIndex;
    public int gunIndex; // Unique index for the UI
    public int maxAmmo = 30;

}
