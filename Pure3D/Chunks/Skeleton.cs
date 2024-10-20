﻿using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(17664)]
    public class Skeleton : VersionNamed
    {
        protected uint NumJoints;

        public Skeleton(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new BinaryReader(stream);
            Name = Util.ReadString(reader);
            Version = reader.ReadUInt32();
            NumJoints = reader.ReadUInt32();
        }

        public override string ToString()
        {
            return $"Skeleton: {Name}";
        }

        public override string ToShortString()
        {
            return "Skeleton";
        }

        public string GetNumJoints()
        {
            return NumJoints.ToString();
        }
    }
}
