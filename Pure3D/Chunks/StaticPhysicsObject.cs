using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(66060289)]
    public class StaticPhysicsObject : Named
    {
        public byte[] Data;

        public StaticPhysicsObject(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new(stream);
            byte strLen = reader.ReadByte();
            Name = Encoding.ASCII.GetString(reader.ReadBytes(strLen));
            Name = Util.ZeroTerminate(Name);
            Data = reader.ReadBytes((int)(length - strLen - 1));
        }

        public override string ToString()
        {
            return $"Static Physics Object: {Name} ({Data.Length} Bytes)";
        }

        public override string ToShortString()
        {
            return $"Static Physics Object";
        }
    }
}
