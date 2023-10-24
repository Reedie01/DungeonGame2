using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth;
    public float Health;
    public Image HealthBar;

    void Start()
    {
        maxHealth = Health;
    }

    void Update()
    {
        HealthBar.fillAmount = Mathf.Clamp(Health / maxHealth, 0, 100);
    }
}
