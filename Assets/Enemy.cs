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

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
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

    public void Die()
    {
        // Death animation & sound
        audioSource.PlayOneShot(deathAudio, 0.7F);
        animator.SetTrigger("Dead");
    }

    public void DespawnEnemy()
    {
        Destroy(gameObject);
    }
}
