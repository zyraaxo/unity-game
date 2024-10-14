using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class MoveTowardsPlayer : MonoBehaviour
{
    public float speed = 2.0f; // Speed at which the objects move towards the player
    public float patrolSpeed = 1.0f; // Speed when patrolling
    public float maxDistance = 10.0f; // Maximum distance at which the object will move towards the player
    public float patrolRadius = 5.0f; // Radius within which the zombie will patrol
    public AudioClip moveAudio; // Audio clip for moving
    public AudioClip attackAudio; // Audio clip for attacking
    public AudioClip deathAudio; // Audio clip for dying
    public AudioClip hitAudio; // Audio clip for hit sound
    public float health = 100.0f; // Zombie's health
    public float damageAmount = 10.0f; // Amount of damage per bullet hit
    [SerializeField] GameObject bloodPrefab; // Blood prefab

    private Transform player;  // Reference to the player Transform
    private Animator animator; // Reference to the Animator component
    private AudioSource audioSource; // Reference to the AudioSource component
    private Vector3 patrolTarget; // Target position for patrolling
    private NavMeshAgent agent; // NavMeshAgent for pathfinding and movement
    private float stoppingDistance = 1.0f; // Distance at which the object stops moving
    private bool isDead = false; // Check if the zombie is dead

    void Start()
    {
        // Find the player by tag
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        // Get the Animator component attached to this object
        animator = GetComponent<Animator>();

        // Get the AudioSource component attached to this object
        audioSource = GetComponent<AudioSource>();

        // Get the NavMeshAgent component attached to this object
        agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.speed = speed; // Set movement speed for the NavMeshAgent
            agent.stoppingDistance = stoppingDistance; // Set the stopping distance
        }

        // If there is an animator, set it to idle by default
        if (animator != null)
        {
            animator.SetBool("isMoving", false); // Default to idle animation
            animator.SetBool("isPatrolling", true); // Start with patrolling animation
        }

        // Set initial patrol target
        SetNewPatrolTarget();
    }

    void Update()
    {
        if (player != null && !isDead)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Determine the state of the zombie
            bool isMoving = distanceToPlayer > stoppingDistance && distanceToPlayer <= maxDistance;
            bool isAttacking = distanceToPlayer <= stoppingDistance;
            bool isPatrolling = !isMoving && !isAttacking;

            switch (true)
            {
                case bool _ when isAttacking:
                    AttackPlayer();
                    break;

                case bool _ when isMoving:
                    MoveTowardsPlayerTarget();
                    break;

                case bool _ when isPatrolling:
                    Patrol();
                    break;
            }

            if (!isAttacking)
            {
                animator.SetBool("isAttacking", false); // Reset attack state after animation is finished
            }
        }

        if (animator != null && animator.GetBool("isAttacking") && !animator.GetCurrentAnimatorStateInfo(0).IsName("AttackAnimationName"))
        {
            animator.SetBool("isAttacking", false); // Reset attack state after animation is finished
        }
    }

    void MoveTowardsPlayerTarget()
    {
        if (agent != null)
        {
            agent.SetDestination(player.position); // Set the player as the destination
        }

        // Trigger moving animation
        if (animator != null)
        {
            animator.SetBool("isMoving", true);
            animator.SetBool("isPatrolling", false); // Stop patrolling animation
        }
    }

    void Patrol()
    {
        if (agent != null)
        {
            agent.SetDestination(patrolTarget); // Set the patrol target as the destination

            // Check if the zombie has reached the patrol target
            if (Vector3.Distance(transform.position, patrolTarget) < 0.2f)
            {
                SetNewPatrolTarget(); // Set a new target if the current one is reached
            }
        }

        // Trigger patrol animation
        if (animator != null)
        {
            animator.SetBool("isMoving", false); // Stop moving animation
            animator.SetBool("isPatrolling", true); // Start patrolling animation
        }
    }

    void SetNewPatrolTarget()
    {
        // Set a new random patrol target within a defined radius
        patrolTarget = new Vector3(
            transform.position.x + Random.Range(-patrolRadius, patrolRadius),
            transform.position.y,
            transform.position.z + Random.Range(-patrolRadius, patrolRadius)
        );
    }

    void AttackPlayer()
    {
        // Logic to attack the player, e.g., dealing damage
        if (animator != null)
        {
            animator.SetBool("isAttacking", true);
        }

        // Play attack audio
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.clip = attackAudio; // Set the audio clip for attacking
            audioSource.Play(); // Play the attack sound
        }

        // Stop the NavMeshAgent from moving during attack
        if (agent != null)
        {
            agent.ResetPath(); // Stop moving when attacking
        }
    }

    // Handle bullet collision
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object colliding is a bullet
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(damageAmount);

            // Play the hit audio
            PlayHitAudio();

            // Play the blood effect at the zombie's position
            DamageEffects();

            // Destroy the bullet after it hits
            Destroy(other.gameObject);
        }
    }

    // Reduce health and handle death
    void TakeDamage(float amount)
    {
        health -= amount;

        // If health is 0 or less, die
        if (health <= 0f)
        {
            StartCoroutine(Die()); // Call coroutine to handle death animation and destruction
        }
    }

    public void DamageEffects()
    {
        if (bloodPrefab != null)
        {
            GameObject bloodEffect = Instantiate(bloodPrefab, transform.position, Quaternion.identity);
            Destroy(bloodEffect, 2f); // Destroy the blood effect after 2 seconds
        }
    }

    // Play hit audio
    void PlayHitAudio()
    {
        if (hitAudio != null && audioSource != null)
        {
            audioSource.PlayOneShot(hitAudio); // Play the hit sound effect
        }
    }

    // Handle zombie death
    IEnumerator Die()
    {
        isDead = true; // Prevent further actions

        if (animator != null)
        {
            animator.SetTrigger("Die"); // Trigger death animation
        }

        // Play death audio
        if (audioSource != null)
        {
            audioSource.clip = deathAudio; // Set the audio clip for dying
            audioSource.Play(); // Play the death sound
        }

        // Stop the NavMeshAgent from moving
        if (agent != null)
        {
            agent.enabled = false; // Disable the NavMeshAgent
        }

        // Wait for the length of the death animation before destroying the object
        yield return new WaitForSeconds(3.0f);

        // Destroy the zombie GameObject after the death animation has finished
        Destroy(gameObject);
    }
}
