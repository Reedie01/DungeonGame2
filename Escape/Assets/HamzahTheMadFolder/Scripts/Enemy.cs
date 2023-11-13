using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    PlayerHealth player;
    public float knockbackPower = 100;
    public float knockbackDuration = 1;

    Rigidbody2D rb;
    Animator anim;

    public int maxHealth = 100;
    int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<PlayerHealth>();
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    { 
        currentHealth -= damage;
        Debug.Log(currentHealth);


        if(currentHealth == 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy dead");
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            StartCoroutine(PlayerMovement.instance.Knockback(knockbackDuration, knockbackPower, this.transform));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Weapon")
        {
            Vector2 difference = (transform.position - other.transform.position) * 2;
            transform.position = new Vector2(transform.position.x + difference.x, transform.position.y + difference.y);
        }
    }
}

