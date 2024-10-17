using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(66060299)]
    public class WorldSphere : VersionNamed
    {
        public uint NumberOfMeshes;
        public uint NumberOfBillboardQuadGroups;

        public WorldSphere(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new(stream);
            Name = Util.ReadString(reader);
            Version = reader.ReadUInt32();
            NumberOfMeshes = reader.ReadUInt32();
            NumberOfBillboardQuadGroups = reader.ReadUInt32();
        }

        public override string ToString()
        {
            return $"{ToShortString()}: {Name} ({NumberOfMeshes} Meshes, {NumberOfBillboardQuadGroups} Billboard Quad Groups, Version: {Version})";
        }

        public override string ToShortString()
        {
            return "World Sphere";
        }
    }
}
