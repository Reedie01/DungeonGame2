using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Enemy : MonoBehaviour
{
    private PlayerHealth player;
    public float knockbackPower = 100;
    public float knockbackDuration = 1;
    public float forceAmount = 10f;

    private Rigidbody2D rb2D;
    private Animator anim;

    public GameObject currentPlayer;

    public int maxHealth = 100;
    public int currentHealth;

    private Vector2 playerPosition;

    public bool hit = false;

    private Collider2D playerCollider;

    public AIChase aIChase;
    public AIShootChase mageAIChase;

    private bool justEnded = false;

    public int lives = 5;

    public LayerMask floorLayerMask;

    public float checkRadius = 0.2f;

    public TilemapCollider2D floor;

    void Awake()
    {
        gameObject.layer = 7;
    }

    void Start()
    {
        GameObject[] floors = GameObject.FindGameObjectsWithTag("Floor");
        foreach (GameObject floor in floors)
        {
            TilemapCollider2D tilemapCollider = floor.GetComponent<TilemapCollider2D>();
            if (tilemapCollider != null)
            {
                floorLayerMask |= 1 << floor.layer;
            }
        }
        aIChase = GetComponent<AIChase>();
        mageAIChase = GetComponent<AIShootChase>();
        currentHealth = maxHealth;
        rb2D = GetComponent<Rigidbody2D>();
        player = GetComponent<PlayerHealth>();
        anim = GetComponent<Animator>();
        currentPlayer = GameObject.FindGameObjectWithTag("Player");
    }

    public void TakeDamage(int damage)
    { 
        playerPosition = new Vector2(currentPlayer.transform.position.x, currentPlayer.transform.position.y);
        currentHealth -= damage;
        TakeKnockback(playerPosition);
        //Debug.Log(currentHealth);


        if(currentHealth == 0)
        {
            Die();
        }
    }

    private void TakeKnockback(Vector2 targetPosition)
    {
        rb2D = GetComponent<Rigidbody2D>();
        /*Vector2 direction = (Vector2)transform.position - targetPosition;
        direction.Normalize();
        rb.AddForce(direction * forceAmount, ForceMode2D.Impulse);
        Debug.Log("Applied Force: " + direction * forceAmount);*/
        if (targetPosition != null)
        {
            //StartCoroutine(DisablePlayerCollisionTemporarily());
            Vector2 targetPosition2D = new Vector2(targetPosition.x, targetPosition.y);
            Vector2 enemyPostition = new Vector2(this.transform.position.x, this.transform.position.y);
            Vector2 direction = (targetPosition2D - enemyPostition.normalized);
            rb2D.AddForce(-direction * forceAmount * (1 / 1000), ForceMode2D.Impulse);
            Debug.Log($"Forece applied to enemy is eqal to {-direction * forceAmount}");
        }
        else if (targetPosition == null)
        {
            Debug.Log("Target is null");
        }

    }

    IEnumerator Hit()
    {
        hit = true;
        if(aIChase != null)
        {
            aIChase.OnHit(knockbackDuration);
        }
        else if(mageAIChase != null)
        {
            mageAIChase.OnHit(knockbackDuration);
        }
        
        yield return new WaitForSeconds(knockbackDuration);
        hit = false;
    }

    void FixedUpdate()
    {
        if (lives == 0)
        {
            Destroy(gameObject);
        }
        Vector2 location = new Vector2(transform.position.x, transform.position.y);
        Vector2 targetPosition = new Vector2(currentPlayer.transform.position.x, currentPlayer.transform.position.y);
        if (targetPosition != null && hit)
        {
            justEnded = false;
            Vector2 direction = (targetPosition - location).normalized;
            rb2D.AddForce(-direction * forceAmount, ForceMode2D.Impulse);
        }
        if(!hit && !justEnded)
        {
            rb2D.velocity = Vector2.zero;
            rb2D.angularVelocity = 0f;
            justEnded = true;
        }
        Collider2D collider = Physics2D.OverlapCircle(transform.position, 1f, floorLayerMask);

        if (collider == null)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator DisablePlayerCollisionTemporarily()
    {
        playerCollider = currentPlayer.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), playerCollider, true);
        //Debug.Log("Phase activated");
        yield return new WaitForSeconds(0.5f);

        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), playerCollider, false);
        //Debug.Log("Phase deactivated");
    }

    void Die()
    {
        Debug.Log("Enemy dead");
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        /*Vector2 location = new Vector2(transform.position.x, transform.position.y);
        if(other.gameObject.tag == "Player")
        {
            PlayerMovement.instance.TakeKnockback(location, 10f);
        }*/
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        playerPosition = new Vector2(currentPlayer.transform.position.x, currentPlayer.transform.position.y);
        if (other.tag == "Weapon")
        {
            StartCoroutine(Hit());
            lives -= 1;
        }
    }
}

