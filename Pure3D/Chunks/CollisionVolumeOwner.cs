using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(117506081)]
    public class CollisionVolumeOwner : Chunk
    {
        public uint NumberOfNames;

        public CollisionVolumeOwner(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new(stream);
            NumberOfNames = reader.ReadUInt32();
        }

        public override string ToString()
        {
            return $"Collision Volume Owner ({NumberOfNames} Names)";
        }

        public override string ToShortString()
        {
            return "Collision Volume Owner";
        }
    }

    [ChunkType(117506082)]
    public class CollisionVolumeOwnerName : Named
    {
        public CollisionVolumeOwnerName(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            base.ReadHeader(stream, length);
        }

        public override string ToString()
        {
            return $"Collision Volume Owner Name: {Name}";
        }

        public override string ToShortString()
        {
            return "Collision Volume Owner Name";
        }
    }
}
