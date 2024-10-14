using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(117506051)]
    public class CollisionCylinder : CollisionSphere
    {
        public float HalfLength;
        public bool FlatEnd;

        public CollisionCylinder(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            base.ReadHeader(stream, length);
            BinaryReader reader = new(stream);
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

    [ChunkType(117506050)]
    public class CollisionSphere : Chunk
    {
        public float Radius;

        public CollisionSphere(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            Radius = new BinaryReader(stream).ReadSingle();
        }

        public override string ToString()
        {
            return $"{ToShortString()} (Radius: {Radius})";
        }

        public override string ToShortString()
        {
            return "Collision Sphere";
        }
    }
}
