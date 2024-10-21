using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(17668)]
    public class SkeletonJointBonePreserve : Chunk
    {
        public bool PreserveBoneLengths;

        public SkeletonJointBonePreserve(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            PreserveBoneLengths = new BinaryReader(stream).ReadUInt32() == 1;
        }

        public override string ToString()
        {
            return $"Skeleton Joint Bone Preserve (Preserve Bone Lengths: {PreserveBoneLengths})";
        }

        public override string ToShortString()
        {
            return "Skeleton Joint Bone Preserve";
        }
    }
}
