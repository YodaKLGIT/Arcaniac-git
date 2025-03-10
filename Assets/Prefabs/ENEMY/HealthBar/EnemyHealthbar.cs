using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthbar : MonoBehaviour
{
    [SerializeField] private Image _healthbarSprite;
    [SerializeField] private Image _healthbarEffectSprite;
    [SerializeField] private float _reduceSpeed = 2;

    private Camera _cam;
    private float _target = 1;
    private float _target2 = 2;


    void Start()
    {
        _cam = Camera.main;
    }

    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        _target = currentHealth / maxHealth;
        _target2 = currentHealth / maxHealth;
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - _cam.transform.position);
        _healthbarSprite.fillAmount = Mathf.MoveTowards(_healthbarSprite.fillAmount, _target, _reduceSpeed * Time.deltaTime);
        _healthbarEffectSprite.fillAmount = Mathf.MoveTowards(_healthbarEffectSprite.fillAmount, _target2, _reduceSpeed * Time.deltaTime);

    }
}

