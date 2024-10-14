using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(69632)]
    public class Shader : VersionNamed
    {
        public string PddiShaderName;
        public bool HasTranslucency;
        public uint VertexNeeds;
        public uint VertexMask;
        protected uint NumParams; // Should match the number of children

        public Shader(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new BinaryReader(stream);
            Name = Util.ReadString(reader);
            Version = reader.ReadUInt32();
            PddiShaderName = Util.ReadString(reader);
            HasTranslucency = reader.ReadUInt32() == 1;
            VertexNeeds = reader.ReadUInt32();
            VertexMask = reader.ReadUInt32();
            NumParams = reader.ReadUInt32();
        }

        public override string ToString()
        {
            return $"Shader: {Name} ({PddiShaderName})";
        }

        public override string ToShortString()
        {
            return "Shader";
        }

        public uint GetNumParams()
        {
            return NumParams;
        }
    }
}
