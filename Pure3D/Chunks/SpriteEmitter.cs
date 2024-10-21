using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(88070)]
    public class SpriteEmitter : VersionNamed
    {
        public string Shader;
        public string AngleMode;
        public float Angle;
        public string TextureAnimationMode;
        public uint TextureFrameCount;
        public uint TextureFrameRate;

        public SpriteEmitter(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            base.ReadHeader(stream, length);
            BinaryReader reader = new(stream);
            Shader = Util.ReadString(reader);
            AngleMode = Util.ZeroTerminate(Encoding.ASCII.GetString(reader.ReadBytes(4)));
            Angle = reader.ReadSingle();
            TextureAnimationMode = Util.ZeroTerminate(Encoding.ASCII.GetString(reader.ReadBytes(4)));
            TextureFrameCount = reader.ReadUInt32();
            TextureFrameRate = reader.ReadUInt32();
        }

        public override string ToString()
        {
            return $"{ToShortString()} : {Name} (Shader: {Shader}, Angle Mode: {AngleMode}, Angle: {Angle}, Version: {Version})";
        }

        public override string ToShortString()
        {
            return "Sprite Emitter";
        }
    }
}
