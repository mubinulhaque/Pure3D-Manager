using Godot;
using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(117506052)]
    public class CollisionOBB : Chunk
    {
        public Vector3 Scale;

        public CollisionOBB(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new(stream);
            Scale = Util.ReadVector3(reader);
        }

        public override string ToString()
        {
            return $"{ToShortString()} (Scale: {Scale})";
        }

        public override string ToShortString()
        {
            return "Collision Oriented Bounding Box";
        }
    }
}
