using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AIChase : MonoBehaviour
{
    public float speed;
    public float checkRadius;
    public float attackRadius;

    public bool shouldRotate;

    public LayerMask whatIsPlayer;

    private Transform target;
    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 movement;
    public Vector3 dir;

    private bool isInChaseRange;
    private bool isInAttackRange;

    private double distanceFromPlayer;
    private float playerX;
    private float playerY;
    private float xDifference;
    private float yDifference;

    public float onHitDelay = 5f;

    public bool hit = false;

    public PlayerHealth playerHealth;

    public void OnHit(float delay)
    {
        if (!hit)
        {
            StartCoroutine(Hit(delay));
        }

    }

    IEnumerator Hit(float delay)
    {
        hit = true;
        yield return new WaitForSeconds(delay);
        hit = false;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;
        playerHealth = target.GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        if (!hit)
        {
            playerX = target.transform.position.x;
            playerY = target.transform.position.y;
            xDifference = playerX - transform.position.x;
            yDifference = playerY - transform.position.y;
            distanceFromPlayer = Math.Sqrt(((yDifference) * (yDifference)) + ((xDifference) * (xDifference)));
            Vector3 objectPosition = target.transform.position;
            anim.SetBool("isRunning", isInChaseRange);

            isInChaseRange = Physics2D.OverlapCircle(transform.position, checkRadius, whatIsPlayer);
            isInChaseRange = Physics2D.OverlapCircle(transform.position, attackRadius, whatIsPlayer);

            dir = target.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            dir.Normalize();
            movement = dir;

            if (shouldRotate)
            {
                anim.SetFloat("X", dir.x);
                anim.SetFloat("Y", dir.y);
            }
        }
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, 0f);
        transform.position = newPosition;
    }

    private void FixedUpdate()
    {
        if(isInChaseRange && !isInAttackRange && !hit)
        {
            MoveCharacter(movement);
        }
        if(isInAttackRange && !hit)
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void MoveCharacter(Vector2 dir)
    {
        rb.MovePosition((Vector2)transform.position + (dir * speed * Time.deltaTime));
    }
}
