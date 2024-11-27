// Scripts/Weapons/Projectile.cs
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float maxRange; // Maximum range of the projectile
    private float speed;    // Speed of the projectile
    private float damage;   // Damage dealt by the projectile
    private Vector3 spawnPosition;

    // Initialize the projectile with damage, range, and speed
    public void Initialize(float damage, float range, float speed)
    {
        this.damage = damage;
        this.maxRange = range;
        this.speed = speed;
        spawnPosition = transform.position;
    }

    private void Update()
    {
        // Move the projectile forward
        transform.position += transform.forward * speed * Time.deltaTime;

        // Destroy the projectile if it exceeds its range
        if (Vector3.Distance(spawnPosition, transform.position) > maxRange)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Apply damage to the enemy
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage((int)damage); // Damage as an integer for compatibility
            }

            // Destroy the projectile on impact
            Destroy(gameObject);
        }
    }
}
