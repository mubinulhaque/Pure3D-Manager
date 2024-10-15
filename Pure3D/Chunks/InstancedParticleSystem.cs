using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(50335745)]
    public class InstancedParticleSystem : Chunk
    {
        public uint Index;
        public uint NumberOfInstances;

        public InstancedParticleSystem(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new(stream);
            Index = reader.ReadUInt32();
            NumberOfInstances = reader.ReadUInt32();
        }

        public override string ToString()
        {
            return $"{ToShortString()}: {Index} ({NumberOfInstances} Instances)";
        }

        public override string ToShortString()
        {
            return "Instanced Particle System";
        }
    }
}
