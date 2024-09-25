using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public int width = 500;
    public int depth = 500;
    public float maxHeight = 10f; // Maximum height of the peaks
    public int numberOfPeaks = 15; // Set number of peaks
    public GameObject treePrefab; // Assign your tree prefab here
    public GameObject treePrefabSmall; // Assign your small tree prefab here

    public int treeCount = 100; // Number of large trees to spawn
    public int treeCountSmall = 100; // Number of small trees to spawn
    public TerrainLayer[] terrainLayers; // Array of terrain layers

    private void Start()
    {
        Terrain terrain = Terrain.activeTerrain;
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
        SpawnTrees(terrain);
        ApplyTerrainLayers(terrain);
    }

    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, maxHeight, depth);

        float[,] heights = new float[width, depth];

        // Generate random peaks
        for (int i = 0; i < numberOfPeaks; i++)
        {
            float peakX = Random.Range(0, width);
            float peakZ = Random.Range(0, depth);
            float peakHeight = Random.Range(2f, maxHeight);

            // Create a peak shape
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < depth; z++)
                {
                    float distance = Vector2.Distance(new Vector2(x, z), new Vector2(peakX, peakZ));
                    float heightContribution = peakHeight * Mathf.Exp(-distance / 10);
                    heights[x, z] += heightContribution;
                }
            }
        }

        terrainData.SetHeights(0, 0, heights);
        return terrainData;
    }

    void SpawnTrees(Terrain terrain)
    {
        // Spawn large trees
        for (int i = 0; i < treeCount; i++)
        {
            SpawnTree(treePrefab, terrain);
        }

        // Spawn small trees
        for (int i = 0; i < treeCountSmall; i++)
        {
            SpawnTree(treePrefabSmall, terrain);
        }
    }

    void SpawnTree(GameObject treePrefab, Terrain terrain)
    {
        float xPos = Random.Range(0, width);
        float zPos = Random.Range(0, depth);
        float yPos = terrain.SampleHeight(new Vector3(xPos, 0, zPos)); // Get the terrain height at the position

        // Instantiate the tree prefab
        Instantiate(treePrefab, new Vector3(xPos, yPos, zPos), Quaternion.identity);
    }

    void ApplyTerrainLayers(Terrain terrain)
    {
        TerrainData terrainData = terrain.terrainData;

        // Get the current heights for the terrain
        float[,] heights = terrainData.GetHeights(0, 0, width, depth);

        // Create a new splat map to apply terrain layers
        float[,,] splatmapData = new float[width, depth, terrainLayers.Length];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                // Randomly choose a terrain layer to apply based on the height
                int randomLayerIndex = Random.Range(0, terrainLayers.Length);
                splatmapData[x, z, randomLayerIndex] = 1; // Set the weight for this layer
            }
        }

        // Apply the splat map to the terrain
        terrainData.SetAlphamaps(0, 0, splatmapData);
    }
}
