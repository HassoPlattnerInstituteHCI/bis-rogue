using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerSimple : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;

    // Armor Class (AC) determines how difficult it is for enemies to hit the player
    public int playerAC = 3;

    private float lastAttackTime = -Mathf.Infinity;
    private float attackCooldown = 3.0f; // in seconds

    public event Action<int> OnHealthChanged;

    private void Awake()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth);
    }

    public void Start()
    {

    }

    public void Update()
    {
        
    }

    // Handles collisions with enemies, food items, and finish point
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            Heal(1);
            Destroy(other.gameObject);
        }
    }

    void OnTriggerStay(Collider other)
    {
        // Attack enemy if cooldown has elapsed
        if (other.gameObject.CompareTag("Enemy") && Time.time - lastAttackTime > attackCooldown)
        {
            Debug.Log("Player attacking enemy: " + other.gameObject.name);

            EnemyAttack enemy = other.GetComponent<EnemyAttack>();
            if (enemy != null)
            {
                enemy.TakeDamage(1);
            }
            lastAttackTime = Time.time;
        }
    }

    public void HitByEnemy(int enemyLevel)
    {
        // roll 1..20 inclusive for attack (original rogue like system)
        int attackRoll = UnityEngine.Random.Range(1, 21);
        if (attackRoll >= (playerAC + ((0 - enemyLevel) + 10) + 1))
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        OnHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Math.Min(maxHealth, currentHealth+amount);
        OnHealthChanged?.Invoke(currentHealth);
    }

    private void Die()
    {
        Debug.Log("Player ist tot");
    }

}
