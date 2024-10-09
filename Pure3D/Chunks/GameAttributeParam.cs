using System.IO;

namespace Pure3D.Chunks
{
    public abstract class GameAttributeParam : Chunk
    {
        public string Parameter;
        public uint Value;

        public GameAttributeParam(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new(stream);
            Parameter = Util.ReadString(reader);
            Value = reader.ReadUInt32();
        }

        public override string ToString()
        {
            return $"Game Attribute Parameter ({Parameter}: {Value})";
        }

        public override string ToShortString()
        {
            return "Game Attribute Parameter";
        }
    }

    [ChunkType(73729)]
    public class GameAttributeIntParam : GameAttributeParam
    {
        public GameAttributeIntParam(File file, uint type) : base(file, type)
        {
        }

        public override string ToShortString()
        {
            return "Game Attribute Integer Parameter";
        }
    }
}
