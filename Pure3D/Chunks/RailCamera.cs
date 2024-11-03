using Godot;
using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(50331658)]
    public class RailCamera : Named
    {
        public RailcamBehaviour Behaviour;
        public float MinRadius;
        public float MaxRadius;
        public bool TrackRail;
        public float TrackDistance;
        public bool ReverseSense;
        public float FOV;
        public Vector3 TargetOffset;
        public Vector3 AxisPlay;
        public float PositionLag;
        public float TargetLag;

        public RailCamera(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            base.ReadHeader(stream, length);
            BinaryReader reader = new(stream);
            Behaviour = (RailcamBehaviour)reader.ReadUInt32();
            MinRadius = reader.ReadSingle();
            MaxRadius = reader.ReadSingle();
            TrackRail = reader.ReadUInt32() == 1;
            TrackDistance = reader.ReadSingle();
            ReverseSense = reader.ReadUInt32() == 1;
            FOV = reader.ReadSingle();
            TargetOffset = Util.ReadVector3(reader);
            AxisPlay = Util.ReadVector3(reader);
            PositionLag = reader.ReadSingle();
            TargetLag = reader.ReadSingle();
            //Data = new BinaryReader(stream).ReadBytes((int)length);
        }

        public override string ToString()
        {
            return $"{ToShortString()}: {Name} (Behaviour: {Behaviour}, Radius: {MinRadius} - {MaxRadius})";
        }

        public override string ToShortString()
        {
            return "Rail Camera";
        }
    }

    public enum RailcamBehaviour
    {
        DISTANCE = 1,
        PROJECTION
    }
}
