using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public float maxShield = 50f;
    private float currentShield;

    public AudioSource playerAudioSource;  // Make the AudioSource public
    public AudioClip ParryAudio;  // Parry audio clip exposed in inspector

    [SerializeField] private PlayerHealthbar _healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        currentShield = maxShield;
        _healthBar.UpdateHealthBar(maxHealth, currentHealth);
        _healthBar.UpdateShieldBar(maxShield, currentShield);
    }

    public void TakeDamage(float damage)
    {
        if (currentShield > 0)
        {
            // If shield is active, reduce shield first
            float shieldDamage = Mathf.Min(damage, currentShield);
            currentShield -= shieldDamage;
            damage -= shieldDamage;  // Remaining damage after shield is depleted

            if (damage > 0)
            {
                // If there's any remaining damage, apply it to health
                currentHealth -= damage;
            }
        }
        else
        {
            // If no shield, apply full damage to health
            currentHealth -= damage;
        }

        // Update health and shield bars
        _healthBar.UpdateHealthBar(maxHealth, currentHealth);
        _healthBar.UpdateShieldBar(maxShield, currentShield);

        if (currentHealth <= 0)
        {
            Die();
        }
    }


    public void Parry()
    {
        currentShield += 10f; // Heal 10 shield per parry
        if (currentShield > maxShield) currentShield = maxShield;
        _healthBar.UpdateShieldBar(maxShield, currentShield);

        // Play the parry sound from the public AudioSource
        if (playerAudioSource != null && ParryAudio != null)
        {
            playerAudioSource.PlayOneShot(ParryAudio);  // Play parry sound
        }
    }

    void Die()
    {
        Debug.Log("Player Died!");
        // Add respawn or game over logic here
    }
}
