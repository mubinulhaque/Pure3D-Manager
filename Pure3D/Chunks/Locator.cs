using System.IO;

namespace Pure3D.Chunks
{
    /// <summary>
    /// Currently unused
    /// </summary>
    [ChunkType(81920)]
    public class Locator : Named
    {
        public uint Version;
        public Vector3 Position;

        public Locator(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new BinaryReader(stream);
            base.ReadHeader(stream, length);
            Version = reader.ReadUInt32();
            Position = Util.ReadVector3(reader);
        }

        public override string ToString()
        {
            return $"Locator: {Name}";
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
        /// <para><c>0</c> - Triggers events</para>
        /// <para><c>1</c> - Triggers scripts</para>
        /// <para><c>2</c> - Wasps, gag models and gag triggers</para>
        /// <para><c>3</c> - Cars and characters</para>
        /// <para><c>4</c> - Splines</para>
        /// <para><c>5</c> - Loads/unloads specific parts of the world</para>
        /// <para><c>6</c> - Occlusion zones</para>
        /// <para><c>7</c> - Interior entrances</para>
        /// <para><c>8</c> - Player's rotation upon entering an interior</para>
        /// <para><c>9</c> - Creates an <c>Action Button</c></para>
        /// <para><c>10</c> - FOV</para>
        /// <para><c>11</c> - Unused</para>
        /// <para><c>12</c> - Static cameras</para>
        /// <para><c>13</c> - Changes active <c>Ped Group</c></para>
        /// <para><c>14</c> - Coins</para>
        /// <para><c>15</c> - Spawns a Wasp Camera at a Type 2 Locator</para>
        /// </summary>
        public uint LocatorType;
        /// <summary>
        /// Amount of Trigger Volumes attached
        /// </summary>
        public uint NumberOfTriggers;
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

        public Locator2(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            BinaryReader reader = new BinaryReader(stream);
            base.ReadHeader(stream, length);

            LocatorType = reader.ReadUInt32();
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
            reader.ReadBytes(4);
        }

        public override string ToString()
        {
            return $"{Name} Locator Type: {LocatorType}";
        }

        public override string ToShortString()
        {
            return "Locator 2";
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
        public byte[] bytes;

        public Locator2Data(byte byte1, byte byte2, byte byte3, byte byte4)
        {
            bytes = new byte[4]
            {
                byte1,
                byte2,
                byte3,
                byte4
            };
        }
    }
}
