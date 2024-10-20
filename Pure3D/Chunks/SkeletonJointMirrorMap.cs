﻿using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(17667)]
    public class SkeletonJointMirrorMap : Chunk
    {
        public uint MappedJointIndex;
        public float XAxisMap;
        public float YAxisMap;
        public float ZAxisMap;

        public SkeletonJointMirrorMap(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new(stream);
            MappedJointIndex = reader.ReadUInt32();
            XAxisMap = reader.ReadSingle();
            YAxisMap = reader.ReadSingle();
            ZAxisMap = reader.ReadSingle();
        }

        public override string ToString()
        {
            return $"Skeleton Joint Mirror Map (JointIdx: {MappedJointIndex})";
        }

        public override string ToShortString()
        {
            return "Skeleton Joint Mirror Map";
        }
    }
}
