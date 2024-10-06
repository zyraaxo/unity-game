using UnityEngine;

public class MoveTowardsPlayer : MonoBehaviour
{
    public float speed = 2.0f; // Speed at which the objects move towards the player
    public float patrolSpeed = 1.0f; // Speed when patrolling
    public float maxDistance = 10.0f; // Maximum distance at which the object will move towards the player
    public float patrolRadius = 5.0f; // Radius within which the zombie will patrol
    public AudioClip moveAudio; // Audio clip for moving
    public AudioClip attackAudio; // Audio clip for attacking
    private Transform player;  // Reference to the player Transform
    private Animator animator; // Reference to the Animator component
    private AudioSource audioSource; // Reference to the AudioSource component
    private Vector3 patrolTarget; // Target position for patrolling
    private float stoppingDistance = 1.0f; // Distance at which the object stops moving

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

        // If there is an animator, set it to idle by default
        if (animator != null)
        {
            animator.SetBool("isMoving", false); // Default to idle animation
            animator.SetBool("isPatrolling", true); // Start with patrolling animation
        }

        // Set initial patrol target
        SetNewPatrolTarget();

        // Configure the audio source
        if (audioSource != null)
        {
            audioSource.clip = moveAudio; // Set the audio clip for moving
            audioSource.loop = true; // Enable looping
        }
    }

    void Update()
    {

        Debug.Log(animator.GetBool("isAttacking"));
        if (player != null)
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

            if (!isAttacking) {
                animator.SetBool("isAttacking", false) ; // Reset attack state after animation is finished
            }
        }

        /*
        if (animator != null && animator.GetBool("isAttacking") && !animator.GetCurrentAnimatorStateInfo(0).IsName("AttackAnimationName"))
        {
            animator.SetBool("isAttacking", false); // Reset attack state after animation is finished
        }*/
    }

    void MoveTowardsPlayerTarget()
    {
        // Calculate the direction to the player
        Vector3 direction = (player.position - transform.position).normalized;

        // Rotate the object to face the player
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * speed);

        // Move towards the player
        transform.position += direction * speed * Time.deltaTime;

        // Trigger moving animation
        if (animator != null)
        {
            animator.SetBool("isMoving", true);
            animator.SetBool("isPatrolling", false); // Stop patrolling animation
        }

        // Play moving audio if not already playing
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play(); // Play the moving sound
        }
    }

    void Patrol()
    {
        // Move towards the patrol target
        Vector3 direction = (patrolTarget - transform.position).normalized;

        // Rotate to face the patrol target
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * patrolSpeed);

        // Move towards the patrol target at patrol speed
        transform.position += direction * patrolSpeed * Time.deltaTime;

        // Check if the zombie has reached the patrol target
        if (Vector3.Distance(transform.position, patrolTarget) < 0.2f)
        {
            SetNewPatrolTarget(); // Set a new target if the current one is reached
        }

        // Trigger patrol animation
        if (animator != null)
        {
            animator.SetBool("isMoving", false); // Stop moving animation
            animator.SetBool("isPatrolling", true); // Start patrolling animation
        }

        // Stop moving audio when patrolling
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop(); // Stop the moving sound when patrolling
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

    void AttackPlayer() {
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
    }
}
