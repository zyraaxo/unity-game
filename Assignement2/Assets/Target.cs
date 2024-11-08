using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private int health = 100;

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log(gameObject.name + " took " + damage + " damage. Remaining health: " + health);

        if (health <= 0)
        {
            Destroy(gameObject);
            Debug.Log(gameObject.name + " has been destroyed.");
        }
    }
}
