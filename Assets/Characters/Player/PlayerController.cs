using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public AudioClip attackAudio;
    public AudioClip deathAudio;
    public AudioClip ouchAudio;
    public AudioSource audioSource;
    public float collisionOffset = 0.05f;
    public float movementSpeed = 1f;
    public ContactFilter2D movementFilter;
    public SwordControl swordControl;
    Vector2 moveInput;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigidBody;
    public Transform damagePopupPrefab;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    Animator animator;

    bool moveLocked;

    public float Health
    {
        set
        {
            // Calculate damage to be used in popup
            float damage = health - value;

            // Set new health
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

    public float health = 2;


    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        // If move input != 0, move
        if (moveInput != Vector2.zero && !moveLocked)
        {
            bool success = TryToMove(moveInput);

            if (!success)
            {
                // If moving fails, try x and y directions separately. This allows moving along collision objects
                if (success = TryToMove(new Vector2(moveInput.x, 0)) == false)
                {
                    success = TryToMove(new Vector2(0, moveInput.y));
                }
            }
            animator.SetBool("isMoving", success);
        }
        else
        {
            animator.SetBool("isMoving", false);
            animator.SetInteger("moveDirection", 0);
        }

        // Handle directions
        if (moveInput.x < 0)
        {
            animator.SetInteger("moveDirection", 3);
        }
        else if (moveInput.x > 0)
        {
            animator.SetInteger("moveDirection", 1);
        }
        else if (moveInput.y < 0)
        {
            animator.SetInteger("moveDirection", 2);
        }
        else if (moveInput.y > 0)
        {
            animator.SetInteger("moveDirection", 4);
        }

    }

    private bool TryToMove(Vector2 direction)
    {
        if (direction == Vector2.zero)
        {
            return false;
        }

        int collisionCount = rigidBody.Cast(
                        direction,
                        movementFilter,
                        castCollisions,
                        movementSpeed * Time.fixedDeltaTime + collisionOffset);

        if (collisionCount == 0)
        {
            rigidBody.MovePosition(rigidBody.position + direction * movementSpeed * Time.fixedDeltaTime);
            return true;
        }
        return false;
    }

    public void Attack()
    {
        LockMovement();
        audioSource.PlayOneShot(attackAudio, 0.7F);

        switch (animator.GetInteger("moveDirection"))
        {
            case 1:
                swordControl.AttackRight();
                break;

            case 2:
                swordControl.AttackDown();
                break;

            case 3:
                swordControl.AttackLeft();
                break;

            case 4:
                swordControl.AttackUp();
                break;

            default:
                // If the player is not moving, attack right
                swordControl.AttackRight();
                break;
        }
    }

    public void EndAttack()
    {
        UnlockMovement();
        swordControl.StopAttack();
    }

    void OnMove(InputValue inputValue)
    {
        moveInput = inputValue.Get<Vector2>();
    }

    void OnFire()
    {
        animator.SetTrigger("swordAttack");
    }

    public void LockMovement()
    {
        moveLocked = true;
    }

    public void UnlockMovement()
    {
        moveLocked = false;
    }

    public void HandleKnockback(Vector2 knockbackForce)
    {
        rigidBody.AddForce(knockbackForce);
        print("knockbackForce (on player): " + knockbackForce);
    }

    public void Die()
    {
        // Death animation & sound
        audioSource.PlayOneShot(deathAudio, 0.7F);
        animator.SetTrigger("Dead");
        rigidBody.simulated = false;
    }

}
