using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(88075)]
    public class ParticleInstancingInfo : Chunk
    {
        public uint Version;
        public uint MaxInstances;

        public ParticleInstancingInfo(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new(stream);
            Version = reader.ReadUInt32();
            MaxInstances = reader.ReadUInt32();
        }

        public override string ToString()
        {
            return $"{ToShortString()} ({MaxInstances} Maximum Instances, Version: {Version})";
        }

        public override string ToShortString()
        {
            return "Particle Instancing Info";
        }
    }
}
