using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(50331657)]
    public class Road : Named
    {
        public uint RoadType;
        public uint NumberOfLanes;
        public bool HasShoulder;
        public Vector3 Direction;
        public Vector3 Top;
        public Vector3 Bottom;

        public Road(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            base.ReadHeader(stream, length);
            BinaryReader reader = new(stream);
            RoadType = reader.ReadUInt32();
            NumberOfLanes = reader.ReadUInt32();
            HasShoulder = reader.ReadUInt32() == 1;
            Direction = Util.ReadVector3(reader);
            Top = Util.ReadVector3(reader);
            Bottom = Util.ReadVector3(reader);
        }

        public override string ToString()
        {
            return $"{ToShortString()} {RoadType}: {Name} ({NumberOfLanes} Lanes, Shoulder: {HasShoulder})";
        }

        public override string ToShortString()
        {
            return "Road";
        }
    }
}
