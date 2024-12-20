﻿using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(117510144)]
    public class PhysicsObject : VersionNamed
    {
        public string MaterialName;
        public uint NumJoints;
        public float Volume;
        public float RestingSensitivity;

        public PhysicsObject(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new BinaryReader(stream);
            Name = Util.ReadString(reader);
            Version = reader.ReadUInt32();
            MaterialName = Util.ReadString(reader);
            NumJoints = reader.ReadUInt32();
            Volume = reader.ReadSingle();
            RestingSensitivity = reader.ReadSingle();
        }

        public override string ToString()
        {
            return $"Physics Object: {Name} ({NumJoints} Joints)";
        }

        public override string ToShortString()
        {
            return "Physics Object";
        }
    }
}
