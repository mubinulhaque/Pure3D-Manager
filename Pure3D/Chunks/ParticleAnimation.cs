using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(88072)]
    public class ParticleAnimation : Chunk
    {
        public byte[] Data;

        public ParticleAnimation(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            Data = new BinaryReader(stream).ReadBytes((int)length);
        }

        public override string ToString()
        {
            return $"Particle Animation (TypeID: {Type}) (Len: {Data.Length})";
        }

        public override string ToShortString()
        {
            return "Particle Animation";
        }
    }
}
