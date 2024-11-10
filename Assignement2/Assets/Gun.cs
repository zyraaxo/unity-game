using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using System;

public class Gun : MonoBehaviour
{
    private FirstPersonController _input;

    [SerializeField]
    private GameObject BulletPreFab;

    [SerializeField]
    private GunData gunData;

    [SerializeField]
    private Transform bulletPoint;

    public ParticleSystem muzzleFlash;


    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private float bSpeed = 1200f;

    void Start()
    {
        _input = transform.root.GetComponent<FirstPersonController>();
    }

    void Update()
    {
        if (_input.shoot)
        {
            Shoot();
            _input.shoot = false;
            Console.WriteLine("Shot gun");

        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(BulletPreFab, bulletPoint.position, bulletPoint.rotation * Quaternion.Euler(90f, 0f, 0f));
        bullet.GetComponent<Rigidbody>().AddForce(bulletPoint.forward * bSpeed);



        audioSource.Play();
        muzzleFlash.Play();
        Destroy(bullet, 1);
    }

}