using UnityEngine;
using System;

public class EnemyAttackDP : MonoBehaviour
{
    [SerializeField]
    [Range(0, 5)]
    public int health;

    [SerializeField]
    [Range(0, 10)]
    public int enemyLevel;

    [SerializeField]
    public float attackCooldown = 3.0f; // in seconds

    private float lastAttackTime = -Mathf.Infinity;
    

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && Time.time - lastAttackTime > attackCooldown)
        {
            Debug.Log("Enemy attacking player: " + other.gameObject.name);

            PlayerDP player = other.GetComponent<PlayerDP>();
            if (player != null)
            {
                player.HitByEnemy(enemyLevel);
            }
            lastAttackTime = Time.time;
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
