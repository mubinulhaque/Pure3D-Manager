using System.IO;

namespace Pure3D.Chunks
{
    public class Unknown : Chunk
    {
        public byte[] Data;
        private uint unknownType;

        public Unknown(File file, uint type) : base(file, type)
        {
            unknownType = type;
        }

        public override void ReadHeader(Stream stream, long length)
        {
            Data = new BinaryReader(stream).ReadBytes((int)length);
        }

        public override string ToString()
        {
            return $"Unknown Chunk (TypeID: {unknownType} (0x{unknownType:X})) ({Data.Length} Bytes)";
        }

        public override string ToShortString()
        {
            return $"Chunk 0x{unknownType:X}";
        }
    }
}
