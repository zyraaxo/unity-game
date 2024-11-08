using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballSpawner : MonoBehaviour
{
    public GameObject fireballPrefab;
    public Transform bulletPoint;
    public float bSpeed = 20f;
    public AudioSource audioSource;
    public ParticleSystem muzzleFlash;
    private Transform player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;

        StartCoroutine(ShootFireballRoutine());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (player == null)
        {
            Debug.LogWarning("Player not found. Cannot shoot fireball.");
            return;
        }

        GameObject fireball = Instantiate(fireballPrefab, bulletPoint.position, bulletPoint.rotation);

        Rigidbody rb = fireball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 direction = (player.position - bulletPoint.position).normalized;
            rb.AddForce(direction * bSpeed, ForceMode.Impulse);
        }

        if (audioSource != null)
        {
            audioSource.Play();
        }

        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        Destroy(fireball, 10f);
    }

    private IEnumerator ShootFireballRoutine()
    {
        while (true)
        {
            Shoot();
            float waitTime = Random.Range(3f, 10f);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
