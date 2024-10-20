using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(73728)]
    public class GameAttribute : VersionNamed // I think it's VersionNamed, at least
    {
        public uint NumberOfParameters;

        public GameAttribute(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new(stream);
            Name = Util.ReadString(reader);
            Version = reader.ReadUInt32();
            NumberOfParameters = reader.ReadUInt32();
        }

        public override string ToString()
        {
            return $"Game Attribute: {Name} (Number of Parameters: {NumberOfParameters}, Version {Version})";
        }

        public override string ToShortString()
        {
            return $"Game Attribute";
        }
    }
}
