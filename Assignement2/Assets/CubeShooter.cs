using UnityEngine;
//These classes are for the 2nd key room, trap like cubeshooters that fire at player, combined classes to track better
public class CubeShooter : MonoBehaviour
{
    public GameObject spherePrefab;
    public float fireRate = 3.0f;
    public float detectionRange = 10.0f;
    public Transform player;
    public float sphereForce = 20.0f;
    public float explosionRadius = 5f;
    public float explosionDamage = 5f;
    public GameObject explosionEffect;

    private float nextFireTime = 0f;

    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            if (Time.time >= nextFireTime)
            {
                FireSphere();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    void FireSphere()
    {
        if (spherePrefab != null)
        {
            GameObject sphere = Instantiate(spherePrefab, transform.position, Quaternion.identity);
            Rigidbody rb = sphere.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 direction = (player.position - transform.position).normalized;

                rb.AddForce(direction * sphereForce, ForceMode.Impulse);

                AudioManager.Instance.PlaySound(AudioManager.Instance.shootSound);

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
    public float explosionRadius = 5f;
    public float explosionDamage = 25f;
    public GameObject explosionEffect;

    void Start()
    {
        Destroy(gameObject, 2f);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.collider.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(explosionDamage);
            }

            if (explosionEffect != null)
            {
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
            }

            AudioManager.Instance.PlaySound(AudioManager.Instance.explosionSound);

            Destroy(gameObject);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
