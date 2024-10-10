using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(117506048)]
    public class CollisionObject : VersionNamed
    {
        public uint NumberOfOwners;
        public string Material;
        public uint NumberOfSubObjects;

        public CollisionObject(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new(stream);
            Name = Util.ReadString(reader);
            Version = reader.ReadUInt32();
            Material = Util.ReadString(reader);
            NumberOfSubObjects = reader.ReadUInt32();
            NumberOfOwners = reader.ReadUInt32();
        }

        public override string ToString()
        {
            return $"Collision Object: {Name} ({NumberOfOwners} Owners, {NumberOfSubObjects} Sub Objects, Version: {Version})";
        }

        public override string ToShortString()
        {
            return "Collision Object";
        }
    }
}
