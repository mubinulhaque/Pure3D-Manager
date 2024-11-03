using Godot;
using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(50331651)]
    public class Road : Named
    {
        public uint RoadType;
        public string Start; // Intersection before the road
        public string End; // Intersection after the road
        public uint MaxCars;
        public uint Speed;
        // Will do these two later
        // They use Speed as a bit mask
        // public uint Difficulty;
        // public uint Shortcut;

        public Road(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            base.ReadHeader(stream, length);
            BinaryReader reader = new(stream);
            RoadType = reader.ReadUInt32();
            Start = Util.ReadString(reader);
            End = Util.ReadString(reader);
            MaxCars = reader.ReadUInt32();
            Speed = reader.ReadUInt32();
        }

        public override string ToString()
        {
            return $"{ToShortString()}: {Name} (Start: {Start}, End: {End}, {MaxCars} Cars Maximum)";
        }

        public override string ToShortString()
        {
            return "Road";
        }
    }

    [ChunkType(50331657)]
    public class RoadData : Named
    {
        public uint RoadType;
        public uint NumberOfLanes;
        public bool HasShoulder;
        public Vector3 Direction;
        public Vector3 Top;
        public Vector3 Bottom;

        public RoadData(File file, uint type) : base(file, type)
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
            return "Road Segment Data";
        }
    }

    [ChunkType(50331650)]
    public class RoadSegment : Named
    {
        public string RoadData;
        public Matrix Transform;
        public Matrix Scale;

        public RoadSegment(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            base.ReadHeader(stream, length);
            BinaryReader reader = new(stream);
            RoadData = Util.ReadString(reader);
            Transform = Util.ReadMatrix(reader);
            Scale = Util.ReadMatrix(reader);
        }

        public override string ToString()
        {
            return $"{ToShortString()}: {Name} (Segment Data: {RoadData})";
        }

        public override string ToShortString()
        {
            return "Road Segment";
        }
    }
}
