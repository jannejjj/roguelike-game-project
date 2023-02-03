using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIRound : MonoBehaviour
{
    TextMeshPro textMesh;

    // Start is called before the first frame update
    void Start()
    {
        textMesh = GetComponent<TextMeshPro>();
    }

    public void SetRound(int roundNumber)
    {
        textMesh.text = roundNumber.ToString();
    }


}
