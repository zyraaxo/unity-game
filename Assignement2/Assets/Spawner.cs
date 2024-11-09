using UnityEngine;

public class TerrainObjectSpawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    public int numberOfObjects = 50;
    public Terrain terrain;
    public float maxSlope = 30f;

    void Start()
    {
        SpawnObjects();
    }

    public void SpawnObjects()
    {
        Vector3 terrainSize = terrain.terrainData.size;
        int spawnedObjects = 0;

        while (spawnedObjects < numberOfObjects)
        {
            float randomX = Random.Range(0, terrainSize.x);
            float randomZ = Random.Range(0, terrainSize.z);
            float y = terrain.SampleHeight(new Vector3(randomX, 0, randomZ)) + terrain.transform.position.y;
            float slope = terrain.terrainData.GetSteepness(randomX / terrainSize.x, randomZ / terrainSize.z);

            if (slope <= maxSlope)
            {
                Vector3 spawnPosition = new Vector3(randomX, y, randomZ);
                GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);

                if ((spawnedObjects + 1) % 100 == 0)
                {
                    spawnedObject.transform.localScale = new Vector3(5f, 5f, 5f);
                }

                spawnedObjects++;
            }
        }
    }
}
