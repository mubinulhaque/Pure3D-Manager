using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(50335744)]
    public class BreakableObject : Chunk
    {
        public uint Index;
        public uint Count;

        public BreakableObject(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new(stream);
            Index = reader.ReadUInt32();
            Count = reader.ReadUInt32();
        }

        public override string ToString()
        {
            return $"Breakable Object {Index} (Count: {Count})";
        }

        public override string ToShortString()
        {
            return $"Breakable Object";
        }
    }
}
