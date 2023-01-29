using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordControl : MonoBehaviour
{
    BoxCollider2D attackCollider;
    public float knockbackForce = 150f;
    public float minDamage = 0.1f;
    public float maxDamage = 0.4f;
    private void Start()
    {
        attackCollider = GetComponent<BoxCollider2D>();
    }

    public void AttackRight()
    {
        attackCollider.enabled = true;
        attackCollider.offset = new Vector2((float)0.17, (float)-0.1);

    }

    public void AttackLeft()
    {
        attackCollider.enabled = true;
        attackCollider.offset = new Vector2((float)-0.17, (float)-0.1);
    }

    public void AttackDown()
    {
        attackCollider.enabled = true;
        attackCollider.offset = new Vector2(0, (float)-0.2);

    }


    public void AttackUp()
    {
        attackCollider.enabled = true;
        attackCollider.offset = new Vector2(0, (float)0.1);
    }

    public void StopAttack()
    {
        attackCollider.enabled = false;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Enemy enemy;
            if ((enemy = other.GetComponent<Enemy>()) != null)
            {
                enemy.Health -= Random.Range(minDamage, maxDamage);
            }

            Vector3 playerPosition = gameObject.GetComponentInParent<Transform>().position;

            // Direction between player and enemy
            Vector2 direction = (Vector2)(other.gameObject.transform.position - playerPosition).normalized;

            // Knockback vector
            Vector2 knockback = direction * knockbackForce;

            enemy.HandleKnockback(knockback);
        }
    }
}
