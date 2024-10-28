using UnityEngine;

[CreateAssetMenu(fileName = "NewGunData", menuName = "Guns/GunData")]
public class GunData : ScriptableObject
{
    public string gunName;
    public GameObject bulletPrefab;
    public ParticleSystem muzzleFlash; // Use ParticleSystem directly
    public AudioClip gunSound;
    public int magazineSize = 10;
    public float fireRate = 0.2f;
    public float reloadTime = 2f;
}
