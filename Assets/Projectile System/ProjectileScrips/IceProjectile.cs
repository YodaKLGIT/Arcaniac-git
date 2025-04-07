using UnityEngine;

public class IceProjectile : Projectile
{
    public float freezeDuration = 3f; // Duration of freeze effect

    // Override the HandleCollision method to include the freezing effect
    protected override void HandleCollision(Collision co)
    {
        base.HandleCollision(co); // Keep the base damage logic

        // Check if the projectile hit an enemy
        if (co.gameObject.CompareTag("Enemy") && isPlayerProjectile)
        {
            EnemyAI enemy = co.gameObject.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                // Apply freeze effect to the enemy
                enemy.Freeze(freezeDuration);
            }
        }
    }
}
