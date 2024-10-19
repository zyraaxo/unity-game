using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 10f; // Speed of the fireball
    public int damage = 10; // Damage dealt by the fireball
    public GameObject collisionParticle; // Reference to the particle effect
    public AudioClip impactSound; // Reference to the impact sound
    private AudioSource audioSource; // Audio source to play the sound
    private Transform target; // Reference to the target (the player)

    public float explosionRadius = 5f; // Radius for the explosion effect
    public LayerMask damageableLayers; // Layers that can be damaged by the explosion

    // Set the target for the fireball
    public void Initialize(Transform player)
    {
        target = player;
    }

    void Start()
    {
        // Add an AudioSource component if not already present
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // Don't play the sound on start
    }

    void Update()
    {
        if (target != null)
        {
            // Move towards the target
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // Optional: Destroy the fireball if it gets too far away
            if (Vector3.Distance(transform.position, target.position) > 20f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Play impact sound
        if (impactSound != null)
        {
            audioSource.PlayOneShot(impactSound);
        }

        // Spawn the particle effect
        if (collisionParticle != null)
        {
            GameObject particleInstance = Instantiate(collisionParticle, transform.position, Quaternion.identity);
            Destroy(particleInstance, 2f); // Destroy the particle effect after 1 second
        }

        // Explode and apply damage within the explosion radius
        Explode();

        // Destroy the fireball on collision
        Destroy(gameObject);
    }

    // Explosion logic that applies damage to nearby objects
    private void Explode()
    {
        // Find all objects within the explosion radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, damageableLayers);

        foreach (Collider nearbyObject in colliders)
        {
            // Check if the nearby object is the player or any damageable object
            PlayerHealth playerHealth = nearbyObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage); // Deal damage to the player
            }
        }
    }

    // Optional: Visualize the explosion radius in the Unity editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
