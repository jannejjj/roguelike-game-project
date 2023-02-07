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
    public UIRound uiRound;
    TilemapCollider2D terrainCollider;
    Tilemap ground;
    Vector3 groundMinCoords;
    Vector3 groundMaxCoords;
    int enemiesToSpawn = 10;
    int coinsToSpawn = 15;
    int roundCounter = 1;
    PlayerController player;
    SwordControl sword;

    public enum ModifierType
    {
        Speedy,     // More move speed
        Slow,       // Less move speed
        Frail,      // Less HP
        Tanky,      // More HP
        Strong,     // Higher damage, more knockback
        Weak,       // Less damage, less knockbak
        Corrupted,  // More damage, but lose 1HP per second
        Vampiric,   // Heal on damaging enemies

    }

    public void NextRound()
    {
        // Spawn 1.25 - 1.5 times more enemies than the previous round, reset player position, health and coins. Increase round number by 1.
        enemiesToSpawn = Mathf.RoundToInt(enemiesToSpawn * Random.Range(1.25f, 1.5f));
        SpawnEnemies(enemiesToSpawn);
        SpawnCoins(coinsToSpawn);
        player.Coins = 0;
        player.Health = player.maxHealth;
        player.transform.position = Vector2.zero;
        roundCounter++;
        uiRound.SetRound(roundCounter);
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInChildren<PlayerController>();
        sword = player.GetComponentInChildren<SwordControl>();
        terrainCollider = collisionLayer.GetComponent<TilemapCollider2D>();
        ground = groundLayer.GetComponent<Tilemap>();

        enemyHandler = enemyHandlerTransform.GetComponent<EnemyHandler>();

        groundMinCoords = ground.localBounds.min;
        groundMaxCoords = ground.localBounds.max;

        SpawnEnemies(enemiesToSpawn);
        SpawnCoins(coinsToSpawn);
    }
    private void Update()
    {
        // Healing
        if (Input.GetKeyDown(KeyCode.H) && (player.Coins >= 50))
        {
            if (player.Health <= 0.75f)
            {
                player.Health += 0.25f;
                player.Coins -= 50;
            }
            else if (player.Health < player.maxHealth)
            {
                player.Health = 1f;
                player.Coins -= 50;
            }
            else
            {
                Debug.Log("HP Full!");
            }
        }
        else if (Input.GetKeyDown(KeyCode.X) && (player.Coins >= 50))
        {
            player.Coins -= 50;

            // Pick a random modifier from the enum
            System.Array modifiers = ModifierType.GetValues(typeof(ModifierType));
            int randIndex = Random.Range(0, modifiers.Length);

            switch (modifiers.GetValue(randIndex))
            {
                case ModifierType.Speedy:
                    SetSpeedyModifier();
                    break;

                case ModifierType.Slow:
                    SetSlowModifier();
                    break;

                case ModifierType.Frail:
                    SetFrailModifier();
                    break;

                case ModifierType.Tanky:
                    SetTankyModifier();
                    break;

                case ModifierType.Strong:
                    SetStrongModifier();
                    break;

                case ModifierType.Weak:
                    SetWeakModifier();
                    break;

                case ModifierType.Corrupted:
                    SetCorruptedModifier();
                    break;

                case ModifierType.Vampiric:
                    SetVampiricModifier();
                    break;

            }
        }

    }

    void SetSpeedyModifier()
    {
        player.movementSpeed += Random.Range(0.15f, 0.3f);
        player.modifiers["Speedy"] += 1;
        Debug.Log("Speedy");
    }

    void SetSlowModifier()
    {
        player.movementSpeed -= Random.Range(0.15f, 0.3f);
        player.modifiers["Slow"] += 1;
        Debug.Log("Slow");
    }

    void SetFrailModifier()
    {
        player.maxHealth -= .25f;
        if (player.maxHealth < player.Health)
        {
            player.Health = player.maxHealth;
        }
        player.modifiers["Frail"] += 1;
        Debug.Log("Frail");
    }

    void SetTankyModifier()
    {
        player.maxHealth += .5f;
        player.Health = player.maxHealth;
        player.modifiers["Tanky"] += 1;
        Debug.Log("Tanky");
    }

    void SetStrongModifier()
    {
        sword.knockbackForce += Random.Range(100f, 300f);
        sword.minDamage += Random.Range(0.05f, 0.1f);
        sword.maxDamage += Random.Range(0.05f, 0.1f);
        player.modifiers["Strong"] += 1;
        Debug.Log("Strong");
    }

    void SetWeakModifier()
    {
        sword.knockbackForce -= Random.Range(50f, 100f);
        // Prevent negative values
        if (sword.knockbackForce < 0f)
        {
            sword.knockbackForce = 0f;
        }
        player.modifiers["Weak"] += 1;
        Debug.Log("Weak");
    }

    void SetCorruptedModifier()
    {
        InvokeRepeating("damageOverTime", 0f, 1f);
        sword.minDamage += Random.Range(0.05f, 0.1f);
        sword.maxDamage += Random.Range(0.05f, 0.1f);
        player.modifiers["Corrupted"] += 1;
        Debug.Log("Corrupted");
    }

    void SetVampiricModifier()
    {
        player.modifiers["Vampiric"] += 1;
        Debug.Log("Vampiric");
    }

    void damageOverTime() // To be used with InvokeRepeating()
    {
        player.Health -= 0.01f;
    }

    void SpawnEnemies(int amount)
    {
        for (int i = 0; i < enemiesToSpawn
        ; i++)
        {
            // Instatiate enemy at random x,y coordinate within the map and place it as a child of the Enemies gameObject

            Vector2 randPosition;
            while (BadPosition(randPosition = GetRandomPosition()))
            {
                randPosition = GetRandomPosition();
            }

            Transform enemyTransform = Instantiate(enemyPrefab, randPosition, Quaternion.identity);
            enemyTransform.parent = enemyHandlerTransform;
        }
        enemyHandler.NumberOfEnemies = enemiesToSpawn;
    }

    void SpawnCoins(int amount)
    {
        // Reduce the amount by the number of coins already in the level (not picked up during previous round)
        amount = (amount - lootHandlerTransform.childCount);
        for (int i = 0; i < amount; i++)
        {
            // Pick one of the coin prefabs at random and instantiate at a random x,y coordinate within the map and place it as a child of the Loot gameObject

            Vector2 randPosition;
            while (BadPosition(randPosition = GetRandomPosition()))
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

    bool BadPosition(Vector2 point)
    {
        // Check that the spawn point isn't too close to the player (no closer than 1f);

        // Additionally, if the closest point on the terrain collider == the analyzed point, the analyzed point is inside the terrain. (margin of 0.15f from the edges of the collider).

        Vector2 closestPoint = terrainCollider.ClosestPoint(point);
        if (Vector2.Distance(player.transform.position, point) < 2f)
        {
            return true;
        }
        else if (Vector2.Distance(closestPoint, point) < 0.15f)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
}
