using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(65559)]
    public class RenderStatus : Chunk
    {
        public bool CastShadow;

        public RenderStatus(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new(stream);
            CastShadow = reader.ReadUInt32() == 1;
        }

        public override string ToString()
        {
            return $"Render Status (Casts Shadow: {CastShadow})";
        }

        public override string ToShortString()
        {
            return $"Render Status";
        }
    }
}
