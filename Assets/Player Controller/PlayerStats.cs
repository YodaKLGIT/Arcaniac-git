using System.Collections;
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

    private bool isParrying = false; // Ensure we don't start multiple coroutines

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
            float shieldDamage = Mathf.Min(damage, currentShield);
            currentShield -= shieldDamage;
            damage -= shieldDamage;

            if (damage > 0)
            {
                currentHealth -= damage;
            }
        }
        else
        {
            currentHealth -= damage;
        }

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

        if (playerAudioSource != null && ParryAudio != null)
        {
            playerAudioSource.PlayOneShot(ParryAudio);
        }

        if (!isParrying)
        {
            isParrying = true;
            StartCoroutine(ParrySlowMo());
        }
    }

    private IEnumerator ParrySlowMo()
    {

        Time.timeScale = 0.1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        yield return new WaitForSecondsRealtime(0.30f);

        ResetTimeScale();
    }

    private void ResetTimeScale()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        isParrying = false;
    }

    void Die()
    {
        Debug.Log("Player Died!");
        // Add respawn or game over logic here
    }
}
