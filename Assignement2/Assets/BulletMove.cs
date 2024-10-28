using UnityEngine;

public class BulletMove : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float bulletSpeed = 30f; // Adjust the speed as needed
    [SerializeField] private float additionalForce = 1000f;

    // Move bullet forward
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Set initial velocity of the bullet
        rb.velocity = transform.forward * bulletSpeed;

        // Optionally, apply additional force in the forward direction
        rb.AddForce(transform.forward * additionalForce);
    }

    void Update()
    {
        // Rotate the bullet to align with its velocity direction
        if (rb.velocity != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(rb.velocity);
            transform.rotation = rotation;
        }

        // Perform a raycast in the forward direction
        RaycastHit hit;
        // The raycast length is set to 30.0f
        if (Physics.Raycast(transform.position, transform.forward, out hit, 30.0f))
        {
            // Log the name of the object hit
            Debug.Log("Bullet hit: " + hit.collider.gameObject.name);

            // Destroy the bullet upon hitting any object
            Destroy(gameObject, 2);
        }
    }
}
