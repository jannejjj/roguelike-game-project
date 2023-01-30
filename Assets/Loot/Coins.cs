using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
    public AudioClip pickupAudio;
    public Sprite coinSmall;
    public Sprite coinMedium;
    public Sprite coinLarge;
    public Transform coinPopupPrefab;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player;

        // Check that the thing colliding with the coins is the player
        if ((player = collision.collider.GetComponent<PlayerController>()) != null)
        {
            player.audioSource.PlayOneShot(pickupAudio, 0.6F);

            int amount;
            // Add coins to player corresponding to the size of the coin stack
            if (spriteRenderer.sprite == coinSmall)
            {
                amount = Mathf.RoundToInt(Random.Range(5, 15));
            }
            else if (spriteRenderer.sprite == coinMedium)
            {
                amount = Mathf.RoundToInt(Random.Range(15, 30));
            }
            else
            {
                amount = Mathf.RoundToInt(Random.Range(30, 50));
            }

            player.Coins += amount;

            // Create coin popup
            Transform coinPopupTransform = Instantiate(coinPopupPrefab, Vector3.zero, Quaternion.identity);
            CoinPopup popup = coinPopupTransform.GetComponent<CoinPopup>();
            popup.Setup(amount, transform.localPosition);

            Destroy(gameObject);

        }
    }
}
