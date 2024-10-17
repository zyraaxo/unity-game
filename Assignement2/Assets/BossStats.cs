using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FinalBoss : MonoBehaviour
{
    // Boss properties
    public float maxHealth = 500f;
    private float currentHealth = 400f; // Set initial health to 400 for testing

    public float meleeAttackDamage = 50f;
    public float mageAttackDamage = 40f;

    public float meleeAttackRange = 2f;
    public float mageAttackRange = 10f;

    public float attackCooldown = 2f;
    private bool canAttack = true;

    // Player reference
    public Transform player;

    // For mage attacks
    public GameObject fireballPrefab;
    public Transform fireballSpawnPoint;

    // Health Bar UI
    public Slider healthBar; // Reference to the health bar slider UI element

    void Start()
    {
        currentHealth = 100f; // Set current health to 400 out of 500
        healthBar.maxValue = maxHealth; // Set the slider's max value to maxHealth
        healthBar.value = currentHealth; // Set the slider to reflect the current health (400)
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= meleeAttackRange && canAttack)
            {
                StartCoroutine(MeleeAttack());
            }
            else if (distanceToPlayer <= mageAttackRange && canAttack)
            {
                //StartCoroutine(MageAttack());
            }
        }
    }

    // Melee attack coroutine
    IEnumerator MeleeAttack()
    {
        canAttack = false;
        Debug.Log("Boss performs a melee attack!");
        player.GetComponent<PlayerHealth>().TakeDamage(meleeAttackDamage);
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    // Boss takes damage
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.value = currentHealth; // Update the health bar slider value to reflect damage

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Boss defeated!");
        Destroy(gameObject);
    }
}
