﻿using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(65537)]
    public class SkinChunk : MeshChunk
    {
        /// <summary>
        /// Name of the associated Skeleton
        /// </summary>
        public string SkeletonName;

        public SkinChunk(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new BinaryReader(stream);
            Name = Util.ReadString(new BinaryReader(stream));
            Version = reader.ReadUInt32();
            SkeletonName = Util.ReadString(reader);
            NumPrimGroups = reader.ReadUInt32();
        }

        public override string ToString()
        {
            return $"Skin: {Name} (Skeleton: {SkeletonName}) ({NumPrimGroups} Prim Groups)";
        }

        public override string ToShortString()
        {
            return "Skin";
        }
    }
}
