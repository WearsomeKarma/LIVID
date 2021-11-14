using UnityEngine;

public abstract class Chunk_Post_Processor :
    MonoBehaviour 
{
    protected int Seed { get; private set; }

    internal void Initalize(int postProcessorSeed)
    {
        Seed = postProcessorSeed;
    }

    internal abstract void Post_Process_Chunk(Runtime_Chunk c);
}
