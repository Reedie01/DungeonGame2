using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public PlayerHealth pHealth;
    private KnockbackFeedback knockback;
    public float damage = 2;
    public GameObject player;

    void Start()
    {
        string tag = gameObject.tag;
        player = GameObject.FindGameObjectWithTag("Player");
        knockback = GetComponent<KnockbackFeedback>();
        pHealth = player.GetComponent<PlayerHealth>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            pHealth.TakeDamage(damage);
            if (tag == "Projectile")
            {
                Destroy(gameObject);
            }
        }
    }
}
