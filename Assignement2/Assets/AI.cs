using UnityEngine;

public class MoveTowardsPlayer : MonoBehaviour
{
    public float speed = 2.0f; // Speed at which the objects move towards the player
    private Transform player;  // Reference to the player Transform
    private Animator animator; // Reference to the Animator component

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

        // If there is an animator, set it to idle by default
        if (animator != null)
        {
            animator.SetBool("isMoving", false); // Default to idle animation
        }
    }

    void Update()
    {
        if (player != null)
        {
            float stoppingDistance = 1.0f; // Distance at which the object stops moving
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer > stoppingDistance)
            {
                // Start moving towards the player
                Vector3 direction = (player.position - transform.position).normalized;
                transform.position += direction * speed * Time.deltaTime;

                // Trigger moving animation
                if (animator != null)
                {
                    animator.SetBool("isMoving", true);
                }
            }
            else
            {
                // Stop moving and return to idle animation
                if (animator != null)
                {
                    animator.SetBool("isMoving", false);
                }
            }
        }
    }
}
