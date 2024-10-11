using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace Pure3D.Chunks
{
    [ChunkType(117506083)]
    public class CollisionAttribute : Chunk
    {
        public bool IsStatic;
        public uint DefaultArea;
        public bool CanRoll;
        public bool CanSlide;
        public bool CanSpin;
        public bool CanBounce;
        public uint ExtraAttribute1;
        public uint ExtraAttribute2;
        public uint ExtraAttribute3;

        public CollisionAttribute(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new(stream);
            IsStatic = reader.ReadUInt16() == 1;
            DefaultArea = reader.ReadUInt32();
            CanRoll = reader.ReadUInt16() == 1;
            CanSlide = reader.ReadUInt16() == 1;
            CanSpin = reader.ReadUInt16() == 1;
            CanBounce = reader.ReadUInt16() == 1;
            ExtraAttribute1 = reader.ReadUInt32();
            ExtraAttribute2 = reader.ReadUInt32();
            ExtraAttribute3 = reader.ReadUInt32();
        }

        public override string ToString()
        {
            return $"{ToShortString()} (Static: {IsStatic}, Default Area: {DefaultArea}, Can Roll: {CanRoll}, Can Slide: {CanSlide}, Can Spin: {CanSpin}, Can Bounce: {CanBounce})";
        }

        public override string ToShortString()
        {
            return "Collision Attribute";
        }
    }
}
