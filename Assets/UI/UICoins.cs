using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UICoins : MonoBehaviour
{
    TextMeshPro textMesh;

    // Start is called before the first frame update
    void Start()
    {
        textMesh = GetComponent<TextMeshPro>();
    }

    public void SetCoins(string newText)
    {
        textMesh.text = newText;
    }

}
