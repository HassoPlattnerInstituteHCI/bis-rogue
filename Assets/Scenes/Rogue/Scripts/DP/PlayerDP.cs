using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerDP : MonoBehaviour
{

    public int maxHealth = 100;
    public int currentHealth;

    public int playerAC = 3;

    private float lastAttackTime = -Mathf.Infinity;
    private float attackCooldown = 2.0f; // in seconds

    public event Action<int> OnHealthChanged;



    private void Awake()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth);
    }
    // Handles collisions with enemies, food items, and finish point
    async void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            Heal(1);
            Destroy(other.gameObject.transform.parent.gameObject);
            SoundManager.Instance.Play("ItemPickup");
        }
        else if (other.gameObject.CompareTag("Finish"))
        {
            Destroy(other.gameObject);
            SoundManager.Instance.Play("LevelComplete");
            await System.Threading.Tasks.Task.Delay(2000);
            RogueManagerDP rogueManager = FindObjectOfType<RogueManagerDP>();
            if (rogueManager != null)
            {
                rogueManager.LevelFinished();
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        // Attack enemy if cooldown has elapsed
        if (other.gameObject.CompareTag("Enemy") && Time.time - lastAttackTime > attackCooldown)
        {
            Debug.Log("Player attacking enemy: " + other.gameObject.name);

            EnemyAttackDP enemy = other.GetComponent<EnemyAttackDP>();
            if (enemy != null)
            {
                enemy.TakeDamage(1);
                SoundManager.Instance.Play("EnemyHit");
            }
            lastAttackTime = Time.time;
        }
    }


    public void HitByEnemy(int enemyLevel)
    {
        // roll 1..20 inclusive for attack (original rogue like system)
        int attackRoll = UnityEngine.Random.Range(1, 21);
        if (attackRoll >= (playerAC + (-enemyLevel + 10) + 1))
        {
            TakeDamage(1);
            SoundManager.Instance.Play("PlayerHit");
        }
    }


    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        OnHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
            return;
        }
        SoundManager.Instance.Play("PlayerHit");
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        OnHealthChanged?.Invoke(currentHealth);
    }

    private void Die()
    {
        Debug.Log("Player ist tot");
        SoundManager.Instance.Play("PlayerDeath");

    }
}