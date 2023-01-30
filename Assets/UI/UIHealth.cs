using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIHealth : MonoBehaviour
{
    TextMeshPro textMesh;

    // Start is called before the first frame update
    void Start()
    {
        textMesh = GetComponent<TextMeshPro>();
    }

    public void SetHealth(string newText)
    {
        if (newText.Contains("-"))
        {
            textMesh.text = "0";
        }
        else
        {
            textMesh.text = newText;
        }
    }

}
