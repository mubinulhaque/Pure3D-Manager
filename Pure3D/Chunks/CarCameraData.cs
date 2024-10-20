using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(50331904)]
    public class CarCameraData : Chunk
    {
        public uint Index;
        public float Rotation;
        public float Angle;
        public float Distance;
        public Vector3 Look;

        public CarCameraData(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new BinaryReader(stream);
            Index = reader.ReadUInt32();
            Rotation = reader.ReadSingle();
            Angle = reader.ReadSingle();
            Distance = reader.ReadSingle();
            Look = Util.ReadVector3(reader);
        }

        public override string ToString()
        {
            return $"({ToShortString()}) {Index} (Rotation: {Rotation}, Angle: {Angle}, Distance: {Distance}, Look: {Look})";
        }

        public override string ToShortString()
        {
            return "Car Camera Data";
        }
    }
}
