using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(88073)]
    public class EmitterAnimation : Chunk
    {
        public byte[] Data;

        public EmitterAnimation(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            Data = new BinaryReader(stream).ReadBytes((int)length);
        }

        public override string ToString()
        {
            return $"Emitter Animation (TypeID: {Type}) (Len: {Data.Length})";
        }

        public override string ToShortString()
        {
            return "Emitter Animation";
        }
    }
}
