using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
    public GameObject healthPowerupPrefab; // The health powerup prefab to spawn
    public GameObject speedPowerupPrefab;   // The speed powerup prefab to spawn

    public Terrain terrain;                  // Reference to the terrain
    public int maxHealthPowerups = 5;       // Maximum number of health powerups on the terrain
    public int maxSpeedPowerups = 3;        // Maximum number of speed powerups on the terrain
    public float spawnInterval = 10f;        // Time interval between spawns

    private List<GameObject> activeHealthPowerups = new List<GameObject>(); // List to track active health powerups
    private List<GameObject> activeSpeedPowerups = new List<GameObject>();   // List to track active speed powerups

    void Start()
    {
        // Spawn the initial set of powerups
        SpawnPowerups(maxHealthPowerups, healthPowerupPrefab);
        SpawnPowerups(maxSpeedPowerups, speedPowerupPrefab);

        // Start the coroutine to spawn more powerups over time
        StartCoroutine(SpawnPowerupsOverTime());
    }

    void SpawnPowerups(int count, GameObject powerupPrefab)
    {
        for (int i = 0; i < count; i++)
        {
            if (powerupPrefab == healthPowerupPrefab && activeHealthPowerups.Count < maxHealthPowerups)
            {
                Vector3 spawnPosition = GetRandomPositionOnTerrain();
                GameObject newPowerup = Instantiate(powerupPrefab, spawnPosition, Quaternion.identity);
                activeHealthPowerups.Add(newPowerup);
            }
            else if (powerupPrefab == speedPowerupPrefab && activeSpeedPowerups.Count < maxSpeedPowerups)
            {
                Vector3 spawnPosition = GetRandomPositionOnTerrain();
                GameObject newPowerup = Instantiate(powerupPrefab, spawnPosition, Quaternion.identity);
                activeSpeedPowerups.Add(newPowerup);
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

            // Remove any null powerups from the lists
            activeHealthPowerups.RemoveAll(powerup => powerup == null);
            activeSpeedPowerups.RemoveAll(powerup => powerup == null);

            // Spawn more powerups if the total is below the maximum for each type
            if (activeHealthPowerups.Count < maxHealthPowerups)
            {
                SpawnPowerups(1, healthPowerupPrefab); // Spawn one health powerup at a time
            }
            if (activeSpeedPowerups.Count < maxSpeedPowerups)
            {
                SpawnPowerups(1, speedPowerupPrefab); // Spawn one speed powerup at a time
            }
        }
    }
}
