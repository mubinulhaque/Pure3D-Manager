﻿using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(65536)]
    public class MeshChunk : VersionNamed
    {
        public uint NumPrimGroups;

        public MeshChunk(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new BinaryReader(stream);
            Name = Util.ReadString(reader);
            Version = reader.ReadUInt32();
            NumPrimGroups = reader.ReadUInt32();
        }

        public override string ToString()
        {
            return $"Mesh: {Name} ({NumPrimGroups} Prim Groups)";
        }

        public override string ToShortString()
        {
            return "Mesh";
        }
    }
}
