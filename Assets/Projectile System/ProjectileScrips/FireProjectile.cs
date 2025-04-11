using UnityEngine;

public class FireProjectile : Projectile
{
    public float fireDamagePerSecond = 10f; // Example, you can modify this per your needs.

    protected override void TryApplyStatusEffect(GameObject target)
    {
        if (spellData == null || spellData.statusEffect == null) return;

        if (spellData.statusEffect is BurnEffect burnEffect)
        {
            // Only apply fire buildup for fire projectiles
            EnemyStatusHandler handler = target.GetComponent<EnemyStatusHandler>();
            if (handler != null)
            {
                handler.AddBuildup(spellData.statusEffect, spellData.statusBuildupAmount); // Fire effect logic
            }
        }
    }
}
