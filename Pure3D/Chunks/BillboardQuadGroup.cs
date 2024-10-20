using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(94210)]
    public class BillboardQuadGroup : VersionNamed
    {
        public string Shader;
        public bool ZTest;
        public bool ZWrite;
        public uint Fog;
        public uint NumQuads;

        public BillboardQuadGroup(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            base.ReadHeader(stream, length);
            BinaryReader reader = new(stream);
            Shader = Util.ReadString(reader);
            ZTest = reader.ReadUInt32() == 1;
            ZWrite = reader.ReadUInt32() == 1;
            Fog = reader.ReadUInt32();
            NumQuads = reader.ReadUInt32();
        }

        public override string ToString()
        {
            return $"Billboard Quad Group: {Name}, {NumQuads} Quads";
        }

        public override string ToShortString()
        {
            return $"{NumQuads} Billboard Quads";
        }
    }
}
