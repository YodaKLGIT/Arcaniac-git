using UnityEngine;
using TMPro;
using UnityEngine.UI; // For TextMeshPro

public class PlayerHealthbar : MonoBehaviour
{
    [SerializeField] private Image _healthbarSprite;
    [SerializeField] private Image _shieldbarSprite;
    [SerializeField] private float _reduceSpeed = 2;

    [SerializeField] private TMP_Text healthText; // TextMeshPro reference
    private float _targetHealth = 1;
    private float _targetShield = 1;

    private float _maxHealth;
    private float _currentHealth;
    private float _maxShield;
    private float _currentShield;

    void Start()
    {
        _maxHealth = 100; // Default values (can be set from PlayerStats)
        _currentHealth = 100;
        _maxShield = 50;
        _currentShield = 50;
    }

    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        _maxHealth = maxHealth;
        _currentHealth = currentHealth;
        _targetHealth = currentHealth / maxHealth;
        UpdateHealthText();
    }

    public void UpdateShieldBar(float maxShield, float currentShield)
    {
        _maxShield = maxShield;
        _currentShield = currentShield;
        _targetShield = (maxShield > 0) ? currentShield / maxShield : 0; // Avoid division by zero
        UpdateHealthText();
    }

    private void Update()
    {
        // Smoothly update health and shield bars
        _healthbarSprite.fillAmount = Mathf.MoveTowards(_healthbarSprite.fillAmount, _targetHealth, _reduceSpeed * Time.deltaTime);
        _shieldbarSprite.fillAmount = Mathf.MoveTowards(_shieldbarSprite.fillAmount, _targetShield, _reduceSpeed * Time.deltaTime);
    }

    private void UpdateHealthText()
    {
        if (healthText != null)
        {
            float totalHealth = _currentHealth + _currentShield;
            float totalMax = _maxHealth + _maxShield;
            healthText.text = $"{totalHealth}/{totalMax}"; // Display combined health + shield
        }
    }
}
