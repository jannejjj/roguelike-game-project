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
    public float knockbackForce = 150f;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        rigidBody = GetComponent<Rigidbody2D>();
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
        PlayerController player = collision.collider.GetComponent<PlayerController>();

        if ((player = collision.collider.GetComponent<PlayerController>()) != null)
        {
            // Damage player
            player.Health -= Random.Range(0.1f, 0.4f);

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
