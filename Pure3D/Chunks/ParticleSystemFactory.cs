using System;
using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(88064)]
    public class ParticleSystemFactory : VersionNamed
    {
        public float FramesPerSecond;
        public uint NumberOfFrames;
        public bool Looping;
        public bool Sorting;
        public uint NumberOfEmitters;

        public ParticleSystemFactory(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            base.ReadHeader(stream, length);
            BinaryReader reader = new(stream);
            FramesPerSecond = reader.ReadSingle();
            NumberOfFrames = reader.ReadUInt32();
            reader.ReadUInt32(); // Don't know why, but this is just here
            Looping = reader.ReadUInt16() == 1;
            Sorting = reader.ReadUInt16() == 1;
            NumberOfEmitters = reader.ReadUInt32();
        }

        public override string ToString()
        {
            return $"{ToShortString()}: {Name} (FPS: {FramesPerSecond}, {NumberOfFrames} Frames, Version: {Version})";
        }

        public override string ToShortString()
        {
            return "Particle System Factory";
        }
    }
}
