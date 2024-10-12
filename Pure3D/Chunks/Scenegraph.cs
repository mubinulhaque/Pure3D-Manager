using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(1179904)]
    public class Scenegraph : VersionNamed
    {

        public Scenegraph(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new(stream);
            Name = Util.ReadString(reader);
            Version = reader.ReadUInt32();
        }

        public override string ToString()
        {
            return $"{ToShortString()}: {Name} (Version: {Version})";
        }

        public override string ToShortString()
        {
            return "Scenegraph";
        }
    }

    [ChunkType(1179905)]
    public class ScenegraphRoot : Chunk
    {
        public ScenegraphRoot(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new(stream);
        }

        public override string ToString()
        {
            return ToShortString();
        }

        public override string ToShortString()
        {
            return "Scenegraph Root";
        }
    }

    [ChunkType(1179906)]
    public class ScenegraphBranch : Named
    {
        public uint NumberOfChildren;

        public ScenegraphBranch(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            base.ReadHeader(stream, length);
            BinaryReader reader = new(stream);
            NumberOfChildren = reader.ReadUInt32();
        }

        public override string ToString()
        {
            return $"{ToShortString()}: {Name} ({NumberOfChildren} Children)";
        }

        public override string ToShortString()
        {
            return "Scenegraph Branch";
        }
    }

    [ChunkType(1179907)]
    public class ScenegraphTransform : Named
    {
        public uint NumberOfChildren;
        public Matrix Transform;

        public ScenegraphTransform(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            base.ReadHeader(stream, length);
            BinaryReader reader = new(stream);
            NumberOfChildren = reader.ReadUInt32();
            Transform = Util.ReadMatrix(reader);
        }

        public override string ToString()
        {
            return $"{ToShortString()}: {Name} ({NumberOfChildren} Children)";
        }

        public override string ToShortString()
        {
            return "Scenegraph Transform";
        }
    }
}
