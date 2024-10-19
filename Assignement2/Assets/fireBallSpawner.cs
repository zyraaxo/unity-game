using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballSpawner : MonoBehaviour
{
    public GameObject fireballPrefab; // Prefab of the fireball to shoot
    public Transform bulletPoint; // The point from which the fireball is shot
    public float bSpeed = 20f; // Speed of the fireball
    public AudioSource audioSource; // Audio source for firing sound
    public ParticleSystem muzzleFlash; // Optional: particle system for muzzle flash effect
    public Transform player; // Reference to the player transform

    void Start()
    {
        // Optional: Start shooting fireballs automatically (e.g., every 2 seconds)
        StartCoroutine(ShootFireballRoutine());
    }

    void Update()
    {
        // Check for input to shoot (e.g., spacebar or any other condition)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Instantiate the fireball at the bullet point's position and rotation
        GameObject fireball = Instantiate(fireballPrefab, bulletPoint.position, bulletPoint.rotation);

        // Get the Rigidbody component of the fireball
        Rigidbody rb = fireball.GetComponent<Rigidbody>();
        if (rb != null && player != null)
        {
            // Calculate direction towards the player
            Vector3 direction = (player.position - bulletPoint.position).normalized;
            rb.AddForce(direction * bSpeed, ForceMode.Impulse); // Shoot the fireball toward the player
        }

        //Play audio and particle effects
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

    // Coroutine to shoot fireballs automatically at intervals
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
