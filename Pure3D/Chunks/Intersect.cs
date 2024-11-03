using Godot;
using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(66060291)]
    public class Intersect : Chunk
    {
        public uint[] Indices;
        public Vector3[] Positions;
        public Vector3[] Normals;

        public Intersect(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new(stream);
            uint len = reader.ReadUInt32();
            Indices = new uint[len];
            for (int i = 0; i < len; i++)
                Indices[i] = reader.ReadUInt32();

            len = reader.ReadUInt32();
            Positions = new Vector3[len];
            for (int i = 0; i < len; i++)
                Positions[i] = Util.ReadVector3(reader);

            len = reader.ReadUInt32();
            Normals = new Vector3[len];
            for (int i = 0; i < len; i++)
                Normals[i] = Util.ReadVector3(reader);
        }

        public override string ToString()
        {
            return $"Intersect ({Indices.Length} Indices, {Positions.Length} Positions, {Normals.Length} Normals)";
        }

        public override string ToShortString()
        {
            return $"Intersect";
        }
    }
}
