using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(9088)]
    public class LightGroup : Named
    {
        public uint NumberOfLights;
        public string[] Lights;

        public LightGroup(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            base.ReadHeader(stream, length);
            BinaryReader reader = new(stream);
            NumberOfLights = reader.ReadUInt32();

            Lights = new string[NumberOfLights];
            for (uint i = 0; i < NumberOfLights; i++)
                Lights[i] = Util.ReadString(reader);
        }

        public override string ToString()
        {
            return $"{ToShortString()}: {Name} ({NumberOfLights} Lights)";
        }

        public override string ToShortString()
        {
            return "Light Group";
        }
    }
}
