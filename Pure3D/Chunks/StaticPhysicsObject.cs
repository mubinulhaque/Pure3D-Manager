using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(66060289)]
    public class StaticPhysicsObject : VersionNamed
    {

        public StaticPhysicsObject(File file, uint type) : base(file, type)
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
            return $"Static Physics Object: {Name} (Version: {Version})";
        }

        public override string ToShortString()
        {
            return $"Static Physics Object";
        }
    }
}
