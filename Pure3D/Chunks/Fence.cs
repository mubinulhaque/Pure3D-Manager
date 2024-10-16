using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(66060295)]
    public class Fence : Chunk
    {

        public Fence(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
        }

        public override string ToString()
        {
            return ToShortString();
        }

        public override string ToShortString()
        {
            return "Fence";
        }
    }

    [ChunkType(50331648)]
    public class Fence2 : Chunk
    {
        public Vector3 Start;
        public Vector3 End;
        public Vector3 Normal;

        public Fence2(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new(stream);
            Start = Util.ReadVector3(reader);
            End = Util.ReadVector3(reader);
            Normal = Util.ReadVector3(reader);
        }

        public override string ToString()
        {
            return $"{ToShortString()}: ({Start} - {End})";
        }

        public override string ToShortString()
        {
            return "Fence 2";
        }
    }
}
