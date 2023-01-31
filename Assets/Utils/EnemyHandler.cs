using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    public UIEnemies uiEnemies;
    public int numberOfEnemies;

    public int NumberOfEnemies
    {
        get { return numberOfEnemies; }
        set
        {
            numberOfEnemies = value;
            uiEnemies.SetEnemies(numberOfEnemies.ToString());
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
