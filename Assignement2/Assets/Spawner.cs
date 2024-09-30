using UnityEngine;

public class TerrainObjectSpawner : MonoBehaviour
{
    public GameObject objectToSpawn; // Assign your prefab in the Inspector
    public int numberOfObjects = 50; // Number of objects to spawn
    public Terrain terrain; // Reference to the Terrain object

    void Start()
    {
        SpawnObjects();
    }

    void SpawnObjects()
    {
        // Get the terrain size
        int terrainWidth = terrain.terrainData.heightmapResolution;
        int terrainHeight = terrain.terrainData.heightmapResolution;
        Vector3 terrainSize = terrain.terrainData.size;

        for (int i = 0; i < numberOfObjects; i++)
        {
            // Generate random positions within the terrain bounds
            float randomX = Random.Range(0, terrainWidth);
            float randomZ = Random.Range(0, terrainHeight);

            // Get the height at the random point
            float y = terrain.SampleHeight(new Vector3(randomX, 0, randomZ));

            // Convert to world position
            Vector3 spawnPosition = new Vector3(
                randomX / terrainWidth * terrainSize.x,
                y,
                randomZ / terrainHeight * terrainSize.z
            );

            // Instantiate the object at the spawn position
            Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
        }
    }
}
