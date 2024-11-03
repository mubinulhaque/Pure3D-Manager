using Godot;
using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(50331654)]
    public class TriggerVolume : Named
    {
        public bool IsRect;
        public Vector3 Scale;
        public Matrix Shape;

        public TriggerVolume(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            base.ReadHeader(stream, length);
            BinaryReader reader = new(stream);
            IsRect = reader.ReadUInt32() == 1;
            Scale = Util.ReadVector3(reader);
            Shape = Util.ReadMatrix(reader);
        }

        public override string ToString()
        {
            return $"{ToShortString()}: {Name} (Rect: {IsRect}, Scale: {Scale})";
        }

        public override string ToShortString()
        {
            return "Trigger Volume";
        }
    }
}
