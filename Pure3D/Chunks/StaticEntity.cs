using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(66060288)]
    public class StaticEntity : Named
    {
        public uint Version; // I think it's Version, at least
        public uint RenderOrder;

        public StaticEntity(File file, uint type) : base(file, type)
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
            return $"Static Entity: {Name} (Version {Version}, Render Order: {RenderOrder})";
        }

        public override string ToShortString()
        {
            return $"Static Entity";
        }
    }
}
