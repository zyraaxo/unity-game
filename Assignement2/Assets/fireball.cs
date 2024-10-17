using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 10f; // Speed of the fireball
    public int damage = 10; // Damage dealt by the fireball

    private Transform target; // Reference to the target (the player)

    // Set the target for the fireball
    public void Initialize(Transform player)
    {
        target = player;
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
        // Check if the fireball collides with the player
        if (collision.gameObject.CompareTag("Player")) // Ensure your player has the "Player" tag
        {
            // Optionally, you can call the TakeDamage method on the player's health component
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(10); // Adjust the damage amount as necessary
            }
        }

        // Destroy the fireball on collision with any object
        Destroy(gameObject);
    }
}
