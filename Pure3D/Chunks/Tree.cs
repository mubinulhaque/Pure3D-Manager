using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(66060292)]
    public class TreeHierarchy : Chunk
    {
        public uint NumberOfTrees;
        public Vector3 MinBounds;
        public Vector3 MaxBounds;

        public TreeHierarchy(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new(stream);
            NumberOfTrees = reader.ReadUInt32();
            MinBounds = Util.ReadVector3(reader);
            MaxBounds = Util.ReadVector3(reader);
        }

        public override string ToString()
        {
            return $"Tree Hierarchy ({ToShortString()}, Bounds: {MinBounds} - {MaxBounds})";
        }

        public override string ToShortString()
        {
            return $"{NumberOfTrees} Trees";
        }
    }
}
