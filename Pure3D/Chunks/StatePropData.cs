using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(134348800)]
    public class StatePropData : VersionNamed
    {
        public string ObjectFactory;
        public uint NumberOfStates;

        public StatePropData(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            base.ReadHeader(stream, length);
            BinaryReader reader = new(stream);
            ObjectFactory = Util.ReadString(reader);
            NumberOfStates = reader.ReadUInt32();
        }

        public override string ToString()
        {
            return $"{ToShortString()}: {Name} (Object Factory: {ObjectFactory}, {NumberOfStates} States, Version: {Version})";
        }

        public override string ToShortString()
        {
            return "State Prop Data";
        }
    }
}
