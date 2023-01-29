using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Animator animator;
    public AudioClip attackAudio;
    public AudioClip deathAudio;
    public AudioClip ouchAudio;
    public AudioSource audioSource;
    public Transform damagePopupPrefab;
    Rigidbody2D rigidBody;
    public float knockbackForce = 500f;
    public float movementSpeed = 300f;
    public float minDamage = 0.1f;
    public float maxDamage = 0.25f;
    public EnemyAggro aggroArea;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (aggroArea.detectedList.Count > 0)
        {
            PlayerController player = aggroArea.detectedList[0].GetComponent<PlayerController>();

            Vector2 directionToPlayer = (player.transform.position - gameObject.transform.position).normalized;

            // Player has been detected, move towards it
            animator.SetBool("isMoving", true);
            rigidBody.AddForce(directionToPlayer * movementSpeed * Time.fixedDeltaTime);

            // Face the player
            if (directionToPlayer.x > 0)
            {
                animator.SetInteger("MoveDirection", 1);
            }
            else if (directionToPlayer.x < 0)
            {
                animator.SetInteger("MoveDirection", 2);
            }
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        print("RB velocity:" + rigidBody.velocity);

        if (rigidBody.velocity != Vector2.zero)
        {
            if (rigidBody.velocity.x > 0)
            {
            }
            else if (rigidBody.velocity.x < 0)
            {
            }
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    public float Health
    {
        set
        {
            // Calculate damage to be used in popup
            float damage = health - value;

            // Set health
            health = value;

            // Damage popup
            Transform damagePopupTransform = Instantiate(damagePopupPrefab, Vector3.zero, Quaternion.identity);
            DamagePopup popup = damagePopupTransform.GetComponent<DamagePopup>();
            popup.Setup(damage, transform.localPosition);

            // Sound
            audioSource.PlayOneShot(ouchAudio, 0.7F);

            // Check death
            if (health <= 0)
            {
                Die();
            }
        }
        get
        {
            return health;
        }
    }

    public float health = 1;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player;

        if ((player = collision.collider.GetComponent<PlayerController>()) != null)
        {
            // Damage player
            player.Health -= Random.Range(minDamage, maxDamage);

            // Get current position of enemy
            Vector3 enemyPosition = gameObject.GetComponentInParent<Transform>().position;

            // Direction between player and enemy
            Vector2 direction = (Vector2)(player.gameObject.transform.position - enemyPosition).normalized;

            // Knockback vector
            Vector2 knockback = direction * knockbackForce;

            player.HandleKnockback(knockback);
        }
    }

    public void HandleKnockback(Vector2 knockbackForce)
    {
        rigidBody.AddForce(knockbackForce);
        print("knockbackForce: " + knockbackForce);
        StartCoroutine(ReduceMovementSpeed(275f, 0.5f));
    }

    IEnumerator ReduceMovementSpeed(float amount, float seconds)
    {
        movementSpeed -= amount;
        yield return new WaitForSeconds(seconds);
        movementSpeed += amount;
    }

    public void Die()
    {
        // Death animation & sound
        audioSource.PlayOneShot(deathAudio, 0.7F);
        animator.SetTrigger("Dead");
        rigidBody.simulated = false;
    }

    public void DespawnEnemy()
    {
        Destroy(gameObject);
    }
}
