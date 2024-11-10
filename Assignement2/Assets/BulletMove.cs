using UnityEngine;
//Used to fire/ apply fo4rce to bullet
public class BulletMove : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float bulletSpeed = 30f;
    [SerializeField] private float additionalForce = 1000f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * bulletSpeed;

        rb.AddForce(transform.forward * additionalForce);
    }

    void Update()
    {
        if (rb.velocity != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(rb.velocity);
            //transform.rotation = rotation;
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 30.0f))
        {
            Debug.Log("Bullet hit: " + hit.collider.gameObject.name);

            Destroy(gameObject, 2);
        }
    }
}
