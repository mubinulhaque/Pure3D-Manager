﻿using Godot;
using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(8704)]
    public class Camera : VersionNamed
    {
        public float FieldOfView;
        public float AspectRatio;
        public float NearClip;
        public float FarClip;
        public Vector3 Position;
        public Vector3 Look;
        public Vector3 Up;

        public Camera(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new BinaryReader(stream);
            Name = Util.ReadString(reader);
            Version = reader.ReadUInt32();
            FieldOfView = reader.ReadSingle();
            AspectRatio = reader.ReadSingle();
            NearClip = reader.ReadSingle();
            FarClip = reader.ReadSingle();
            Position = Util.ReadVector3(reader);
            Look = Util.ReadVector3(reader);
            Up = Util.ReadVector3(reader);
        }

        public override string ToString()
        {
            return $"Camera: {Name} (FOV: {FieldOfView}, Aspect Ratio: {AspectRatio}, Near Clip: {NearClip}, Far Clip: {FarClip}, Version: {Version})";
        }

        public override string ToShortString()
        {
            return "Camera";
        }
    }
}
