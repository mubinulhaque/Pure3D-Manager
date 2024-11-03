using Godot;
using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(66060292)]
    public class TreeHierarchy : Chunk
    {
        public uint NumberOfChildren;
        public Vector3 MinBounds;
        public Vector3 MaxBounds;

        public TreeHierarchy(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new(stream);
            NumberOfChildren = reader.ReadUInt32();
            MinBounds = Util.ReadVector3(reader);
            MaxBounds = Util.ReadVector3(reader);
        }

        public override string ToString()
        {
            return $"{ToShortString()} ({NumberOfChildren} Children, Bounds: {MinBounds} - {MaxBounds})";
        }

        public override string ToShortString()
        {
            return "Tree Hierarchy";
        }
    }

    [ChunkType(66060293)]
    public class TreeNode : Chunk
    {
        public uint NumberOfChildren;
        public int ParentOffset;

        public TreeNode(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new(stream);
            NumberOfChildren = reader.ReadUInt32();
            ParentOffset = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"{ToShortString()} ({NumberOfChildren} Children, Parent Offset: {ParentOffset})";
        }

        public override string ToShortString()
        {
            return "Tree Node";
        }
    }

    [ChunkType(66060294)]
    public class TreeNode2 : Chunk
    {
        public TreeAxis Axis;
        public float Position;
        public uint StaticEntityLimit;
        public uint StaticPhysicsEntityLimit;
        public uint IntersectLimit;
        public uint DynamicPhysicsEntityLimit;
        public uint FenceLimit;
        public uint RoadLimit;
        public uint PathLimit;
        public uint AnimatedEntityLimit;

        public TreeNode2(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new(stream);
            Axis = (TreeAxis)reader.ReadByte();
            Position = reader.ReadSingle();
            StaticEntityLimit = reader.ReadUInt32();
            StaticPhysicsEntityLimit = reader.ReadUInt32();
            IntersectLimit = reader.ReadUInt32();
            DynamicPhysicsEntityLimit = reader.ReadUInt32();
            FenceLimit = reader.ReadUInt32();
            RoadLimit = reader.ReadUInt32();
            PathLimit = reader.ReadUInt32();
            AnimatedEntityLimit = reader.ReadUInt32();
        }

        public override string ToString()
        {
            return $"{ToShortString()} (Axis: {Axis}, Position: {Position})";
        }

        public override string ToShortString()
        {
            return "Tree Node 2";
        }

        public enum TreeAxis
        {
            X,
            Y,
            Z,
            None = 255
        }
    }
}
