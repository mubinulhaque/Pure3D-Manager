using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(117506051)]
    public class CollisionCylinder : Chunk
    {
        public float Radius;
        public float HalfLength;
        public bool FlatEnd;

        public CollisionCylinder(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new(stream);
            Radius = reader.ReadSingle();
            HalfLength = reader.ReadSingle();
            FlatEnd = reader.ReadUInt16() == 1;
        }

        public override string ToString()
        {
            return $"{ToShortString()} (Radius: {Radius}, Half Length: {HalfLength}, Flat End: {FlatEnd})";
        }

        public override string ToShortString()
        {
            return "Collision Cylinder";
        }
    }
}
