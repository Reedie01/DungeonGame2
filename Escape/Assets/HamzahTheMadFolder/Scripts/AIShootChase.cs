using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShootChase : MonoBehaviour
{
    public float speed;
    public float checkRadius;
    public float stoppingDistance;
    public float retreatDistance;

    public LayerMask whatIsPlayer;

    private float timeBtwShots;
    public float startTimeBtwShots;

    public bool shouldRotate;
    private bool isRunning;

    public GameObject projectile;
    public Transform player;
    private Vector2 movement;
    public Vector3 dir;

    Animator anim;

    public float onHitDelay = 5f;

    public bool hit = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        timeBtwShots = startTimeBtwShots;
        anim = GetComponent<Animator>();
    }

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


    void Update()
    {
        if (!hit)
        {
            anim.SetBool("isRunning", isRunning);

            if (Vector2.Distance(transform.position, player.position) > stoppingDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
                isRunning = true;
            }
            else if (Vector2.Distance(transform.position, player.position) < stoppingDistance /* && Vector2.Distance(transform.position, player.position) > stoppingDistance */)
            {
                transform.position = this.transform.position;
                isRunning = false;
            }
            else if (Vector2.Distance(transform.position, player.position) < retreatDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
                isRunning = true;
            }

            if (timeBtwShots <= 0)
            {
                GameObject lastShot = Instantiate(projectile, transform.position, Quaternion.identity);
                lastShot.layer = 8;
                lastShot.tag = "Projectile";
                timeBtwShots = startTimeBtwShots;

            }
            else
            {
                timeBtwShots -= Time.deltaTime;
            }

            dir = player.position - transform.position;
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
}
