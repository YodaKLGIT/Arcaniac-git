using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "New Ice Status Effect", menuName = "Status Effects/Ice")]
public class FreezeEffect : BaseStatusEffect
{
    public float freezeDuration = 7f;
    public GameObject freezeVFXPrefab;

    public override IEnumerator ApplyEffect(EnemyStatusHandler enemy)
    {
        // Find the enemy's capsule or main body part to attach the VFX
        Transform capsuleTransform = enemy.transform.Find("Capsule");

        // Instantiate the freeze VFX inside the capsule
        GameObject freezeVFX = Instantiate(freezeVFXPrefab, capsuleTransform);

        // Optionally adjust the VFX position inside the capsule
        freezeVFX.transform.localPosition = Vector3.zero;

        // Stop enemy movement for the freeze duration
        enemy.enemyAI.Freeze(freezeDuration);

        yield return new WaitForSeconds(freezeDuration);
        enemy.enemyAI.Unfreeze();

        // Reset the ice buildup after the freeze effect finishes
        enemy.iceBuildup = 0f;

        // Remove the effect from the enemy
        enemy.RemoveEffect(this);

        // Destroy the VFX once the effect is finished
        Destroy(freezeVFX);
    }

    public override bool CheckTriggerCondition(EnemyStatusHandler enemy)
    {
        // Trigger freeze if the buildup threshold is met
        return enemy.iceBuildup >= buildupThreshold;
    }
}
