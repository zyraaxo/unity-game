using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandGenerator : MonoBehaviour
{
    [Header("Island Settings")]
    public int width = 100;
    public int height = 100;
    public float scale = 20f;
    public float heightMultiplier = 5f;
    public float islandRadius = 40f;
    public Vector2 offset;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        GenerateIsland();
    }

    void GenerateIsland()
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[width * height];
        int[] triangles = new int[(width - 1) * (height - 1) * 6];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float xCoord = (float)x / width * scale + offset.x;
                float yCoord = (float)y / height * scale + offset.y;

                // Generate height using Perlin noise
                float perlinValue = Mathf.PerlinNoise(xCoord, yCoord);

                // Calculate distance from center to give island shape
                float distanceFromCenter = Vector2.Distance(new Vector2(x, y), new Vector2(width / 2f, height / 2f)) / islandRadius;
                distanceFromCenter = Mathf.Clamp01(distanceFromCenter);  // Keep the value between 0 and 1

                // Apply Perlin noise and distance-based falloff to create island effect
                float heightValue = perlinValue * heightMultiplier * Mathf.Clamp01(1 - distanceFromCenter);

                vertices[y * width + x] = new Vector3(x, heightValue, y);
            }
        }

        int triIndex = 0;
        for (int y = 0; y < height - 1; y++)
        {
            for (int x = 0; x < width - 1; x++)
            {
                int vertexIndex = y * width + x;

                // First triangle
                triangles[triIndex] = vertexIndex;
                triangles[triIndex + 1] = vertexIndex + width;
                triangles[triIndex + 2] = vertexIndex + 1;

                // Second triangle
                triangles[triIndex + 3] = vertexIndex + 1;
                triangles[triIndex + 4] = vertexIndex + width;
                triangles[triIndex + 5] = vertexIndex + width + 1;

                triIndex += 6;
            }
        }

        // Assign vertices and triangles to the mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        // Apply the mesh to the MeshFilter
        meshFilter.mesh = mesh;
    }

    // Optional: Display the island in the editor to help with debugging
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (meshFilter != null && meshFilter.mesh != null)
        {
            Vector3[] vertices = meshFilter.mesh.vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                Gizmos.DrawSphere(vertices[i], 0.1f);
            }
        }
    }
}
