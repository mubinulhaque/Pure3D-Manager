using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(88069)]
    public class BaseEmitter : Chunk
    {
        public byte[] Data;
        private uint unknownType;

        public BaseEmitter(File file, uint type) : base(file, type)
        {
            unknownType = type;
        }

        public override void ReadHeader(Stream stream, long length)
        {
            Data = new BinaryReader(stream).ReadBytes((int)length);
        }

        public override string ToString()
        {
            return $"Base Emitter (TypeID: {unknownType}) (Len: {Data.Length})";
        }

        public override string ToShortString()
        {
            return "Base Emitter";
        }
    }
}
