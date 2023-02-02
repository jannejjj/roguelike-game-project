using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    public UIEnemies uiEnemies;
    int numberOfEnemies;

    GameHandler gameHandler;

    public int NumberOfEnemies
    {
        get { return numberOfEnemies; }
        set
        {
            numberOfEnemies = value;
            uiEnemies.SetEnemies(numberOfEnemies.ToString());

            if (numberOfEnemies == 0)
            {
                gameHandler.NextRound();
            }
        }
    }

    private void Start()
    {
        gameHandler = GetComponentInParent<GameHandler>();
    }

}
