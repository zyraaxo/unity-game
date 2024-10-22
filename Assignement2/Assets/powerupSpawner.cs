using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
    public GameObject healthPowerupPrefab;
    public GameObject speedPowerupPrefab;
    public Terrain terrain;
    public int maxHealthPowerups = 5;
    public int maxSpeedPowerups = 3;
    public float spawnInterval = 10f;
    private List<GameObject> activeHealthPowerups = new List<GameObject>();
    private List<GameObject> activeSpeedPowerups = new List<GameObject>();

    void Start()
    {
        // Spawn the initial set of powerups
        SpawnPowerups(maxHealthPowerups, healthPowerupPrefab);
        SpawnPowerups(maxSpeedPowerups, speedPowerupPrefab);

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

    Vector3 GetRandomPositionOnTerrain()
    {
        float terrainWidth = terrain.terrainData.size.x;
        float terrainLength = terrain.terrainData.size.z;

        // Get random x, z positions
        float randomX = Random.Range(0, terrainWidth);
        float randomZ = Random.Range(0, terrainLength);

        // Get the terrain height at the random position (y-axis)
        float yPos = terrain.SampleHeight(new Vector3(randomX, 0, randomZ));

        yPos += terrain.transform.position.y;

        return new Vector3(randomX, yPos + 1, randomZ);
    }

    // Coroutine to spawn more powerups over time
    IEnumerator SpawnPowerupsOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            activeHealthPowerups.RemoveAll(powerup => powerup == null);
            activeSpeedPowerups.RemoveAll(powerup => powerup == null);

            if (activeHealthPowerups.Count < maxHealthPowerups)
            {
                SpawnPowerups(1, healthPowerupPrefab);
            }
            if (activeSpeedPowerups.Count < maxSpeedPowerups)
            {
                SpawnPowerups(1, speedPowerupPrefab); // Spawn one speed powerup at a time
            }
        }
    }
}
