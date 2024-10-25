using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class MoveTowardsPlayer : MonoBehaviour
{
    public float speed = 2.0f;
    public float patrolSpeed = 1.0f;
    public float maxDistance = 10.0f;
    public float patrolRadius = 5.0f;
    public AudioClip moveAudio;
    public AudioClip attackAudio;
    public AudioClip deathAudio;
    public int maxHerdSize = 10; // Maximum number of zombies in a herd for flocking behavior

    public AudioClip hitAudio;
    public float health = 100.0f;
    public float damageAmount = 10.0f;
    public float flockingSpeed = 1.0f; // Speed during flocking

    [SerializeField] GameObject bloodPrefab;

    public float flockingRadius = 5.0f; // Radius for detecting nearby zombies for flocking
    public float alignmentWeight = 1.0f;
    public float cohesionWeight = 1.0f;
    public float separationWeight = 1.5f;

    private Transform player;
    private Animator animator;
    private AudioSource audioSource;
    private Vector3 patrolTarget;
    private NavMeshAgent agent;
    private float stoppingDistance = 1.0f;
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
                // Stop flocking and initiate attack
                AttackPlayer();
                agent.speed = speed; // Reset to normal speed when attacking
            }
            else if (isMoving)
            {
                // Move towards the player and stop flocking
                MoveTowardsPlayerTarget();
                agent.speed = speed; // Reset to normal speed when moving towards player
            }
            else if (isPatrolling)
            {
                // Apply patrol behavior if not close to the player
                Patrol();
                agent.speed = patrolSpeed; // Set patrol speed
            }

            // Reset the attacking animation if the player is not in attack range
            if (!isAttacking)
            {
                animator.SetBool("isAttacking", false);
            }

            // Only apply flocking when not close to the player and herd size <= maxHerdSize
            if (!isAttacking && !isMoving)
            {
                ApplyFlocking();
                agent.speed = flockingSpeed; // Set speed for flocking behavior
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
                // Only count neighbors for flocking up to the maxHerdSize
                if (neighborCount >= maxHerdSize)
                    break;

                Vector3 toNeighbor = neighbor.transform.position - transform.position;
                alignment += otherZombie.agent.velocity;
                cohesion += neighbor.transform.position;
                separation += toNeighbor / toNeighbor.sqrMagnitude;
                neighborCount++;
            }
        }

        // Apply flocking only if the herd is within max size
        if (neighborCount > 0 && neighborCount <= maxHerdSize)
        {
            alignment = (alignment / neighborCount).normalized * alignmentWeight;
            cohesion = ((cohesion / neighborCount) - transform.position).normalized * cohesionWeight;
            separation = (separation / neighborCount).normalized * separationWeight;

            Vector3 flockingDirection = alignment + cohesion + separation;
            agent.velocity = Vector3.Lerp(agent.velocity, flockingDirection * flockingSpeed, Time.deltaTime); // Use flocking speed
        }
    }

    void MoveTowardsPlayerTarget()
    {
        if (agent != null)
        {
            agent.SetDestination(player.position);
            PlayGrowlSound();
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
            // If the agent is close to the current patrol target, set a new one
            if (Vector3.Distance(transform.position, patrolTarget) < 0.5f)
            {
                SetNewPatrolTarget();
            }

            agent.SetDestination(patrolTarget); // Move towards the patrol target
        }

        if (animator != null)
        {
            animator.SetBool("isMoving", true); // Ensure moving animation is playing
            animator.SetBool("isPatrolling", true);
        }
    }

    void SetNewPatrolTarget()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius; // Generate a random direction
        randomDirection += transform.position; // Offset by current position
        randomDirection.y = transform.position.y; // Keep the y-level consistent

        // Check if the new patrol target is on the NavMesh to avoid unreachable positions
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, NavMesh.AllAreas))
        {
            patrolTarget = hit.position; // Set a valid position as the new patrol target
        }
    }

    void PlayGrowlSound()
    {
        if (AudioManager.Instance != null && AudioManager.Instance.attackAudio != null)
        {
            AudioManager.Instance.PlaySound(AudioManager.Instance.attackAudio);
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
            agent.ResetPath(); // Stop moving while attacking
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(damageAmount);
            PlayHitAudio();
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

    public void DamageEffects()
    {
        if (bloodPrefab != null)
        {
            GameObject bloodEffect = Instantiate(bloodPrefab, transform.position, Quaternion.identity);
            Destroy(bloodEffect, 2f);
        }
    }

    void PlayHitAudio()
    {
        if (hitAudio != null && audioSource != null)
        {
            audioSource.PlayOneShot(hitAudio);
        }
    }

    IEnumerator Die()
    {
        isDead = true;
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        if (audioSource != null)
        {
            audioSource.clip = deathAudio;
            audioSource.Play();
        }

        if (agent != null)
        {
            agent.enabled = false;
        }

        yield return new WaitForSeconds(3.0f);
        Destroy(gameObject);
    }
}
