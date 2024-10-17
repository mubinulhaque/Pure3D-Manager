using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(66060301)]
    public class LensFlare : VersionNamed
    {
        public uint NumberOfBillboardQuadGroups;

        public LensFlare(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new(stream);
            Name = Util.ReadString(reader);
            Version = reader.ReadUInt32();
            NumberOfBillboardQuadGroups = reader.ReadUInt32();
        }

        public override string ToString()
        {
            return $"{ToShortString()}: {Name} ({NumberOfBillboardQuadGroups} Billboard Quad Groups, Version: {Version})";
        }

        public override string ToShortString()
        {
            return "Lens Flare";
        }
    }
}
