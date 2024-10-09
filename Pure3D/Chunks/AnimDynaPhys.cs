using System.IO;
using Godot;

namespace Pure3D.Chunks
{
    [ChunkType(66060302)]
    public class AnimDynaPhys : Named
    {
        public uint Version;
        public uint RenderOrder;

        public AnimDynaPhys(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            base.ReadHeader(stream, length);
            BinaryReader reader = new(stream);
            Version = reader.ReadUInt32();
            RenderOrder = reader.ReadUInt32();
        }

        public override string ToString()
        {
            return $"Animated Dynamic Physics Object: {Name} (Render Order: {RenderOrder}, Version {Version}))";
        }

        public override string ToShortString()
        {
            return $"Animated Dynamic Physics Object";
        }
    }
}
