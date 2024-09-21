using System.IO;
using System.Text;
using Godot;

namespace Pure3D.Chunks
{
    [ChunkType(1184256)]
    public class FrameController : VersionNamed
    {
        public string Value;
        public uint FrameOffset;
        public string HierarchyName;
        public string AnimName;

        public FrameController(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(System.IO.Stream stream, long length)
        {
            BinaryReader reader = new BinaryReader(stream);
            base.ReadHeader(stream, length);

            Value = Util.ZeroTerminate(Encoding.ASCII.GetString(reader.ReadBytes(4)));
            FrameOffset = reader.ReadUInt32();
            HierarchyName = Util.ReadString(reader);
            AnimName = Util.ReadString(reader);
        }

        public override string ToString()
        {
            return $"Frame Controller: {Name}, Version {Version}";
        }

        public override string ToShortString()
        {
            return "Frame Controller";
        }
    }
}