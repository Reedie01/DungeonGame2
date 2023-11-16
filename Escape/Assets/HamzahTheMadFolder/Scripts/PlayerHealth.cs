using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth;
    public float health;

    public Slider healthBar;

    public Image fillImage;

    public UIManager uIManager;

    void Start()
    {
        health = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.minValue = 0f;
        healthBar.value = health;

        UpdateSliderColor();
        healthBar.onValueChanged.AddListener(delegate { UpdateSliderColor(); });
    }

    private void UpdateSliderColor()
    {
        float normalizedValue = Mathf.InverseLerp(healthBar.minValue, healthBar.maxValue, healthBar.value);
        Color lerpedColor = Color.Lerp(Color.red, Color.green, normalizedValue);
        fillImage.color = lerpedColor;
    }

    void Update()
    {
        if(health <= 0)
        {
            uIManager.GameOver();
        }
        healthBar.value = health;
    }

    public void Heal()
    {
        health = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        healthBar.value = health;
        //Debug.Log(amount);
    }
}