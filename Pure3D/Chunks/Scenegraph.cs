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
}
