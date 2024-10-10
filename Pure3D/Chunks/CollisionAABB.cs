using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(117506054)]
    public class CollisionAABB : Chunk
    {
        public uint Nothing; // No, really, this does nothing

        public CollisionAABB(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new(stream);
            Nothing = reader.ReadUInt32();
        }

        public override string ToString()
        {
            return ToShortString();
        }

        public override string ToShortString()
        {
            return "Collision Axis-Aligned Bounding Box";
        }
    }
}
