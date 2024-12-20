using System.IO;

namespace Pure3D.Chunks
{
    /// <summary>
    /// Parent of <c>AnimationGroup</c>s that
    /// define the animation of a <c>Skeleton</c>
    /// </summary>
    [ChunkType(1183746)]
    public class AnimationGroupList : Chunk
    {
        public uint Version;
        public uint NumberOfGroups;

        public AnimationGroupList(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new BinaryReader(stream);
            Version = reader.ReadUInt32();
            NumberOfGroups = reader.ReadUInt32();
        }

        public override string ToString()
        {
            return $"Animation Group List: {NumberOfGroups}";
        }

        public override string ToShortString()
        {
            return $"{NumberOfGroups} Animation Groups";
        }
    }
}
