using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
    public AudioClip pickupAudio;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player;

        if ((player = collision.collider.GetComponent<PlayerController>()) != null)
        {
            audioSource.PlayOneShot(pickupAudio, 0.6F);
        }
    }
}
