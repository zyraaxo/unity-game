using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI; // For NavMesh functionality
using UnityEngine.UI;

public class NPCMovement : MonoBehaviour
{
    public float moveSpeed = 3.5f; // Normal movement speed of the NPC
    public float runSpeed = 100f; // Running speed of the NPC
    public float changeDirectionInterval = 5f; // How often the NPC changes direction
    public float movementRadius = 10f; // Radius within which the NPC will move
    public float maxHealth = 500f; // Max health of the NPC
    private float currentHealth; // Current health of the NPC
    public GameObject zombieSpawner; // Reference to the ZombieSpawner GameObject
    public GameObject player; // Reference to the player GameObject
    private NavMeshAgent navMeshAgent; // NavMeshAgent reference
    public float attackRange = 5f; // Range within which the boss can attack
    private bool isAttacking = false; // To track if the boss is currently attacking


    public Animator animator; // Reference to the Animator component
    public Image screenFlashImage; // Reference to the UI Image for screen flash

    private Vector3 targetPosition;
    private bool isIdle;
    private bool isDead = false; // Tracks if the NPC is dead
    public Slider healthBar; // Reference to the health bar slider UI element

    // Walking sound variables
    public AudioSource walkingSound; // Reference to the AudioSource for the walking sound
    public AudioSource growlSound;
    public AudioSource deathSound;
    public AudioSource slam;

    public AudioSource warningSound;

    // Random running variables
    private bool isRunning = false; // Tracks if the NPC is currently running
    public float minRunInterval = 10f; // Minimum time between runs
    public float maxRunInterval = 20f; // Maximum time between runs
    public float minRunDuration = 2f; // Minimum duration for running
    public float maxRunDuration = 5f; // Maximum duration for running

    // Damage value for testing
    public float damageAmount = 10f; // Amount of damage to deal when pressing "C"

    void Start()
    {
        animator.SetBool("isWalking", true); // Stop attack animation

        // Initialize health
        currentHealth = maxHealth;
        healthBar.direction = Slider.Direction.RightToLeft;

        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;

        // Get the NavMeshAgent component
        navMeshAgent = GetComponent<NavMeshAgent>();

        // Get the Animator component (assuming it's attached to the same GameObject)
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        // Set the NPC's movement speed
        navMeshAgent.speed = moveSpeed;

        // Set the initial random destination


        // Start random running behavior
        // StartCoroutine(TriggerRandomRunning());
    }

    void Update()
    {
        if (isDead)
            return; // Prevent movement or behavior if the NPC is dead

        // Calculate the distance to the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // Check if the NPC is within attack range
        if (distanceToPlayer <= attackRange)
        {
            // If within attack range, only attack
            if (!isAttacking)
            {
                Attack(); // Start the attack
            }
        }
        else
        {
            // If not within attack range, move towards the player
            if (!isAttacking) // Only move if not attacking
            {
                RunTowardsPlayer(); // Move towards the player
            }
        }

        // Check for damage input
        if (Input.GetKeyDown(KeyCode.C))
        {
            TakeDamage(damageAmount); // Deal damage when "C" is pressed
        }

        // Check if the NPC is near the edge of the movement area
        CheckForEdge();
        PlayWalkingSound();
        SpawnZombies();
    }

    void Attack()
    {
        // Check if already attacking
        if (!isAttacking)
        {
            isAttacking = true; // Set attacking state

            // Play attack animation
            animator.SetBool("isAttacking", true); // Trigger attack animation
            PlayAttackSound();

            // Start the attack animation duration timer
            Invoke("FinishAttack", 1.0f); // Call FinishAttack after 1 second (or adjust as necessary)
        }
    }

    void FinishAttack()
    {
        animator.SetBool("isAttacking", false); // Stop attack animation

        // Reference to the player health script
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>(); // Find the PlayerHealth component in the scene

        // Check if the player is in range (implement your own logic for this)
        if (playerHealth != null && IsPlayerInRange())
        {
            playerHealth.TakeDamage(10); // Damage amount can be adjusted as needed
        }

        // Start the coroutine to wait before moving again
        StartCoroutine(WaitBeforeMoving());
    }

