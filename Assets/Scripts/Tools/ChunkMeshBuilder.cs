using UnityEngine;

public static class ChunkMeshBuilder
{
    private const int TRIANGLE_OFFSET = 6;

    public static Mesh Build_Height_Mesh(Chunk chunk, int size)
    {
        Vector3[] vertices = new Vector3[size*size];
        int sizeOffset = size - 1;
        int[] triangles = new int[sizeOffset * sizeOffset * TRIANGLE_OFFSET];

        for(int z = 0, v = 0; z < size; z++)
        {
            for(int x = 0; x < size; x++)
            {
                float y = (float)chunk[x,z];
                vertices[v] = new Vector3(x, y, z);
                v++;
            }
        }

        int vert = 0;
        int tri  = 0;
        for(int tri_z = 0; tri_z < sizeOffset; tri_z++)
        {
            for(int tri_x = 0; tri_x < sizeOffset; tri_x++)
            {
                triangles[tri]   = vert;
                triangles[tri+1] = vert + sizeOffset + 1;
                triangles[tri+2] = vert + 1;
                triangles[tri+3] = vert + 1;
                triangles[tri+4] = vert + sizeOffset + 1;
                triangles[tri+5] = vert + sizeOffset + 2;

                tri+=TRIANGLE_OFFSET;
                vert++;
            }
            vert++;
        }

        Mesh mesh = new Mesh();
        
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        return mesh;
    }
}
