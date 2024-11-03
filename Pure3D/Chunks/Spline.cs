using Godot;
using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(50331655)]
    public class Spline : Named
    {
        public uint NumberOfPositions;
        public Vector3[] Positions;

        public Spline(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            base.ReadHeader(stream, length);
            BinaryReader reader = new(stream);
            NumberOfPositions = reader.ReadUInt32();

            Positions = new Vector3[NumberOfPositions];
            for (int i = 0; i < NumberOfPositions; i++)
                Positions[i] = Util.ReadVector3(reader);
        }

        public override string ToString()
        {
            return $"{ToShortString()}: {Name} ({NumberOfPositions} Positions)";
        }

        public override string ToShortString()
        {
            return "Spline";
        }
    }
}
