using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(50331662)]
    public class SurfaceList : Chunk
    {
        public uint Version;
        public uint NumberOfSurfaces;
        public byte[] Surfaces;

        public SurfaceList(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new(stream);
            Version = reader.ReadUInt32();
            NumberOfSurfaces = reader.ReadUInt32();

            Surfaces = new byte[NumberOfSurfaces];
            for (int i = 0; i < NumberOfSurfaces; i++)
                Surfaces[i] = reader.ReadByte();
        }

        public override string ToString()
        {
            return $"Surface List ({ToShortString()}, Version: {Version})";
        }

        public override string ToShortString()
        {
            return $"{NumberOfSurfaces} Surfaces";
        }
    }
}
