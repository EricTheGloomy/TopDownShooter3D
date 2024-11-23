// Scripts/Enemy/Enemy.cs
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int enemyHealth = 50; // Default health
    public float movementSpeed = 3f;       // Default speed

    private void Die()
    {
        Debug.Log($"{name} has been defeated!");
        FindObjectOfType<EnemyManager>()?.RemoveEnemy(gameObject);
        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        enemyHealth -= damage;
        if (enemyHealth <= 0)
        {
            Die();
        }
    }
}
