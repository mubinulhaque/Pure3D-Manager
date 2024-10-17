using System.IO;

namespace Pure3D.Chunks
{
    public abstract class Entity : VersionNamed // I think it's VersionNamed, at least
    {
        public uint RenderOrder;

        public Entity(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new(stream);
            Name = Util.ReadString(reader);
            Version = reader.ReadUInt32();
            RenderOrder = reader.ReadUInt32();
        }

        public override string ToString()
        {
            return $"{ToShortString()}: {Name} (Render Order: {RenderOrder}, Version {Version})";
        }

        public override string ToShortString()
        {
            return "Entity";
        }
    }

    [ChunkType(66060302)]
    public class AnimatedDynamicPhysicsObject : Entity
    {
        public AnimatedDynamicPhysicsObject(File file, uint type) : base(file, type)
        {
        }

        public override string ToShortString()
        {
            return $"Animated Dynamic Physics Object";
        }
    }

    [ChunkType(66060290)]
    public class DynamicPhysicsObject : Entity
    {
        public DynamicPhysicsObject(File file, uint type) : base(file, type)
        {
        }

        public override string ToShortString()
        {
            return "Dynamic Physics Object";
        }
    }

    [ChunkType(66060297)]
    public class InstaStaticEntity : Entity
    {
        public InstaStaticEntity(File file, uint type) : base(file, type)
        {
        }

        public override string ToShortString()
        {
            return "Instanced Static Entity";
        }
    }

    [ChunkType(66060298)]
    public class InstaStaticPhysicsObject : Entity
    {
        public InstaStaticPhysicsObject(File file, uint type) : base(file, type)
        {
        }

        public override string ToShortString()
        {
            return "Instanced Static Physics Object";
        }
    }

    [ChunkType(66060288)]
    public class StaticEntity : Entity
    {
        public StaticEntity(File file, uint type) : base(file, type)
        {
        }

        public override string ToShortString()
        {
            return "Static Entity";
        }
    }
}
