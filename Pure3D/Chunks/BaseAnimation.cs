using System.IO;
using Godot;

namespace Pure3D.Chunks
{
    public class BaseAnimation : Chunk
    {
        public uint Version; // I think it's Version at least

        public BaseAnimation(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            if (length != 4)
            {
                GD.PrintErr("Base Animation chunk's data is not 4 bytes long!");
            }
            else
            {
                BinaryReader reader = new(stream);
                Version = reader.ReadUInt32();
            }
        }

        public override string ToString()
        {
            return $"{ToShortString()} (Version: {Version})";
        }

        public override string ToShortString()
        {
            return "Base Animation";
        }
    }

    [ChunkType(88073)]
    public class EmitterAnimation : BaseAnimation
    {
        public EmitterAnimation(File file, uint type) : base(file, type)
        {
        }

        public override string ToShortString()
        {
            return "Emitter Animation";
        }
    }

    [ChunkType(88072)]
    public class ParticleAnimation : BaseAnimation
    {
        public ParticleAnimation(File file, uint type) : base(file, type)
        {
        }

        public override string ToShortString()
        {
            return "Particle Animation";
        }
    }

    [ChunkType(88074)]
    public class GeneratorAnimation : BaseAnimation
    {
        public GeneratorAnimation(File file, uint type) : base(file, type)
        {
        }

        public override string ToShortString()
        {
            return "Generator Animation";
        }
    }
}
