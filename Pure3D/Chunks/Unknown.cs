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
            if (unknownType == 50331653)
            {
                // Currently Locators cannot be created as a Chunk,
                // so this is just to let the user know that it is there
                // (check the Locator class for more detail)
                return $"Locator (TypeID: {unknownType}) (Len: {Data.Length})";
            } else
            {
                return $"Unknown Chunk (TypeID: {unknownType}) (Len: {Data.Length})";
            }
        }
    }
}
