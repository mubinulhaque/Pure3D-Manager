using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(66060298)]
    public class InstaStaticPhysicsObject : Named
    {
        public uint Version; // I think it's Version, at least
        public uint RenderOrder;

        public InstaStaticPhysicsObject(File file, uint type) : base(file, type)
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
            return $"Instanced Static Physics Object: {Name} (Version {Version}, Render Order: {RenderOrder})";
        }

        public override string ToShortString()
        {
            return $"Instanced Static Physics Object";
        }
    }
}
