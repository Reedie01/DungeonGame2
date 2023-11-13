using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KnockbackFeedback : MonoBehaviour
{
    public float knockbackTime = 0.2f;
    public float hitDirectionForce = 10f;
    public float constForce = 5f;
    public float inputForce = 7.5f;

    private Rigidbody2D rb;

    private Coroutine knockbackCoroutine;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public bool IsBeingKnockback { get; private set; }

    public IEnumerator KnockbackAction(Vector2 hitDirection, Vector2 constantForceDirection, float inputDirection)
    {
        IsBeingKnockback = true;

        Vector2 _hitForce;
        Vector2 _constantForce;
        Vector2 _knockbackForce;
        Vector2 _combinedForce;

        _hitForce = hitDirection * hitDirectionForce;
        _constantForce = constantForceDirection * constForce;
        

        float _elapsedTime = 0f;
        while(_elapsedTime < knockbackTime)
        {
            _elapsedTime += Time.fixedDeltaTime;

            //combine _hitForce and _constantForce
            _knockbackForce = _hitForce + _constantForce;

            //combine knockBackForce with Input Force
            if (inputDirection != 0)
            {
                _combinedForce = _knockbackForce + new Vector2(inputDirection, 0f);
            }
            else
            {
                _combinedForce = _knockbackForce;
            }

            //apply knockback
            rb.velocity = _combinedForce;

            yield return new WaitForFixedUpdate();
        }

        IsBeingKnockback = false;
    }    

    public void CallKnockback(Vector2 hitDirection, Vector2 constantForceDirection, float inputDirection) 
    {
        knockbackCoroutine = StartCoroutine(KnockbackAction(hitDirection, constantForceDirection, inputDirection));
    }
}
