using Godot;
using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(77824)]
    public class Light : VersionNamed
    {
        public LightTypes LightType;
        public uint Colour;
        public float Constant;
        public float Linear;
        public float Squared;
        public bool Enabled;

        public Light(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new(stream);
            Name = Util.ReadString(reader);
            Version = reader.ReadUInt32();
            LightType = (LightTypes)reader.ReadUInt32();
            Colour = reader.ReadUInt32();
            Constant = reader.ReadSingle();
            Linear = reader.ReadSingle();
            Squared = reader.ReadSingle();
            Enabled = reader.ReadUInt32() == 1;
        }

        public override string ToString()
        {
            return $"{ToShortString()}: {Name} (Type: {LightType}, Enabled: {Enabled}, Version: {Version})";
        }

        public enum LightTypes
        {
            AMBIENT,
            POINT,
            DIRECTIONAL,
            SPOT
        }

        public override string ToShortString()
        {
            return "Light";
        }
    }

    public abstract class LightVector : Chunk
    {
        public Vector3 Vector;

        public LightVector(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            Vector = Util.ReadVector3(new BinaryReader(stream));
        }

        public override string ToString()
        {
            return $"{ToShortString()}: {Vector}";
        }

        public override string ToShortString()
        {
            return "Light Vector";
        }
    }

    [ChunkType(77825)]
    public class LightDirection : LightVector
    {
        public LightDirection(File file, uint type) : base(file, type)
        {
        }

        public override string ToShortString()
        {
            return "Light Direction";
        }
    }

    [ChunkType(77826)]
    public class LightPosition : LightDirection
    {
        public LightPosition(File file, uint type) : base(file, type)
        {
        }

        public override string ToShortString()
        {
            return "Light Position";
        }
    }

    [ChunkType(77828)]
    public class LightShadow : Chunk
    {
        public bool Shadow;

        public LightShadow(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            Shadow = new BinaryReader(stream).ReadUInt32() == 1;
        }

        public override string ToString()
        {
            return $"{ToShortString()}: {Shadow}";
        }

        public override string ToShortString()
        {
            return "Light Shadow";
        }
    }
}
