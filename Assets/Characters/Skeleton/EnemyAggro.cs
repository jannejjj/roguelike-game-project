using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggro : MonoBehaviour
{
    Collider2D col;
    public List<Collider2D> detectedList = new();

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            detectedList.Add(other);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        detectedList.Remove(other);
    }
}
