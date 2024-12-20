﻿using Godot;
using System.IO;

namespace Pure3D.Chunks
{
    /// <summary>
    /// Currently unused
    /// </summary>
    [ChunkType(81920)]
    public class Locator : VersionNamed
    {
        public Vector3 Position;

        public Locator(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new BinaryReader(stream);
            Name = Util.ReadString(reader);
            Version = reader.ReadUInt32();
            Position = Util.ReadVector3(reader);
        }

        public override string ToString()
        {
            return $"Locator: {Name} (Position: ({Position.X}, {Position.Y}, {Position.Z}), Version: {Version})";
        }

        public override string ToShortString()
        {
            return "Locator";
        }
    }

    /// <summary>
    /// Used for spawning objects in the world at a specific position
    /// </summary>
    [ChunkType(50331653)]
    public class Locator2 : Named
    {
        /// <summary>
        /// <para>Number from 0 to 15 that determines the object of the Locator</para>
        /// </summary>
        public Types LocatorType;
        /// <summary>
        /// Size of the <c>Data</c> array
        /// </summary>
        public uint DataSize;
        /// <summary>
        /// Collection of elements four bytes each
        /// </summary>
        public Locator2Data[] Data;
        /// <summary>
        /// Position of the Locator
        /// </summary>
        public Vector3 Position;
        /// <summary>
        /// Amount of Trigger Volumes attached
        /// </summary>
        public uint NumberOfTriggers;

        public Locator2(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new BinaryReader(stream);
            base.ReadHeader(stream, length);

            LocatorType = (Types)reader.ReadUInt32();
            DataSize = reader.ReadUInt32();
            Data = new Locator2Data[DataSize];

            for (int i = 0; i < DataSize; i++)
            {
                Data[i] = new Locator2Data(
                    reader.ReadByte(),
                    reader.ReadByte(),
                    reader.ReadByte(),
                    reader.ReadByte()
                );
            }

            Position = Util.ReadVector3(reader);
            NumberOfTriggers = reader.ReadUInt32();
        }

        public override string ToString()
        {
            return $"{Name} Locator Type: {LocatorType} ({NumberOfTriggers} Triggers)";
        }

        public override string ToShortString()
        {
            return "Locator 2";
        }

        public enum Types : uint
        {
            /// <summary>
            /// Triggers events
            /// </summary>
            EVENT,
            /// <summary>
            /// Triggers scripts
            /// </summary>
            SCRIPT,
            /// <summary>
            /// Wasps, gag models and gag triggers
            /// </summary>
            WASP_GAG,
            /// <summary>
            /// Cars and characters
            /// </summary>
            CAR_START,
            SPLINE,
            /// <summary>
            /// Loads/unloads specific parts of the world
            /// </summary>
            WORLD_LOADING,
            OCCLUSION,
            INTERIOR,
            /// <summary>
            /// Defines player's position upon entering an interior
            /// </summary>
            PLAYER_POS,
            /// <summary>
            /// Creates an Action that the player can trigger
            /// </summary>
            ACTION,
            FOV,
            /// <summary>
            /// Unused in Hit & Run
            /// </summary>
            UNUSED,
            CAMERA,
            /// <summary>
            /// Changes active <c>Ped Group</c>
            /// </summary>
            PED,
            COIN,
            /// <summary>
            /// Spawns a Wasp Camera at a <c>WASP_GAG</c> Locator
            /// </summary>
            WASP_SPAWN
        }
    }

    /// <summary>
    /// Four bytes that can be translated into different data types
    /// </summary>
    public class Locator2Data
    {
        /// <summary>
        /// Four bytes to be converted into a different data type
        /// </summary>
        public byte[] Bytes;

        public Locator2Data(byte byte1, byte byte2, byte byte3, byte byte4)
        {
            Bytes = new byte[4]
            {
                byte1,
                byte2,
                byte3,
                byte4
            };
        }

        public override string ToString()
        {
            return $"{Bytes[0]}, {Bytes[1]}, {Bytes[2]}, {Bytes[3]}";
        }
    }

    [ChunkType(50331660)]
    public class LocatorMatrix : Chunk
    {
        public Matrix Transform;

        public LocatorMatrix(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new(stream);
            Transform = Util.ReadMatrix(reader);
        }

        public override string ToString()
        {
            string[] matrix = Util.PrintMatrix(Transform);
            return $"{ToShortString()} (Transform: {matrix[0]}, {matrix[1]}, {matrix[2]}, {matrix[3]})";
        }

        public override string ToShortString()
        {
            return "Locator Matrix";
        }
    }
}
