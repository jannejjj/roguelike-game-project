using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PositiveModifierPopup : MonoBehaviour
{
    private TextMeshPro textMesh;
    private float vanishTimer;
    private Color initialColor;

    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
    }


    public void Setup(string text, Vector3 position)
    {
        textMesh.SetText(text);

        initialColor = textMesh.color;

        vanishTimer = 0.8f;

        transform.localPosition = position;
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
            initialColor.a -= vanishSpeed * Time.fixedDeltaTime;
            textMesh.color = initialColor;

            if (initialColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
