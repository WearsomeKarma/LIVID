using UnityEngine;

public sealed class Chunk_Generator :
    MonoBehaviour
{
    /// <summary>
    /// The inital distance which 
    /// chunks are generated out to from
    /// the player's position.
    /// </summary>
    [SerializeField]
    private int render_Distance = 1;

    /// <summary>
    /// The player which chunk position is based
    /// off of.
    /// </summary>
    [SerializeField]
    private GameObject player;

    /// <summary>
    /// For this instance of Chunk_Generator this
    /// array indicates the biomes which the generator
    /// will take into consideration during chunk
    /// generation.
    /// </summary>
    [SerializeField]
    private Biome[] defined_Biomes;

    private Chunk[,] chunk_Map;

    public void Start()
    {

    }

    public void Update()
    {

    }
}
