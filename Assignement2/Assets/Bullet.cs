using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private int damage = 10; // Damage value (not used now since we're destroying objects)

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * 30f; // Set initial velocity
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Bullet hit: " + collision.collider.gameObject.name);

        // Destroy the object the bullet hits
        Destroy(collision.collider.gameObject);

        // Optionally destroy the bullet itself after hitting
        Destroy(gameObject, 0.1f); // Delay bullet destruction slightly
    }

}
