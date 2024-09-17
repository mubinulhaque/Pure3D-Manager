using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(88075)]
    public class ParticleInstancingInfo : Chunk
    {
        public byte[] Data;
        private uint unknownType;

        public ParticleInstancingInfo(File file, uint type) : base(file, type)
        {
            unknownType = type;
        }

        public override void ReadHeader(Stream stream, long length)
        {
            Data = new BinaryReader(stream).ReadBytes((int)length);
        }

        public override string ToString()
        {
            return $"Particle Instancing Info (TypeID: {unknownType}) (Len: {Data.Length})";
        }

        public override string ToShortString()
        {
            return "Particle Instancing Info";
        }
    }
}
