﻿using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    public abstract class ShaderParam : Chunk
    {
        public string Param;

        public ShaderParam(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            Param = Util.ZeroTerminate(Encoding.ASCII.GetString(new BinaryReader(stream).ReadBytes(4)));
        }

        public override string ToString()
        {
            return $"Shader Parameter: {Param}";
        }
    }

    [ChunkType(69634)]
    public class ShaderTextureParam : ShaderParam
    {
        public string Value;

        public ShaderTextureParam(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            base.ReadHeader(stream, length);
            Value = Util.ReadString(new BinaryReader(stream));
        }

        public override string ToString()
        {
            // Prints the name of the parameter and the texture assigned to it
            return $"{Param} Shader Texture Parameter: {Value}";
        }

        public override string ToShortString()
        {
            return "Shader Texture Parameter";
        }
    }

    [ChunkType(69635)]
    public class ShaderIntParam : ShaderParam
    {
        public uint Value;

        public ShaderIntParam(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            base.ReadHeader(stream, length);
            Value = new BinaryReader(stream).ReadUInt32();
        }

        public override string ToString()
        {
            return $"Shader Integer Parameter: {Param}, {Value}";
        }

        public override string ToShortString()
        {
            return "Shader Int Parameter";
        }
    }

    [ChunkType(69636)]
    public class ShaderFloatParam : ShaderParam
    {
        public float Value;

        public ShaderFloatParam(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            base.ReadHeader(stream, length);
            Value = new BinaryReader(stream).ReadSingle();
        }

        public override string ToString()
        {
            return $"Shader Float Parameter: {Param}, {Value}";
        }

        public override string ToShortString()
        {
            return "Shader Float Parameter";
        }
    }

    [ChunkType(69637)]
    public class ShaderColourParam : ShaderParam
    {
        public uint Colour;

        public ShaderColourParam(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            base.ReadHeader(stream, length);
            BinaryReader reader = new BinaryReader(stream);
            Colour = reader.ReadUInt32();
        }

        public override string ToString()
        {
            Godot.Color col = Util.GetColour(Colour);
            // Returns the name of the parameter and the colour assigned to it in RGBA format (0-255)
            return $"{Param} Shader Colour Parameter: ({col.R}, {col.G}, {col.B}, {col.A})";
        }

        public override string ToShortString()
        {
            return "Shader Colour Parameter";
        }
    }
}
