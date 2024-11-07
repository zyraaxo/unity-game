using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class MoveTowardsPlayer : MonoBehaviour
{
    public float speed = 2.0f;
    public ParticleSystem deathParticles; // Reference to the death particle system

    public float attackRange = 5f;
    public float patrolSpeed = 1.0f;
    public float maxDistance = 10.0f;
    public float patrolRadius = 5.0f;
    public AudioClip moveAudio;
    public AudioClip attackAudio;
    private bool canAttack = true;
    public AudioClip deathAudio;
    public int maxHerdSize = 10;
    public AudioClip hitAudio;
    public float health = 100.0f;
    public float damageAmount = 10.0f;
    public float flockingSpeed = 1.0f;
    [SerializeField] GameObject bloodPrefab;
    public float flockingRadius = 5.0f;
    public float alignmentWeight = 1.0f;
    public float cohesionWeight = 1.0f;
    public float separationWeight = 1.5f;

    private Transform player;
    private Animator animator;
    private AudioSource audioSource;
    private Vector3 patrolTarget;
    private NavMeshAgent agent;
    private float stoppingDistance = 2.0f;
    private bool isDead = false;

    void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            agent.speed = speed;
            agent.stoppingDistance = stoppingDistance;
        }

        if (animator != null)
        {
            animator.SetBool("isMoving", false);
            animator.SetBool("isPatrolling", true);
        }

        SetNewPatrolTarget();
    }

    void Update()
    {
        if (player != null && !isDead)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            bool isAttacking = distanceToPlayer <= stoppingDistance;
            bool isMoving = distanceToPlayer > stoppingDistance && distanceToPlayer <= maxDistance;
            bool isPatrolling = !isMoving && !isAttacking;

            if (isAttacking)
            {
                AttackPlayer();
                agent.speed = speed;
            }
            else if (isMoving)
            {
                MoveTowardsPlayerTarget();
                agent.speed = speed;
            }
            else if (isPatrolling)
            {
                Patrol();
                agent.speed = patrolSpeed;
            }

            if (!isAttacking)
            {
                animator.SetBool("isAttacking", false);
            }

            if (!isAttacking && !isMoving)
            {
                ApplyFlocking();
                agent.speed = flockingSpeed;
            }
        }
    }

    void ApplyFlocking()
    {
        Vector3 alignment = Vector3.zero;
        Vector3 cohesion = Vector3.zero;
        Vector3 separation = Vector3.zero;
        int neighborCount = 0;

        Collider[] neighbors = Physics.OverlapSphere(transform.position, flockingRadius);

        foreach (Collider neighbor in neighbors)
        {
            MoveTowardsPlayer otherZombie = neighbor.GetComponent<MoveTowardsPlayer>();

            if (otherZombie != null && otherZombie != this)
            {
                if (neighborCount >= maxHerdSize)
                    break;

                Vector3 toNeighbor = neighbor.transform.position - transform.position;
                alignment += otherZombie.agent.velocity;
                cohesion += neighbor.transform.position;
                separation += toNeighbor / toNeighbor.sqrMagnitude;
                neighborCount++;
            }
        }

        if (neighborCount > 0 && neighborCount <= maxHerdSize)
        {
            alignment = (alignment / neighborCount).normalized * alignmentWeight;
            cohesion = ((cohesion / neighborCount) - transform.position).normalized * cohesionWeight;
            separation = (separation / neighborCount).normalized * separationWeight;

            Vector3 flockingDirection = alignment + cohesion + separation;
            agent.velocity = Vector3.Lerp(agent.velocity, flockingDirection * flockingSpeed, Time.deltaTime);
        }
    }

    void MoveTowardsPlayerTarget()
    {
        if (agent != null)
        {
            agent.SetDestination(player.position);
        }

        if (animator != null)
        {
            animator.SetBool("isMoving", true);
            animator.SetBool("isPatrolling", false);
        }
    }

    void Patrol()
    {
        if (agent != null)
        {
            if (Vector3.Distance(transform.position, patrolTarget) < 0.5f)
            {
                SetNewPatrolTarget();
            }

            agent.SetDestination(patrolTarget);
        }

        if (animator != null)
        {
            animator.SetBool("isMoving", true);
            animator.SetBool("isPatrolling", true);
        }
    }

    void SetNewPatrolTarget()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;
        randomDirection.y = transform.position.y;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, NavMesh.AllAreas))
        {
            patrolTarget = hit.position;
        }
    }

    void AttackPlayer()
    {
        if (animator != null)
        {
            animator.SetBool("isAttacking", true);
        }

        if (agent != null)
        {
            agent.ResetPath();
        }

        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        directionToPlayer.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5.0f);

        Invoke("FinishAttack", 1.0f);
    }

    void FinishAttack()
    {
        animator.SetBool("isAttacking", false);

        if (canAttack)
        {
            PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();

            if (playerHealth != null && IsPlayerInRange())
            {
                playerHealth.TakeDamage(3);
                canAttack = false;
                StartCoroutine(ResetAttackCooldown());
            }
        }
    }

    IEnumerator ResetAttackCooldown()
    {
        yield return new WaitForSeconds(2f);
        canAttack = true;
    }

    bool IsPlayerInRange()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        return distanceToPlayer <= attackRange;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(damageAmount);
            DamageEffects();
            Destroy(other.gameObject);
        }
    }

    void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            StartCoroutine(Die());
        }
    }

    void DamageEffects()
    {
        if (bloodPrefab != null)
        {
            GameObject bloodEffect = Instantiate(bloodPrefab, transform.position, Quaternion.identity);
            Destroy(bloodEffect, 2f);
        }
    }

    IEnumerator Die()
    {
        isDead = true;

        // Play death animation
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // Play death sound
        if (audioSource != null)
        {
            audioSource.clip = deathAudio;
            audioSource.Play();
        }

        // Play death particles if they are assigned
        if (deathParticles != null)
        {
            ParticleSystem particles = Instantiate(deathParticles, transform.position, Quaternion.identity);
            particles.Play();
            Destroy(particles.gameObject, 2f); // Destroy particles after 2 seconds
        }

        // Disable AI movement
        if (agent != null)
        {
            agent.enabled = false;
        }

        // Wait for the animation and particle effect to finish before destroying the zombie
        yield return new WaitForSeconds(3.0f);
        Destroy(gameObject);
    }
}
