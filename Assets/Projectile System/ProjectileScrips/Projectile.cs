using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject impactVFX;
    public AudioClip impactSound;
    private bool collided;
    public float damage = 20f;
    public bool isPlayerProjectile;
    public SpellData spellData;

    void OnCollisionEnter(Collision co)
    {
        if (collided) return;
        collided = true;

        HandleCollision(co);
        CreateImpactEffect(co.contacts[0]);
        Destroy(gameObject);
    }

    protected virtual void HandleCollision(Collision co)
    {
        if (co.gameObject.CompareTag("Enemy") && isPlayerProjectile)
        {
            // Apply damage and status effect for player projectiles
            ApplyDamage<EnemyAI>(co.gameObject);
            TryApplyStatusEffect(co.gameObject); // Call this method to apply the status effect
        }
        else if (co.gameObject.CompareTag("Player") && !isPlayerProjectile)
        {
            // Apply damage for enemy projectiles hitting the player
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
                enemy.TakeDamage(damage);
            else if (component is PlayerStats player)
                player.TakeDamage(damage);
        }
    }

    protected virtual void TryApplyStatusEffect(GameObject target)
    {
        if (spellData == null || spellData.statusEffect == null) return;

        // The base projectile does not handle fire or ice status effects anymore
        EnemyStatusHandler handler = target.GetComponent<EnemyStatusHandler>();
        if (handler != null)
        {
            handler.AddBuildup(spellData.statusEffect, spellData.statusBuildupAmount);
        }
    }

    private void CreateImpactEffect(ContactPoint contact)
    {
        Vector3 impactPosition = contact.point + contact.normal * 0.1f;
        Quaternion impactRotation = Quaternion.LookRotation(contact.normal);
        var impact = Instantiate(impactVFX, impactPosition, impactRotation);

        AudioClip chosenSound = (spellData != null && spellData.hitSound != null) ? spellData.hitSound : impactSound;
        if (chosenSound != null)
        {
            AudioSource.PlayClipAtPoint(chosenSound, impactPosition);
        }

        ParticleSystem ps = impact.GetComponent<ParticleSystem>();
        Destroy(impact.gameObject, ps ? ps.main.duration + ps.main.startLifetime.constantMax : 1f);
    }

    private void HandleParry(GameObject otherProjectileObj)
    {
        Projectile other = otherProjectileObj.GetComponent<Projectile>();
        if (other != null && isPlayerProjectile != other.isPlayerProjectile)
        {
            if (isPlayerProjectile)
            {
                PlayerStats player = FindObjectOfType<PlayerStats>();
                if (player != null) player.Parry();
            }

            Destroy(otherProjectileObj);
            Destroy(gameObject);
        }
    }
}
