using System.IO;
using System.Xml.Linq;

namespace Pure3D.Chunks
{
    [ChunkType(117506049)]
    public class CollisionVolume : Chunk
    {
        public uint ObjectReferenceIndex;
        public int OwnerIndex;
        public uint NumberOfSubVolumes;

        public CollisionVolume(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new(stream);
            ObjectReferenceIndex = reader.ReadUInt32();
            OwnerIndex = reader.ReadInt32();
            NumberOfSubVolumes = reader.ReadUInt32();
        }

        public override string ToString()
        {
            return $"Collision Volume ({NumberOfSubVolumes} Sub Volumes)";
        }

        public override string ToShortString()
        {
            return "Collision Volume";
        }
    }
}
