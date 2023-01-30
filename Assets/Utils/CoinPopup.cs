using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinPopup : MonoBehaviour
{
    private TextMeshPro textMesh;
    private float vanishTimer;
    private Color color;

    // Start is called before the first frame update
    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
    }


    public void Setup(int amount, Vector3 coinPosition)
    {
        // Get dmg float, round to int and set as the text
        textMesh.SetText(amount.ToString());

        // initial color
        color = textMesh.color;

        vanishTimer = 0.6f;

        transform.localPosition = coinPosition;
    }

    private void FixedUpdate()
    {
        float moveYspeed = 0.7f;
        transform.position += new Vector3(0, moveYspeed) * Time.fixedDeltaTime;

        if (vanishTimer > (0.6f / 2))
        {
            // First half of popup lifespan
            float scaleUpAmount = 1f;
            transform.localScale += Vector3.one * scaleUpAmount * Time.fixedDeltaTime;
        }
        else
        {
            // Second half of popup lifespan
            float scaleDownAmount = 1.5f;
            transform.localScale -= Vector3.one * scaleDownAmount * Time.fixedDeltaTime;
        }

        vanishTimer -= Time.fixedDeltaTime;
        if (vanishTimer < 0)
        {
            // Decrease the alpha value of the popup color until it's vanished
            float vanishSpeed = 2f;
            color.a -= vanishSpeed * Time.fixedDeltaTime;
            textMesh.color = color;

            if (color.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }

}
