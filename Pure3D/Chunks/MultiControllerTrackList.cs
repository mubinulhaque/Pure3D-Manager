using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(18593)]
    public class MultiControllerTrackList : Chunk
    {
        public uint NumberOfTracks;
        public string[] Names;
        public float[] Starts;
        public float[] Ends;
        public float[] Scales;

        public MultiControllerTrackList(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new(stream);
            NumberOfTracks = reader.ReadUInt32();

            Names = new string[NumberOfTracks];
            Starts = new float[NumberOfTracks];
            Ends = new float[NumberOfTracks];
            Scales = new float[NumberOfTracks];

            for (uint i = 0; i < NumberOfTracks; i++)
            {
                Names[i] = Util.ReadString(reader);
                Starts[i] = reader.ReadSingle();
                Ends[i] = reader.ReadSingle();
                Scales[i] = reader.ReadSingle();
            }
        }

        public override string ToString()
        {
            return $"Multi Controller Track List ({NumberOfTracks} Tracks)";
        }

        public override string ToShortString()
        {
            return $"{NumberOfTracks} Multi Controller Tracks";
        }
    }
}
