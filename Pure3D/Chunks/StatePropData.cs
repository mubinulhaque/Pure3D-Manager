using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(134348800)]
    public class StatePropData : VersionNamed
    {
        public string ObjectFactory;
        public uint NumberOfStates;

        public StatePropData(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            base.ReadHeader(stream, length);
            BinaryReader reader = new(stream);
            ObjectFactory = Util.ReadString(reader);
            NumberOfStates = reader.ReadUInt32();
        }

        public override string ToString()
        {
            return $"{ToShortString()}: {Name} (Object Factory: {ObjectFactory}, {NumberOfStates} States, Version: {Version})";
        }

        public override string ToShortString()
        {
            return "State Prop Data";
        }
    }

    [ChunkType(134348801)]
    public class StatePropState : Named
    {
        public bool AutoTransition;
        public uint OutState;
        public uint NumberOfDrawables;
        public uint NumberOfFrameControllers;
        public uint NumberOfEvents;
        public uint NumberOfCallbacks;
        public float OutFrame;

        public StatePropState(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            base.ReadHeader(stream, length);
            BinaryReader reader = new(stream);
            AutoTransition = reader.ReadUInt32() == 1;
            OutState = reader.ReadUInt32();
            NumberOfDrawables = reader.ReadUInt32();
            NumberOfFrameControllers = reader.ReadUInt32();
            NumberOfEvents = reader.ReadUInt32();
            NumberOfCallbacks = reader.ReadUInt32();
            OutFrame = reader.ReadSingle();
        }

        public override string ToString()
        {
            return $"{ToShortString()}: {Name} ({NumberOfDrawables} Drawables, {NumberOfFrameControllers} Frame Controllers, {NumberOfEvents} Events, {NumberOfCallbacks} Callbacks)";
        }

        public override string ToShortString()
        {
            return "State Prop State";
        }
    }

    [ChunkType(134348802)]
    public class StatePropVisibility : Named
    {
        public bool Visible;

        public StatePropVisibility(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            base.ReadHeader(stream, length);
            BinaryReader reader = new(stream);
            Visible = reader.ReadUInt32() == 1;
        }

        public override string ToString()
        {
            return $"{ToShortString()}: {Name} (Visible: {Visible})";
        }

        public override string ToShortString()
        {
            return "State Prop Visibility";
        }
    }

    [ChunkType(134348803)]
    public class StatePropFrameController : Named
    {
        public bool IsCyclic;
        public uint NumberOfCycles;
        public uint HoldFrame;
        public float MinFrame;
        public float MaxFrame;
        public float RelativeSpeed;

        public StatePropFrameController(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            base.ReadHeader(stream, length);
            BinaryReader reader = new(stream);
            IsCyclic = reader.ReadUInt32() == 1;
            NumberOfCycles = reader.ReadUInt32();
            HoldFrame = reader.ReadUInt32();
            MinFrame = reader.ReadSingle();
            MaxFrame = reader.ReadSingle();
            RelativeSpeed = reader.ReadSingle();
        }

        public override string ToString()
        {
            return $"{ToShortString()}: {Name} (Cyclic: {IsCyclic}, {NumberOfCycles} Cycles, Frame: {MinFrame} - {MaxFrame}, Relative Speed: {RelativeSpeed})";
        }

        public override string ToShortString()
        {
            return "State Prop Frame Controller";
        }
    }
}
