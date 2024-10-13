using System.IO;
using System.Text;
using Godot;

namespace Pure3D.Chunks
{
    /// <summary>
    /// Abstract class for the child of an Animation Group chunk</c>.
    /// </summary>
    public abstract class AnimationChannel : Chunk
    {
        public uint Version;
        public uint NumberOfFrames;
        public string Parameter;
        public ushort[] Frames;

        public AnimationChannel(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new(stream);
            Version = reader.ReadUInt32();
            Parameter = Util.ZeroTerminate(Encoding.ASCII.GetString(reader.ReadBytes(4)));
            NumberOfFrames = reader.ReadUInt32();

            Frames = new ushort[NumberOfFrames];
            for (int i = 0; i < NumberOfFrames; i++)
            {
                Frames[i] = reader.ReadUInt16();
            }
        }

        public override string ToString()
        {
            return $"{ToShortString()}: {Parameter}, {NumberOfFrames} Frames";
        }

        public override string ToShortString()
        {
            return "Animation Channel";
        }
    }

    [ChunkType(1184008)]
    public class BooleanChannel : AnimationChannel
    {
        public uint Start;
        public ushort[] Values;

        public BooleanChannel(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new(stream);
            Version = reader.ReadUInt32();
            Parameter = Util.ZeroTerminate(Encoding.ASCII.GetString(reader.ReadBytes(4)));
            NumberOfFrames = reader.ReadUInt16();

            Start = reader.ReadUInt16();

            Frames = new ushort[NumberOfFrames];
            for (int i = 0; i < NumberOfFrames; i++)
            {
                Frames[i] = reader.ReadUInt16();
            }

            Values = new ushort[NumberOfFrames];
            for (int i = 0; i < NumberOfFrames; i++)
            {
                Values[i] = reader.ReadUInt16();
            }
        }

        public override string ToShortString()
        {
            return "Boolean Channel";
        }
    }

    [ChunkType(1184009)]
    public class ColourChannel : AnimationChannel
    {
        public uint[] Values;

        public ColourChannel(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            base.ReadHeader(stream, length);
            BinaryReader reader = new(stream);

            Values = new uint[NumberOfFrames];
            for (int i = 0; i < NumberOfFrames; i++)
            {
                Values[i] = reader.ReadUInt32();
            }
        }

        public override string ToShortString()
        {
            return "Colour Channel";
        }
    }

    /// <summary>
    /// Animation for the rotation of a <c>SkeletonJoint</c>.
    /// </summary>
    [ChunkType(1184017)]
    public class CompressedQuaternionChannel : AnimationChannel
    {
        public Quaternion[] Values;

        public CompressedQuaternionChannel(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            base.ReadHeader(stream, length);
            BinaryReader reader = new(stream);

            Values = new Quaternion[NumberOfFrames];
            for (int i = 0; i < NumberOfFrames; i++)
            {
                var w = reader.ReadInt16() / (float)short.MaxValue;
                Values[i] = new Quaternion
                (
                    reader.ReadInt16() / (float)short.MaxValue,
                    reader.ReadInt16() / (float)short.MaxValue,
                    reader.ReadInt16() / (float)short.MaxValue,
                    w
                );
            }
        }

        public override string ToShortString()
        {
            return "Compressed Quaternion Channel";
        }
    }

    [ChunkType(1184007)]
    public class EntityChannel : AnimationChannel
    {
        public string[] Values;

        public EntityChannel(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            base.ReadHeader(stream, length);
            BinaryReader reader = new(stream);

            Values = new string[NumberOfFrames];
            for (int i = 0; i < NumberOfFrames; i++)
            {
                Values[i] = Util.ReadString(reader);
            }
        }

        public override string ToShortString()
        {
            return $"Entity Channel";
        }
    }

    [ChunkType(1184000)]
    public class Float1Channel : AnimationChannel
    {
        public float[] Values;

        public Float1Channel(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            base.ReadHeader(stream, length);
            BinaryReader reader = new(stream);

            Values = new float[NumberOfFrames];
            for (int i = 0; i < NumberOfFrames; i++)
            {
                Values[i] = reader.ReadSingle();
            }
        }

