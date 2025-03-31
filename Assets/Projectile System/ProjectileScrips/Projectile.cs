using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject impactVFX;
    public AudioClip impactSound;
    private bool collided;
    public float damage = 20f;
    public bool isPlayerProjectile;
    private static bool isParrying = false;

    void OnCollisionEnter(Collision co)
    {
        if (collided) return;
        collided = true;

        HandleCollision(co);

        CreateImpactEffect(co.contacts[0]);

        Destroy(gameObject);
    }

    private void HandleCollision(Collision co)
    {
        if (co.gameObject.CompareTag("Enemy") && isPlayerProjectile)
        {
            ApplyDamage<EnemyAI>(co.gameObject);
        }
        else if (co.gameObject.CompareTag("Player") && !isPlayerProjectile)
        {
            ApplyDamage<PlayerStats>(co.gameObject);
        }
        else if (co.gameObject.CompareTag("Projectile"))
        {
            HandleParry(co.gameObject);
        }
    }

    private void ApplyDamage<T>(GameObject target) where T : MonoBehaviour
    {
        T component = target.GetComponent<T>();
        if (component != null)
        {
            if (component is EnemyAI enemy)
            {
                enemy.TakeDamage(damage);
            }
            else if (component is PlayerStats player)
            {
                player.TakeDamage(damage);
            }
        }
    }

    private void CreateImpactEffect(ContactPoint contact)
    {
        Vector3 impactPosition = contact.point + contact.normal * 0.1f;
        Quaternion impactRotation = Quaternion.LookRotation(contact.normal);
        var impact = Instantiate(impactVFX, impactPosition, impactRotation);

        if (impactSound != null)
        {
            AudioSource.PlayClipAtPoint(impactSound, impactPosition);
        }

        ParticleSystem ps = impact.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            Destroy(impact.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
        }
        else
        {
            Destroy(impact.gameObject, 1f);
        }
    }

    private void HandleParry(GameObject otherProjectileObj)
    {
        Projectile otherProjectile = otherProjectileObj.GetComponent<Projectile>();
        if (otherProjectile != null && isPlayerProjectile != otherProjectile.isPlayerProjectile)
        {
            if (isPlayerProjectile)
            {
                PlayerStats player = FindObjectOfType<PlayerStats>();
                if (player != null)
                {
                    player.Parry(); // Let PlayerStats handle slow motion
                }
            }

            Destroy(otherProjectile.gameObject);
            Destroy(gameObject);
        }
    }


    public void Parry()
    {
        Debug.Log("Parry successful! Time.timeScale: " + Time.timeScale);
        if (!isParrying)
        {
            isParrying = true;
            StartCoroutine(ParrySlowMo());
        }
    }

    private IEnumerator ParrySlowMo()
    {
        Debug.Log("Parry slow-motion started at Time.timeScale: " + Time.timeScale);

        // Almost freeze time
        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        Debug.Log("Time scale set to 0.01");

        yield return new WaitForSecondsRealtime(0.1f);

        Debug.Log("Resetting time scale...");
        ResetTimeScale();
    }

    private void ResetTimeScale()
    {
        Debug.Log("Resetting time scale to normal.");
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        isParrying = false;
    }
}
