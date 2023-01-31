using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public Transform enemyPrefab;
    public Transform lootParent;
    public Transform enemyParent;
    public Transform[] coinPrefabs;
    public Transform groundLayer;
    public Transform collisionLayer;
    TilemapCollider2D terrainCollider;
    Tilemap ground;
    Vector3 groundMinCoords;
    Vector3 groundMaxCoords;
    int enemiesToSpawn = 5;
    int coinsToSpawn = 15;
    PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInChildren<PlayerController>();
        terrainCollider = collisionLayer.GetComponent<TilemapCollider2D>();
        ground = groundLayer.GetComponent<Tilemap>();

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
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            // Instatiate enemy at random x,y coordinate within the map and place it as a child of the Enemies gameObject

            Vector2 randPosition;
            while (IsInsideTerrain(randPosition = GetRandomPosition()))
            {
                randPosition = GetRandomPosition();
            }

            Transform enemyTransform = Instantiate(enemyPrefab, randPosition, Quaternion.identity);
            enemyTransform.parent = enemyParent;
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
            coinTransform.parent = lootParent;
        }
    }

    Vector2 GetRandomPosition()
    {
        return new Vector2(Random.Range(groundMinCoords.x, groundMaxCoords.x), Random.Range(groundMinCoords.y, groundMaxCoords.y));
    }

    bool IsInsideTerrain(Vector2 point)
    {
        // If closest point on the terrain collider == point, the analyzed point is inside terrain
        return terrainCollider.ClosestPoint(point) == point;
    }
}
