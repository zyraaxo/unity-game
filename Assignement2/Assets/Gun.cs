using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class Gun : MonoBehaviour
{
    private StarterAssetsInputs _input;

    [SerializeField]
    private GameObject BulletPreFab;

    // Reference to the bullet spawn point
    [SerializeField]
    private Transform bulletPoint;


    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private float bSpeed = 1200f; // Ensure this is positive for forward force

    void Start()
    {
        _input = transform.root.GetComponent<StarterAssetsInputs>();
    }

    void Update()
    {
        if (_input.shoot)
        {
            Shoot();
            _input.shoot = false;
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(BulletPreFab, bulletPoint.position, bulletPoint.rotation * Quaternion.Euler(90f, 0f, 0f));
        bullet.GetComponent<Rigidbody>().AddForce(bulletPoint.forward * bSpeed);


        audioSource.Play();
        Destroy(bullet, 1);
    }

}
