using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(17688)]
    public class CompositeDrawableEffect : CompositeDrawableProp
    {
        public CompositeDrawableEffect(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            base.ReadHeader(stream, length);
        }

        public override string ToString()
        {
            return $"Composite Drawable Effect: {Name} (Joint: {SkeletonJointID}, Translucent: {IsTranslucent})";
        }

        public override string ToShortString()
        {
            return $"Composite Drawable Effect";
        }
    }
}
