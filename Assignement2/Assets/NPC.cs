using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class NPCMovement : MonoBehaviour
{
    public float moveSpeed = 3.5f;
    public float runSpeed = 100f;
    public float changeDirectionInterval = 5f;
    public float movementRadius = 10f;
    public float maxHealth = 500f;
    private float currentHealth;
    public GameObject zombieSpawner;
    public GameObject player;
    private NavMeshAgent navMeshAgent;
    public float attackRange = 2f;
    private bool isAttacking = false;

    public Animator animator;
    public Image screenFlashImage;

    private Vector3 targetPosition;
    private bool isIdle;
    private bool isDead = false;
    public Slider healthBar;

    public AudioSource walkingSound;
    public AudioSource growlSound;
    public AudioSource deathSound;
    public AudioSource slam;

    public AudioSource warningSound;

    private bool isRunning = false;
    public float minRunInterval = 10f;
    public float maxRunInterval = 20f;
    public float minRunDuration = 2f;
    public float maxRunDuration = 5f;

    public float damageAmount = 10f;

    void Start()
    {
        animator.SetBool("isWalking", true);

        currentHealth = maxHealth;
        healthBar.direction = Slider.Direction.RightToLeft;

        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;

        navMeshAgent = GetComponent<NavMeshAgent>();

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        navMeshAgent.speed = moveSpeed;



        // StartCoroutine(TriggerRandomRunning());
    }

    void Update()
    {
        if (isDead)
            return;

        if (player == null)
        {
            Debug.LogWarning("Player reference is null. Cannot calculate distance or move towards the player.");
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= attackRange)
        {
            if (!isAttacking)
            {
                Attack();
            }
        }
        else
        {
            if (!isAttacking)
            {
                MoveTowardsPlayer();
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            TakeDamage(damageAmount);
        }


        PlayWalkingSound();

        SpawnZombies();
    }


    void Attack()
    {
        if (!isAttacking)
        {
            isAttacking = true;

            animator.SetBool("isAttacking", true);
            PlayAttackSound();

            Invoke("FinishAttack", 1.0f);
        }
    }

    void FinishAttack()
    {
        animator.SetBool("isAttacking", false);

        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();

        if (playerHealth != null && IsPlayerInRange())
        {
            playerHealth.TakeDamage(10);
        }

        StartCoroutine(WaitBeforeMoving());
    }

    private System.Collections.IEnumerator WaitBeforeMoving()
    {
        animator.SetBool("isIdle", true);
        animator.SetBool("isWalking", false);
        yield return new WaitForSeconds(7f);
        //RunTowardsPlayer();
        isAttacking = false;
        animator.SetBool("isWalking", true);
    }

    private bool IsPlayerInRange()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        return distanceToPlayer <= attackRange;
    }













    void PlayWalkingSound()
    {
        if (!walkingSound.isPlaying)
        {
            walkingSound.Play();
        }
    }

    void PlayGrowlSound()
    {
        if (AudioManager.Instance == null)
        {
            Debug.LogError("AudioManager instance is null! Make sure the AudioManager is present in the scene.");
        }
        else if (AudioManager.Instance.growlSound == null)
        {
            Debug.LogError("growlSound is null! Please assign an audio clip in the AudioManager.");
        }
        else
        {
            AudioManager.Instance.PlaySound(AudioManager.Instance.growlSound);
        }
    }

    void PlayDeathSound()
    {
        if (AudioManager.Instance == null)
        {
            Debug.LogError("AudioManager instance is null! Make sure the AudioManager is present in the scene.");
        }
        else if (AudioManager.Instance.deathSound == null)
        {
            Debug.LogError("deathSound is null! Please assign an audio clip in the AudioManager.");
        }
        else
        {
            AudioManager.Instance.PlaySound(AudioManager.Instance.deathSound);
        }
    }
    void PlayAttackSound()
    {
        if (AudioManager.Instance == null)
        {
            Debug.LogError("AudioManager instance is null! Make sure the AudioManager is present in the scene.");
        }
        else if (AudioManager.Instance.slamSound == null)
        {
            Debug.LogError("slamSound is null! Please assign an audio clip in the AudioManager.");
        }
        else
        {
            AudioManager.Instance.PlaySound(AudioManager.Instance.slamSound);
        }
    }

    void StopWalkingSound()
    {
        if (walkingSound.isPlaying)
        {
            walkingSound.Stop();
        }
    }

    void SetIdle(bool idle)
    {
        if (isDead)
            return; // No idle if dead

        isIdle = idle;
        if (animator != null)
        {
            animator.SetBool("isIdle", isIdle);
        }
    }

    // Damage function
    public void TakeDamage(float damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;
        healthBar.value = currentHealth;

        animator.SetTrigger("getHit");
        PlayGrowlSound();
        Debug.Log($"NPC took {damage} damage. Current health: {currentHealth}");
        SpawnZombies();

        if (currentHealth <= 0)
        {
            Die(); // Trigger death if health is zero or less
        }
        else
        {
            //RunTowardsPlayer(); // Make the boss run towards the player
        }
    }


    IEnumerator StopRunningAfterReachingPlayer()
    {
        while (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        navMeshAgent.speed = moveSpeed;
        animator.SetBool("run", false);




    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            float damageAmount = 20f;
            TakeDamage(damageAmount);
            Destroy(other.gameObject);
        }
    }

    void Die()
    {
        isDead = true;
        animator.SetTrigger("die");
        StopWalkingSound();
        PlayDeathSound();
        navMeshAgent.isStopped = true;
        Destroy(gameObject, 5f);
    }

    void SpawnZombies()
    {
        if (currentHealth <= maxHealth / 2.0f && !zombieSpawner.activeSelf)
        {
            warningSound.Play();
            StartCoroutine(DelayedSpawn());
        }
    }

    IEnumerator DelayedSpawn()
    {
        StartCoroutine(FlashScreen());
        yield return new WaitForSeconds(1.0f);
        zombieSpawner.SetActive(true);
        Debug.Log("Zombie spawner activated after sound.");
    }

    IEnumerator FlashScreen()
    {
        float flashDuration = 0.5f;
        float elapsed = 0f;

        while (elapsed < flashDuration)
        {
            elapsed += Time.deltaTime;
            Color newColor = screenFlashImage.color;
            newColor.a = Mathf.Lerp(0, 1, elapsed / flashDuration);
            screenFlashImage.color = newColor;
            yield return null;
        }

        elapsed = 0f;
        while (elapsed < flashDuration)
        {
            elapsed += Time.deltaTime;
            Color newColor = screenFlashImage.color;
            newColor.a = Mathf.Lerp(1, 0, elapsed / flashDuration);
            screenFlashImage.color = newColor;
            yield return null;
        }
    }


    void MoveTowardsPlayer()
    {
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

        navMeshAgent.SetDestination(player.transform.position);

        if (navMeshAgent.velocity.magnitude > 0.1f)
        {
            if (animator != null)
            {
                animator.SetBool("isWalking", true);
            }
        }

    }
}
