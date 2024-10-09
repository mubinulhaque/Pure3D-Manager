using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(88065)]
    public class ParticleSystem2 : VersionNamed
    {
        public string Factory;

        public ParticleSystem2(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            base.ReadHeader(stream, length);
            BinaryReader reader = new(stream);
            Factory = Util.ReadString(reader);
        }

        public override string ToString()
        {
            return $"Particle System 2: {Name} (Factory: {Factory}, Version {Version})";
        }

        public override string ToShortString()
        {
            return $"Particle System 2";
        }
    }
}
