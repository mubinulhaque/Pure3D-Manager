using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    /// <summary>
    /// Parent of an <c>AnimationGroupList</c>
    /// and an <c>AnimationSize</c>.
    /// </summary>
    [ChunkType(1183744)]
    public class AnimationChunk : VersionNamed
    {
        public string AnimType;
        public float NumberOfFrames;
        public float FrameRate;
        public uint Looping;

        public AnimationChunk(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new BinaryReader(stream);
            base.ReadHeader(stream, length);
            AnimType = Util.ZeroTerminate(Encoding.ASCII.GetString(reader.ReadBytes(4)));
            NumberOfFrames = reader.ReadSingle();
            FrameRate = reader.ReadSingle();
            Looping = reader.ReadUInt32();
        }

        public override string ToString()
        {
            return $"Animation: {Name}, Version {Version}, Frames {NumberOfFrames}, FrameRate {FrameRate}, Looping {Looping}";
        }

        public override string ToShortString()
        {
            return "Animation";
        }
    }
}