    private System.Collections.IEnumerator WaitBeforeMoving()
    {
        animator.SetBool("isIdle", true);

        // Stand still for 7 seconds
        yield return new WaitForSeconds(7f);

        // After waiting, move towards the player
        RunTowardsPlayer(); // Ensure boss continues moving towards the player

        isAttacking = false; // Reset attacking state
    }

    private bool IsPlayerInRange()
    {
        // Check if the player is in range based on the defined attack range
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        return distanceToPlayer <= attackRange; // Return true if within range
    }
    IEnumerator HoldPositionBeforeMoving()
    {

        yield return new WaitForSeconds(5.0f); // Adjust buffer time as necessary

        // After the buffer time, move towards the player
        RunTowardsPlayer(); // Ensure boss continues moving towards the player

        isAttacking = false; // Reset attacking state
    }








    // Coroutine to trigger random running behavior

    void StartRunning()
    {
        isRunning = true;
        navMeshAgent.speed = runSpeed; // Set speed to running speed
        animator.SetBool("run", true); // Trigger the running animation
        Debug.Log("NPC started running.");
    }

    void StopRunning()
    {
        isRunning = false;
        navMeshAgent.speed = moveSpeed; // Reset speed to walking speed
        animator.SetBool("run", false); // Stop the running animation
        Debug.Log("NPC stopped running.");
    }



    // Play the walking sound when the NPC is moving
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
        if (!slam.isPlaying)
        {
            slam.Play();
        }
    }

    // Stop the walking sound when the NPC is idle
    void StopWalkingSound()
    {
        if (walkingSound.isPlaying)
        {
            walkingSound.Stop();
        }
    }

    // Sets the idle state and updates the Animator
    void SetIdle(bool idle)
    {
        if (isDead)
            return; // No idle if dead

        isIdle = idle;
        if (animator != null)
        {
            animator.SetBool("isIdle", isIdle); // Update the "isIdle" parameter in the Animator
        }
    }

    // Damage function
    public void TakeDamage(float damage)
    {
        if (isDead)
            return; // Can't take damage if dead

        currentHealth -= damage; // Reduce health
        healthBar.value = currentHealth; // Update the health bar slider value to reflect damage

        animator.SetTrigger("getHit"); // Play the get hit animation
        PlayGrowlSound();
        Debug.Log($"NPC took {damage} damage. Current health: {currentHealth}");
        SpawnZombies();

        if (currentHealth <= 0)
        {
            Die(); // Trigger death if health is zero or less
        }
        else
        {
            RunTowardsPlayer(); // Make the boss run towards the player
        }
    }
    void RunTowardsPlayer()
    {
        if (player != null && !isAttacking) // Ensure the boss is not currently attacking
        {
            navMeshAgent.speed = runSpeed; // Set running speed
            navMeshAgent.SetDestination(player.transform.position); // Set destination to player
            animator.SetBool("run", true); // Trigger run animation

            StartCoroutine(StopRunningAfterReachingPlayer()); // Stop running after reaching player
        }
    }

    IEnumerator StopRunningAfterReachingPlayer()
    {
        // Wait until the boss reaches the player
        while (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
        {
            yield return null; // Continue until the boss is close enough
        }

        // Optional: Add a small delay before stopping running
        yield return new WaitForSeconds(0.5f);

        navMeshAgent.speed = moveSpeed; // Reset speed back to normal
        animator.SetBool("run", false); // Stop the run animation




    }



    // Check if the collider is tagged as "Bullet"
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            float damageAmount = 20f;
            TakeDamage(damageAmount);
            Destroy(other.gameObject); // Optionally destroy the bullet after it hits the NPC
        }
    }

    void Die()
    {
        isDead = true;
        animator.SetTrigger("die");
        StopWalkingSound();
        PlayDeathSound();
        navMeshAgent.isStopped = true;
        Destroy(gameObject, 5f); // Destroys the object after 5 seconds
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

    void CheckForEdge()
    {
        // Add logic here to check if the NPC is near the edge of the movement area and react accordingly
    }
}
