using UnityEngine;

public class TerrainObjectSpawner : MonoBehaviour
{
    public GameObject objectToSpawn; // Assign your prefab in the Inspector
    public int numberOfObjects = 50; // Number of objects to spawn
    public Terrain terrain; // Reference to the Terrain object
    public float maxSlope = 30f; // Maximum slope allowed for object spawning

    void Start()
    {
        SpawnObjects();
    }

    void SpawnObjects()
    {
        Vector3 terrainSize = terrain.terrainData.size;
        int spawnedObjects = 0;

        while (spawnedObjects < numberOfObjects)
        {
            // Generate random positions within the terrain bounds
            float randomX = Random.Range(0, terrainSize.x);
            float randomZ = Random.Range(0, terrainSize.z);

            // Get the height at the random point
            float y = terrain.SampleHeight(new Vector3(randomX, 0, randomZ)) + terrain.transform.position.y;

            // Calculate the slope (steepness) at the random point
            float slope = terrain.terrainData.GetSteepness(randomX / terrainSize.x, randomZ / terrainSize.z);

            // Check if the slope is within the acceptable range
            if (slope <= maxSlope)
            {
                // Set the spawn position with the correct terrain Y height
                Vector3 spawnPosition = new Vector3(randomX, y, randomZ);

                // Instantiate the object at the spawn position
                Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);

                // Increment the counter for spawned objects
                spawnedObjects++;
            }
        }
    }
}
