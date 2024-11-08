using UnityEngine;
//Used for fireball from boss fight scene
public class Fireball : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;
    public GameObject collisionParticle;
    public AudioClip impactSound;
    private AudioSource audioSource;
    private Transform target;

    public float explosionRadius = 5f;
    public LayerMask damageableLayers;

    public void Initialize(Transform player)
    {
        target = player;
    }

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            if (Vector3.Distance(transform.position, target.position) > 20f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (AudioManager.Instance == null)
        {
            Debug.LogError("AudioManager instance is null! Make sure the AudioManager is present in the scene.");
        }
        else if (AudioManager.Instance.explosionSound == null)
        {
            Debug.LogError("expolsionSound is null! Please assign an audio clip in the AudioManager.");
        }
        else
        {
            AudioManager.Instance.PlaySound(AudioManager.Instance.explosionSound);
        }

        if (collisionParticle != null)
        {
            GameObject particleInstance = Instantiate(collisionParticle, transform.position, Quaternion.identity);
            Destroy(particleInstance, 2f);
        }

        Explode();

        Destroy(gameObject);
    }

    private void Explode() //Blast radius damage from fireball impact
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, damageableLayers);

        foreach (Collider nearbyObject in colliders)
        {
            PlayerHealth playerHealth = nearbyObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
