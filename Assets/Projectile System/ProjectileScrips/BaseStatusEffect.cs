using UnityEngine;
using System.Collections;

public abstract class BaseStatusEffect : ScriptableObject
{
    public string effectName;
    public float buildupThreshold = 100f; // The threshold for when the effect triggers
    //public GameObject effectVFX; // Visual effect for the status effect

    // This will be used to apply the status effect to the enemy
    public abstract IEnumerator ApplyEffect(EnemyStatusHandler enemy);

    // This method can be used to check if the status effect should trigger
    public abstract bool CheckTriggerCondition(EnemyStatusHandler enemy);
}
