using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(18592)]
    public class MultiController : VersionNamed
    {
        public uint NumberOfTracks;
        public float Length;
        public float FrameRate;

        public MultiController(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new(stream);
            Name = Util.ReadString(reader);
            Version = reader.ReadUInt32();
            Length = reader.ReadSingle();
            FrameRate = reader.ReadSingle();
            NumberOfTracks = reader.ReadUInt32();
        }

        public override string ToString()
        {
            return $"Multi Controller: {Name} ({NumberOfTracks} Tracks {Length} Long, Version: {Version})";
        }

        public override string ToShortString()
        {
            return "Multi Controller";
        }
    }
}
