﻿using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(94212)]
    public class BillboardPerspectiveInfo : Chunk
    {
        public uint Version;
        public uint Perspective;

        public BillboardPerspectiveInfo(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new(stream);
            Version = reader.ReadUInt32();
            Perspective = reader.ReadUInt32();
        }

        public override string ToString()
        {
            return $"Billboard Perspective Info ({Perspective})";
        }

        public override string ToShortString()
        {
            return "Billboard Perspective Info";
        }
    }
}
