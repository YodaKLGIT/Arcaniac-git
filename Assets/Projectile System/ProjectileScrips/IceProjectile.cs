using UnityEngine;

public class IceProjectile : Projectile
{
    public float freezeDuration = 7f; // Example, you can modify this per your needs.

    protected override void TryApplyStatusEffect(GameObject target)
    {
        if (spellData == null || spellData.statusEffect == null) return;

        if (spellData.statusEffect is FreezeEffect freezeEffect)
        {
            // Only apply ice buildup for ice projectiles
            EnemyStatusHandler handler = target.GetComponent<EnemyStatusHandler>();
            if (handler != null)
            {
                handler.AddBuildup(spellData.statusEffect, spellData.statusBuildupAmount); // Ice effect logic
            }
        }
    }
}
