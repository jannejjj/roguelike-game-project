using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIEnemies : MonoBehaviour
{
    TextMeshPro textMesh;

    // Start is called before the first frame update
    void Start()
    {
        textMesh = GetComponent<TextMeshPro>();
    }

    public void SetEnemies(string newText)
    {
        {
            textMesh.text = newText;
        }
    }
}
