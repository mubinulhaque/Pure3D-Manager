﻿using System.IO;

namespace Pure3D.Chunks
{
    /// <summary>
    /// Currently unusuable, because the actual header is supposed to be 50331653 (0x3000005),
    /// but the program can't read it, since the header's too big
    /// </summary>
    [ChunkType(81920)]
    public class Locator : Named
    {
        public uint Version;
        public Vector3 Position;

        public Locator(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new BinaryReader(stream);
            base.ReadHeader(stream, length);
            Version = reader.ReadUInt32();
            Position = Util.ReadVector3(reader);
        }

        public override string ToString()
        {
            return $"Locator: {Name}";
        }

        public override string ToShortString()
        {
            return "Locator";
        }
    }
}
