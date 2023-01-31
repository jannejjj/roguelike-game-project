using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public Transform enemyPrefab;
    public Transform lootHandlerTransform;
    public Transform enemyHandlerTransform;
    EnemyHandler enemyHandler;
    public Transform[] coinPrefabs;
    public Transform groundLayer;
    public Transform collisionLayer;
    TilemapCollider2D terrainCollider;
    Tilemap ground;
    Vector3 groundMinCoords;
    Vector3 groundMaxCoords;
    int enemiesToSpawn = 10;
    int coinsToSpawn = 15;
    PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInChildren<PlayerController>();
        terrainCollider = collisionLayer.GetComponent<TilemapCollider2D>();
        ground = groundLayer.GetComponent<Tilemap>();

        enemyHandler = enemyHandlerTransform.GetComponent<EnemyHandler>();

        groundMinCoords = ground.localBounds.min;
        groundMaxCoords = ground.localBounds.max;

        spawnEnemies(enemiesToSpawn);
        SpawnCoins(coinsToSpawn);
    }

    void IncreasePlayerSpeed(float amount)
    {
        player.movementSpeed += amount;
    }


    void spawnEnemies(int amount)
    {
        for (int i = 0; i < enemiesToSpawn
        ; i++)
        {
            // Instatiate enemy at random x,y coordinate within the map and place it as a child of the Enemies gameObject

            Vector2 randPosition;
            while (IsInsideTerrain(randPosition = GetRandomPosition()))
            {
                randPosition = GetRandomPosition();
            }

            Transform enemyTransform = Instantiate(enemyPrefab, randPosition, Quaternion.identity);
            enemyTransform.parent = enemyHandlerTransform;
            enemyHandler.NumberOfEnemies = enemiesToSpawn;
        }
    }

    void SpawnCoins(int amount)
    {
        for (int i = 0; i < coinsToSpawn; i++)
        {

            // Pick one of the coin prefabs at random and instantiate at a random x,y coordinate within the map and place it as a child of the Loot gameObject

            Vector2 randPosition;
            while (IsInsideTerrain(randPosition = GetRandomPosition()))
            {
                randPosition = GetRandomPosition();
            }

            Transform coinTransform = Instantiate(coinPrefabs[Mathf.RoundToInt(Random.Range(1, 3))], randPosition, Quaternion.identity);
            coinTransform.parent = lootHandlerTransform;
        }
    }

    Vector2 GetRandomPosition()
    {
        // Get a random position within the level (0.1f margins from edges)

        return new Vector2(Random.Range(groundMinCoords.x + 0.1f, groundMaxCoords.x - 0.1f), Random.Range(groundMinCoords.y + 0.1f, groundMaxCoords.y - 0.1f));
    }

    bool IsInsideTerrain(Vector2 point)
    {
        // If closest point on the terrain collider == point, the analyzed point is inside terrain. (margin of 0.05 from edges of the collider)

        Vector2 closestPoint = terrainCollider.ClosestPoint(point);
        return (Vector2.Distance(closestPoint, point) < 0.05f);
    }
}
