using Godot;
using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(117506055)]
    public class CollisionVector : Chunk
    {
        public Vector3 Vector;

        public CollisionVector(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new(stream);
            Vector = Util.ReadVector3(reader);
        }

        public override string ToString()
        {
            return $"{ToShortString()} {Vector}";
        }

        public override string ToShortString()
        {
            return "Collision Vector";
        }
    }
}
