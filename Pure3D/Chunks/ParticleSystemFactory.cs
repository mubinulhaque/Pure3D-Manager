using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(88064)]
    public class ParticleSystemFactory : Chunk
    {
        public byte[] Data;
        private uint unknownType;

        public ParticleSystemFactory(File file, uint type) : base(file, type)
        {
            unknownType = type;
        }

        public override void ReadHeader(Stream stream, long length)
        {
            Data = new BinaryReader(stream).ReadBytes((int)length);
        }

        public override string ToString()
        {
            return $"Particle System Factory (TypeID: {unknownType}) (Len: {Data.Length})";
        }

        public override string ToShortString()
        {
            return "Particle System Factory";
        }
    }
}
