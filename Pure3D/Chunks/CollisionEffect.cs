using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(50333184)]
    public class CollisionEffect : Chunk
    {
        public uint Classtype;
        public uint PhysicsProp;
        public string Sound;

        public CollisionEffect(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new(stream);
            Classtype = reader.ReadUInt32();
            PhysicsProp = reader.ReadUInt32();
            Sound = Util.ReadString(reader);
        }

        public override string ToString()
        {
            return $"{ToShortString()} (Type: {Classtype}, Physics Prop: {PhysicsProp}, Sound: {Sound})";
        }

        public override string ToShortString()
        {
            return "Collision Effect";
        }
    }
}
