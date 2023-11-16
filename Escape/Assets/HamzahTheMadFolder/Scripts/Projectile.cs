using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;

    public float shotDuration = 5f;

    private Transform player;
    private Vector3 target;

    public string mageTag = "Mage";
    private int mageLayer;

    private void Awake()
    {
        StartCoroutine(Fired());
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            MoveTowardsTarget(player);
            Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, 0f);
            transform.position = newPosition;
        }
        
    }

    IEnumerator Fired()
    {
        yield return new WaitForSeconds(shotDuration);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Walls"))
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Mage"))
        {
            // Ignore collision with objects tagged as "Mage"
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider, true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Mage"))
        {
            // Resume collision with objects tagged as "Mage"
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider, false);
        }
    }
    private void MoveTowardsTarget(GameObject target)
    {
        Rigidbody2D rb2D = GetComponent<Rigidbody2D>();

        if (rb2D != null && target != null)
        {
            Vector2 direction = ((Vector2)target.transform.position - rb2D.position).normalized;
            rb2D.velocity = direction * speed;
        }
        else if (rb2D == null)
        {
            Debug.Log("missing rb2D");
        }
    }
}
