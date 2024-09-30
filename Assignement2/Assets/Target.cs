using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private int health = 100; // Health of the target

    public void TakeDamage(int damage)
    {
        health -= damage; // Subtract damage from health
        Debug.Log(gameObject.name + " took " + damage + " damage. Remaining health: " + health);

        // Check if health is less than or equal to zero
        if (health <= 0)
        {
            Destroy(gameObject); // Destroy the target if health is depleted
            Debug.Log(gameObject.name + " has been destroyed.");
        }
    }
}
