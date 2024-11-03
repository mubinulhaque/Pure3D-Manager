using Godot;
using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(50331659)]
    public class Path : Chunk
    {
        public uint NumberOfPositions;
        public Vector3[] Positions;

        public Path(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new(stream);
            NumberOfPositions = reader.ReadUInt32();

            Positions = new Vector3[NumberOfPositions];
            for (uint i = 0; i < NumberOfPositions; i++)
                Positions[i] = Util.ReadVector3(reader);
        }

        public override string ToString()
        {
            return $"{ToShortString()} ({NumberOfPositions} Positions)";
        }

        public override string ToShortString()
        {
            return "Path";
        }
    }
}
