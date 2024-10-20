using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
    public GameObject powerupPrefab; // The powerup prefab to spawn
    public Terrain terrain;          // Reference to the terrain
    public int maxPowerups = 5;      // Maximum number of powerups on the terrain
    public float spawnInterval = 10f; // Time interval between spawns

    private List<GameObject> activePowerups = new List<GameObject>(); // List to track active powerups

    void Start()
    {
        // Spawn the initial set of powerups
        SpawnPowerups(maxPowerups);

        // Start the coroutine to spawn more powerups over time
        StartCoroutine(SpawnPowerupsOverTime());
    }

    void SpawnPowerups(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (activePowerups.Count < maxPowerups)
            {
                Vector3 spawnPosition = GetRandomPositionOnTerrain();
                GameObject newPowerup = Instantiate(powerupPrefab, spawnPosition, Quaternion.identity);
                activePowerups.Add(newPowerup);
            }
        }
    }

    // Get a random position on the terrain
    Vector3 GetRandomPositionOnTerrain()
    {
        float terrainWidth = terrain.terrainData.size.x;
        float terrainLength = terrain.terrainData.size.z;

        // Get random x, z positions
        float randomX = Random.Range(0, terrainWidth);
        float randomZ = Random.Range(0, terrainLength);

        // Get the terrain height at the random position (y-axis)
        float yPos = terrain.SampleHeight(new Vector3(randomX, 0, randomZ));

        // Add terrain's actual position.y to adjust the height correctly relative to the world position
        yPos += terrain.transform.position.y;

        return new Vector3(randomX, yPos + 1, randomZ);
    }


    // Coroutine to spawn more powerups over time
    IEnumerator SpawnPowerupsOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // Remove any null powerups from the list (e.g., if a powerup was picked up or destroyed)
            activePowerups.RemoveAll(powerup => powerup == null);

            // Spawn more powerups if the total is below the maximum
            if (activePowerups.Count < maxPowerups)
            {
                SpawnPowerups(1); // Spawn one powerup at a time
            }
        }
    }
}
