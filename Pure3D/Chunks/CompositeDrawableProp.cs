using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(17686)]
    public class CompositeDrawableProp : Named
    {
        public bool IsTranslucent;
        public uint SkeletonJointID;

        public CompositeDrawableProp(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new BinaryReader(stream);
            base.ReadHeader(stream, length);
            IsTranslucent = reader.ReadUInt32() == 1;
            SkeletonJointID = reader.ReadUInt32();
        }

        public override string ToString()
        {
            return $"Composite Drawable Prop: {Name} (Joint: {SkeletonJointID}, Translucent: {IsTranslucent})";
        }

        public override string ToShortString()
        {
            return "Composite Drawable Prop";
        }
    }
}
