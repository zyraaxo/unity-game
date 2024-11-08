using UnityEngine;
public class Bullet : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private int damage = 10;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * 30f;
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Bullet hit: " + collision.collider.gameObject.name);

        Destroy(collision.collider.gameObject);

        Destroy(gameObject, 0.1f);
    }

}
