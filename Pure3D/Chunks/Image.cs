using System.IO;
using Godot;

namespace Pure3D.Chunks
{
    /// <summary>
    /// Always has a single child of ImageData, which contains the binary data of the Image
    /// </summary>
    [ChunkType(102401)]
    public class Image : Named
    {
        public uint Version;
        public uint Width;
        public uint Height;
        public uint Bpp;
        public uint Palettized;
        public uint HasAlpha;
        public Formats Format;

        public Image(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new BinaryReader(stream);
            base.ReadHeader(stream, length);
            Version = reader.ReadUInt32();
            Width = reader.ReadUInt32();
            Height = reader.ReadUInt32();
            Bpp = reader.ReadUInt32();
            Palettized = reader.ReadUInt32();
            HasAlpha = reader.ReadUInt32();
            Format = (Formats)reader.ReadUInt32();
        }

        public override string ToString()
        {
            return $"Image Chunk: {Name} ({Format}) ({Width}x{Height})";
        }

        public enum Formats : uint
        {
            PNG = 1,
            DXT1 = 6,
            DXT3 = 8,
            DXT5 = 10,
            P3DI = 11,
            P3DI2 = 25,
        }

        public override string ToShortString()
        {
            return "Image";
        }

        public byte[] loadImageData()
        {
            if (Children.Count != 1) GD.PrintErr($"Image {Name}: invalid number of children!");
            else if (Children[0] is not ImageData) GD.PrintErr($"Image {Name}: no ImageData child!");
            else
            {
                ImageData child = (ImageData)Children[0];

                if (child != null) return child.Data;
                else GD.PrintErr($"Image {Name}: invalid ImageData child!");
            }
            
            return new byte[] {};
        }
    }
}
