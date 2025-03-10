using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject impactVFX;
    public AudioClip impactSound;
    private bool collided;
    public float damage = 20f;

    public bool isPlayerProjectile; // Check if the projectile was fired by the player

    void OnCollisionEnter(Collision co)
    {
        if (!collided)
        {
            collided = true;

            // Only apply damage to the player or enemy
            if (co.gameObject.CompareTag("Enemy") && isPlayerProjectile)
            {
                // Player projectile hits the enemy
                EnemyAI enemy = co.gameObject.GetComponent<EnemyAI>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }
            else if (co.gameObject.CompareTag("Player") && !isPlayerProjectile)
            {
                // Enemy projectile hits the player
                PlayerStats player = co.gameObject.GetComponent<PlayerStats>();
                if (player != null)
                {
                    player.TakeDamage(damage);
                }
            }

            // Handle the parry logic (when projectiles hit each other)
            else if (co.gameObject.CompareTag("Projectile"))
            {
                Projectile otherProjectile = co.gameObject.GetComponent<Projectile>();
                if (otherProjectile != null && isPlayerProjectile != otherProjectile.isPlayerProjectile)
                {
                    // Parry triggered, heal player and play the parry sound
                    if (isPlayerProjectile)
                    {
                        PlayerStats player = FindObjectOfType<PlayerStats>();
                        if (player != null)
                        {
                            player.Parry(); // Heal the player upon successful parry

                    
                        }
                    }
                    // Destroy both projectiles on parry
                    Destroy(otherProjectile.gameObject);
                    Destroy(gameObject);
                }
            }

            // Create impact effect
            ContactPoint contact = co.contacts[0];
            Vector3 impactPosition = contact.point + contact.normal * 0.1f;
            Quaternion impactRotation = Quaternion.LookRotation(contact.normal);
            var impact = Instantiate(impactVFX, impactPosition, impactRotation);

            // Play impact sound
            if (impactSound != null)
            {
                AudioSource.PlayClipAtPoint(impactSound, impactPosition);
            }

            // Destroy the VFX after the particle system finishes
            ParticleSystem ps = impact.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                Destroy(impact.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
            }
            else
            {
                Destroy(impact.gameObject, 1f); // Default to 1 second if no ParticleSystem
            }

            // Destroy the projectile itself after collision
            Destroy(gameObject);
        }
    }
}
