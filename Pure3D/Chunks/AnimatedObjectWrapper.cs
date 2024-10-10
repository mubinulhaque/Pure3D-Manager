using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(66060304)]
    public class AnimatedObjectWrapper : VersionNamed
    {
        public bool HasAlpha;

        public AnimatedObjectWrapper(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new(stream);
            Name = Util.ReadString(reader);
            Version = reader.ReadByte();
            HasAlpha = reader.ReadByte() == 1;
        }

        public override string ToString()
        {
            return $"Animated Object Wrapper: {Name} (Alpha: {HasAlpha}, Version: {Version})";
        }

        public override string ToShortString()
        {
            return "Animated Object Wrapper";
        }
    }
}
