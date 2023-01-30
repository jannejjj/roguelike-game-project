using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public Transform enemyPrefab;
    public Transform lootParent;
    public Transform enemyParent;
    public Transform[] coinPrefabs;
    int enemiesToSpawn = 5;
    int coinsToSpawn = 15;
    PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInChildren<PlayerController>();
        spawnEnemies(enemiesToSpawn);
        SpawnCoins(coinsToSpawn);
    }

    void IncreasePlayerSpeed(float amount)
    {
        player.movementSpeed += amount;
    }


    void spawnEnemies(int amount)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            // Instatiate enemy at random x,y coordinate
            Transform enemyTransform = Instantiate(enemyPrefab, new Vector3(Random.Range(-3f, 2.5f), Random.Range(-1.5f, 2f), 0f), Quaternion.identity);
            enemyTransform.parent = enemyParent;
        }
    }

    void SpawnCoins(int amount)
    {
        for (int i = 0; i < coinsToSpawn; i++)
        {
            // Pick one of the coin prefabs at random and instantiate at a random x,y coordinate
            Transform coinTransform = Instantiate(coinPrefabs[Mathf.RoundToInt(Random.Range(1, 3))], new Vector3(Random.Range(-3f, 2.5f), Random.Range(-1.5f, 2f), 0f), Quaternion.identity);
            coinTransform.parent = lootParent;
        }
    }
}
