using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "New Fire Status Effect", menuName = "Status Effects/Fire")]
public class BurnEffect : BaseStatusEffect
{
    public GameObject burnVFXPrefab; // VFX particle prefab for burning effect
    private float burnDamagePercentage = 0.2f; // 10% damage per second

    // Ensure ApplyEffect returns IEnumerator
    public override IEnumerator ApplyEffect(EnemyStatusHandler enemy)
    {
        // Find the enemy's capsule or main body part to attach the VFX
        Transform capsuleTransform = enemy.transform.Find("Capsule"); // Replace "Capsule" with the correct name of the enemy's capsule model if needed

        // Instantiate the burn VFX inside the capsule
        GameObject burnVFX = Instantiate(burnVFXPrefab, capsuleTransform);

        // Optionally adjust the VFX position inside the capsule
        burnVFX.transform.localPosition = Vector3.zero;  // Adjust this to position the VFX inside as needed

        // Apply burn damage over time while the buildup condition is met
        while (enemy.fireBuildup >= buildupThreshold)
        {
            float damage = enemy.enemyAI.maxHealth * burnDamagePercentage * Time.deltaTime;
            enemy.TakeDamage(damage);
            yield return null; // Wait until the next frame
        }

        // Once the condition is no longer met (buildup below threshold), remove the effect
        if (enemy.fireBuildup < buildupThreshold)
        {
            enemy.RemoveEffect(this);  // Removes the effect
        }

        // Reset the fire buildup after the effect is finished
        enemy.fireBuildup = 0f;

        // Destroy the VFX once the effect is finished
        Destroy(burnVFX);
    }

    public override bool CheckTriggerCondition(EnemyStatusHandler enemy)
    {
        // Trigger burning if the buildup threshold is met
        return enemy.fireBuildup >= buildupThreshold;
    }
}
