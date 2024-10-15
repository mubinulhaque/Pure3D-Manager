using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(28672)]
    public class History : Chunk
    {
        public uint NumberOfLines;
        public string[] Lines;

        public History(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new(stream);
            NumberOfLines = reader.ReadByte();

            // For some reason, the first string read is always empty
            // So we don't include it in the Lines array
            Util.ReadString(reader);
            Lines = new string[NumberOfLines];
            for (uint i = 0; i < NumberOfLines; i++)
                Lines[i] = Util.ReadString(reader);
        }

        public override string ToString()
        {
            return $"{ToShortString()}: {Lines[0]} ({NumberOfLines} Lines)";
        }

        public override string ToShortString()
        {
            return "History";
        }
    }
}
