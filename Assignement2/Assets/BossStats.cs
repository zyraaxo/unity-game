using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FinalBoss : MonoBehaviour
{
    public float maxHealth = 500f;
    private float currentHealth = 400f;

    public float meleeAttackDamage = 50f;
    public float mageAttackDamage = 40f;

    public float meleeAttackRange = 2f;
    public float mageAttackRange = 10f;

    public float attackCooldown = 2f;
    private bool canAttack = true;

    public Transform player;

    public GameObject fireballPrefab;
    public Transform fireballSpawnPoint;

    public Slider healthBar;
    void Start()
    {
        currentHealth = 100f;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
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

    IEnumerator MeleeAttack()
    {
        canAttack = false;
        Debug.Log("Boss performs a melee attack!");
        player.GetComponent<PlayerHealth>().TakeDamage(meleeAttackDamage);
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.value = currentHealth;

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
