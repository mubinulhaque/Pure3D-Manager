using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(66060302)]
    public class AnimatedDynamicPhysicsObject : VersionNamed
    {
        public uint RenderOrder;

        public AnimatedDynamicPhysicsObject(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new(stream);
            Name = Util.ReadString(reader);
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
