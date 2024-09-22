using System.IO;
using System.Text;
using Godot;

namespace Pure3D.Chunks
{
    [ChunkType(1184007)]
    public class EntityChannel : Chunk
    {
        public uint NumberOfFrames;
        public string[] Values;
        public uint Version;
        public string Param;
        public ushort[] Frames;

        public EntityChannel(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new BinaryReader(stream);
            Version = reader.ReadUInt32();
            Param = Util.ZeroTerminate(Encoding.ASCII.GetString(reader.ReadBytes(4)));
            NumberOfFrames = reader.ReadUInt32();
            Frames = new ushort[NumberOfFrames];

            for (int i = 0; i < NumberOfFrames; i++)
            {
                Frames[i] = reader.ReadUInt16();
            }

            Values = new string[NumberOfFrames];
            
            for (int i = 0; i < NumberOfFrames; i++)
            {
                Values[i] = Util.ReadString(reader);
            }
        }

        public override string ToString()
        {
            return $"{Param} Entity Channel Version: {Version}, Frames: {NumberOfFrames}";
        }

        public override string ToShortString()
        {
            return $"Entity Channel";
        }
    }
}