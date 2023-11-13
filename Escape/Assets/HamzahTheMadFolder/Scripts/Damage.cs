using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public PlayerHealth pHealth;
    private KnockbackFeedback knockback;
    public float damage = 2;

    void Start()
    {
        knockback = GetComponent<KnockbackFeedback>();
        //pHealth = null;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            pHealth.TakeDamage(damage);
        }
    }
}
