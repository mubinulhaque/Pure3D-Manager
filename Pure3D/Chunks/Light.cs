using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(77824)]
    public class Light : VersionNamed
    {
        public uint LightType;
        public uint Colour;
        public float Constant;
        public float Linear;
        public float Squared;
        public bool Enabled;

        public Light(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new(stream);
            Name = Util.ReadString(reader);
            Version = reader.ReadUInt32();
            LightType = reader.ReadUInt32();
            Colour = reader.ReadUInt32();
            Constant = reader.ReadSingle();
            Linear = reader.ReadSingle();
            Squared = reader.ReadSingle();
            Enabled = reader.ReadUInt32() == 1;
        }

        public override string ToString()
        {
            return $"{ToShortString()}: {Name} (Type: {LightType}, Enabled: {Enabled}, Version: {Version})";
        }

        public override string ToShortString()
        {
            return "Light";
        }
    }
}
