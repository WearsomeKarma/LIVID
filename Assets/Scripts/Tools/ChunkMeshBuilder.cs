using UnityEngine;

public static class ChunkMeshBuilder
{
    private const int TRIANGLE_OFFSET = 6;

    public static Mesh Build_Height_Mesh(NoiseMap heightMap)
    {
        Vector3[] vertices = new Vector3[heightMap.SIZE * heightMap.SIZE];
        int[] triangles = new int[vertices.Length * TRIANGLE_OFFSET];

        for(int z = 0, i = 0; z < heightMap.SIZE; z++)
        {
            for(int x = 0; x < heightMap.SIZE; x++)
            {
                vertices[i] = new Vector3(x, (float)heightMap[x,z], z);
                i++;

                if (i % TRIANGLE_OFFSET != 0)
                    continue;

                triangles[i]   = i;
                triangles[i+1] = i + heightMap.SIZE;
                triangles[i+2] = i + 1;
                triangles[i+3] = i + 1;
                triangles[i+4] = i + heightMap.SIZE + 1;
                triangles[i+5] = i + heightMap.SIZE + 2;
            }
        }

        Mesh mesh = new Mesh();
        
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        return mesh;
    }
}
