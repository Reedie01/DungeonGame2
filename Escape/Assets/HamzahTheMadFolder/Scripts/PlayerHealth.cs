using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth;
    public float Health;
    public Image HealthBar;
    

    void Start()
    {
        Health = maxHealth;
      //  HealthBar.fillAmount = 0.5f;
        Debug.Log("this works");
    }

    void Update()
    {
        if(Health <= 0)
        {
            SceneManager.LoadScene("HamzahDeathScene");
        }
    }

    public void TakeDamage(float amount)
    {
        Health = Health - amount;
        HealthBar.fillAmount = Health / maxHealth;
            //Mathf.Clamp(Health / maxHealth, 0, 100);
        Debug.Log("TakeDamage()");
    }
}