        public override string ToShortString()
        {
            return "Float 1 Channel";
        }
    }

    [ChunkType(1184014)]
    public class IntegerChannel : AnimationChannel
    {
        public uint[] Values;

        public IntegerChannel(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            base.ReadHeader(stream, length);
            BinaryReader reader = new(stream);

            Values = new uint[NumberOfFrames];
            for (int i = 0; i < NumberOfFrames; i++)
            {
                Values[i] = reader.ReadUInt32();
            }
        }

        public override string ToShortString()
        {
            return "Integer Channel";
        }
    }

    /// <summary>
    /// Animation for the rotation of a <c>SkeletonJoint</c>.
    /// </summary>
    [ChunkType(1184005)]
    public class QuaternionChannel : AnimationChannel
    {
        public Quaternion[] Values;

        public QuaternionChannel(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            base.ReadHeader(stream, length);
            BinaryReader reader = new(stream);

            Values = new Quaternion[NumberOfFrames];
            for (int i = 0; i < NumberOfFrames; i++)
            {
                var w = reader.ReadSingle();
                Values[i] = new Quaternion
                (
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    w
                );
            }
        }

        public override string ToShortString()
        {
            return "Quaternion Channel";
        }
    }

    /// <summary>
    /// Animation for the transform of a <c>SkeletonJoint</c>.
    /// </summary>
    [ChunkType(1184002)]
    public class Vector1Channel : AnimationChannel
    {
        public ushort Mapping;
        public float[] Values;
        public Vector3 Constants;

        public Vector1Channel(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new BinaryReader(stream);
            Version = reader.ReadUInt32();
            Parameter = Util.ZeroTerminate(Encoding.ASCII.GetString(reader.ReadBytes(4)));
            Mapping = reader.ReadUInt16();
            Constants = Util.ReadVector3(reader);
            NumberOfFrames = reader.ReadUInt32();

            Frames = new ushort[NumberOfFrames];
            for (int i = 0; i < NumberOfFrames; i++)
            {
                Frames[i] = reader.ReadUInt16();
            }

            Values = new float[NumberOfFrames];
            for (int i = 0; i < NumberOfFrames; i++)
            {
                Values[i] = reader.ReadSingle();
            }
        }

        public override string ToShortString()
        {
            return "Vector 1 Channel";
        }
    }

    /// <summary>
    /// Animation for the transform of a <c>SkeletonJoint</c>.
    /// </summary>
    [ChunkType(1184003)]
    public class Vector2Channel : AnimationChannel
    {
        public ushort Mapping;
        public Vector2[] Values;
        public Vector3 Constants;

        public Vector2Channel(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new(stream);
            Version = reader.ReadUInt32();
            Parameter = Util.ZeroTerminate(Encoding.ASCII.GetString(reader.ReadBytes(4)));
            Mapping = reader.ReadUInt16();
            Constants = Util.ReadVector3(reader);
            NumberOfFrames = reader.ReadUInt32();

            Frames = new ushort[NumberOfFrames];
            for (int i = 0; i < NumberOfFrames; i++)
            {
                Frames[i] = reader.ReadUInt16();
            }

            Values = new Vector2[NumberOfFrames];
            for (int i = 0; i < NumberOfFrames; i++)
            {
                Values[i] = Util.ReadVector2(reader);
            }
        }

        public override string ToShortString()
        {
            return "Vector 2 Channel";
        }
    }

    /// <summary>
    /// Animation for the transform of a <c>SkeletonJoint</c>.
    /// </summary>
    [ChunkType(1184004)]
    public class Vector3Channel : AnimationChannel
    {
        public Vector3[] Values;

        public Vector3Channel(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            base.ReadHeader(stream, length);
            BinaryReader reader = new BinaryReader(stream);

            Values = new Vector3[NumberOfFrames];
            for (int i = 0; i < NumberOfFrames; i++)
            {
                Values[i] = Util.ReadVector3(reader);
            }
        }

        public override string ToShortString()
        {
            return "Vector 3 Channel";
        }
    }
}
