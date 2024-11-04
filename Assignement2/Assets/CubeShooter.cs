using UnityEngine;

public class CubeShooter : MonoBehaviour
{
    public GameObject spherePrefab; // Prefab of the sphere to be fired
    public float fireRate = 3.0f; // Time between shots
    public float detectionRange = 10.0f; // Range to detect the player
    public Transform player; // Reference to the player object
    public float sphereForce = 20.0f; // Force applied to the fired sphere
    public float explosionRadius = 5f; // Radius of the explosion
    public float explosionDamage = 5f; // Damage dealt to the player
    public GameObject explosionEffect; // Assign an explosion effect prefab

    private float nextFireTime = 0f;

    void Update()
    {
        // Check the distance to the player
        if (Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            // Check if it's time to fire
            if (Time.time >= nextFireTime)
            {
                FireSphere();
                nextFireTime = Time.time + fireRate; // Set the next fire time
            }
        }
    }

    void FireSphere()
    {
        if (spherePrefab != null) // Check if spherePrefab is assigned
        {
            // Instantiate the sphere prefab at the cube's position
            GameObject sphere = Instantiate(spherePrefab, transform.position, Quaternion.identity);
            // Get the Rigidbody component of the sphere
            Rigidbody rb = sphere.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Calculate the direction to the player
                Vector3 direction = (player.position - transform.position).normalized;

                // Apply force to the sphere to make it move towards the player
                rb.AddForce(direction * sphereForce, ForceMode.Impulse); // Use Impulse for instant force

                // Play shooting sound
                AudioManager.Instance.PlaySound(AudioManager.Instance.shootSound);

                // Add SphereExplosion component to handle collision and explosion
                SphereExplosion explosionScript = sphere.AddComponent<SphereExplosion>();
                explosionScript.explosionRadius = explosionRadius;
                explosionScript.explosionDamage = explosionDamage;
                explosionScript.explosionEffect = explosionEffect;
            }
        }
        else
        {
            Debug.LogError("Sphere Prefab is not assigned in CubeShooter.");
        }
    }
}


public class SphereExplosion : MonoBehaviour
{
    public float explosionRadius = 5f; // Radius of the explosion
    public float explosionDamage = 25f; // Damage dealt to the player
    public GameObject explosionEffect; // Assign an explosion effect prefab

    void Start()
    {
        // Optional: Destroy the sphere after 2 seconds if it doesn't collide
        Destroy(gameObject, 2f);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the object is the player
        if (collision.collider.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.collider.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(explosionDamage); // Deal damage to the player
            }

            // Trigger explosion effect if assigned
            if (explosionEffect != null)
            {
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
            }

            // Play explosion sound
            AudioManager.Instance.PlaySound(AudioManager.Instance.explosionSound);

            // Destroy the sphere on collision
            Destroy(gameObject);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Visualize the explosion radius in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
