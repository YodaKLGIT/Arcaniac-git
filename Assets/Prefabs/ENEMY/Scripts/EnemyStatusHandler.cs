using UnityEngine;
using System.Collections.Generic;

public class EnemyStatusHandler : MonoBehaviour
{
    private Dictionary<BaseStatusEffect, float> effectsBuildup = new Dictionary<BaseStatusEffect, float>();
    private Dictionary<BaseStatusEffect, Coroutine> activeEffects = new Dictionary<BaseStatusEffect, Coroutine>();

    public float fireBuildup = 0f;
    public float iceBuildup = 0f;

    // Make this public or use a getter to access enemyAI
    public EnemyAI enemyAI;

    private void Start()
    {
        // Ensure we have a reference to EnemyAI
        enemyAI = GetComponent<EnemyAI>();
    }

    public void UpdateEffects()
    {
        // Loop through active effects and apply them
        foreach (var effect in activeEffects)
        {
            BaseStatusEffect statusEffect = effect.Key;
            if (statusEffect.CheckTriggerCondition(this))
            {
                if (!activeEffects.ContainsKey(statusEffect))
                {
                    statusEffect.ApplyEffect(this);
                }
            }
        }
    }

    public void AddBuildup(BaseStatusEffect effect, float amount)
    {
        // Apply the buildup to the respective status effect
        if (effect is FreezeEffect)
        {
            iceBuildup += amount;
            iceBuildup = Mathf.Clamp(iceBuildup, 0f, 100f); // Clamping the buildup to 100

            // Check if the buildup threshold is met for triggering the effect
            if (effect.CheckTriggerCondition(this) && !activeEffects.ContainsKey(effect))
            {
                // Apply the effect if the threshold is met
                ApplyEffect(effect);
            }
        }
        else if (effect is BurnEffect)
        {
            fireBuildup += amount;
            fireBuildup = Mathf.Clamp(fireBuildup, 0f, 100f); // Clamping the buildup to 100

            // Check if the buildup threshold is met for triggering the effect
            if (effect.CheckTriggerCondition(this) && !activeEffects.ContainsKey(effect))
            {
                // Apply the effect if the threshold is met
                ApplyEffect(effect);
            }
        }
    }

    public void RemoveEffect(BaseStatusEffect effect)
    {
        if (activeEffects.ContainsKey(effect))
        {
            StopCoroutine(activeEffects[effect]);
            activeEffects.Remove(effect);
        }
    }

    public void ApplyEffect(BaseStatusEffect effect)
    {
        // Check if the effect is already active to avoid duplicate applications
        if (activeEffects.ContainsKey(effect))
            return;

        // Start the coroutine and store the reference to the coroutine
        Coroutine effectCoroutine = StartCoroutine(effect.ApplyEffect(this));

        // Add the effect and coroutine to the activeEffects dictionary
        activeEffects.Add(effect, effectCoroutine);
    }

    public void TakeDamage(float damage)
    {
        enemyAI.TakeDamage(damage);
    }

    // You can add any other methods related to the status effects here
}
