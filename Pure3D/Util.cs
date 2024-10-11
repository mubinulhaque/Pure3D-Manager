using System;
using System.IO;
using System.Text;
using Godot;

namespace Pure3D
{
    public sealed class Util
    {
        // ReadString accessor because Pure3D loves null terminated strings.
        public static string ReadString(BinaryReader reader)
        {
            byte strLen = reader.ReadByte();
            string str = Encoding.ASCII.GetString(reader.ReadBytes(strLen));
            str = ZeroTerminate(str);

            return str;
        }

        public static string ZeroTerminate(string str)
        {
            int length = str.IndexOf(char.MinValue);
            return length != -1 ? str.Substring(0, length) : str;
        }

        public static Vector2 ReadVector2(BinaryReader reader)
        {
            Vector2 vector = new Vector2();

            vector.X = reader.ReadSingle();
            vector.Y = reader.ReadSingle();

            return vector;
        }

        public static Vector3 ReadVector3(BinaryReader reader)
        {
            Vector3 vector = new Vector3();

            vector.X = reader.ReadSingle();
            vector.Y = reader.ReadSingle();
            vector.Z = reader.ReadSingle();

            return vector;
        }

        public static Quaternion ReadQuaternion(BinaryReader reader)
        {
            Quaternion vector = new Quaternion();

            vector.X = reader.ReadSingle();
            vector.Y = reader.ReadSingle();
            vector.Z = reader.ReadSingle();
            vector.W = reader.ReadSingle();

            return vector;
        }

        public static Matrix ReadMatrix(BinaryReader reader)
        {
            Matrix matrix = new Matrix();

            matrix.M11 = reader.ReadSingle();
            matrix.M12 = reader.ReadSingle();
            matrix.M13 = reader.ReadSingle();
            matrix.M14 = reader.ReadSingle();
            matrix.M21 = reader.ReadSingle();
            matrix.M22 = reader.ReadSingle();
            matrix.M23 = reader.ReadSingle();
            matrix.M24 = reader.ReadSingle();
            matrix.M31 = reader.ReadSingle();
            matrix.M32 = reader.ReadSingle();
            matrix.M33 = reader.ReadSingle();
            matrix.M34 = reader.ReadSingle();
            matrix.M41 = reader.ReadSingle();
            matrix.M42 = reader.ReadSingle();
            matrix.M43 = reader.ReadSingle();
            matrix.M44 = reader.ReadSingle();

            return matrix;
        }

        /// <summary>
        /// Displays a Pure3D colour properly in Godot
        /// </summary>
        /// <param name="p3dColour">Pure3D colour</param>
        /// <returns>Godot <c>Color</c></returns>
        public static Color GetColour(uint p3dColour)
        {
            // Pure3D colours are in the format BGRA
            // Godot colours are in the format RGBA
            byte[] values = BitConverter.GetBytes(p3dColour);

            // So we correct it here
            return Color.Color8(values[2], values[1], values[0], values[3]);
        }

        /// <summary>
        /// Prints a Pure3D Vector3 in a readable format
        /// </summary>
        /// <param name="vector">Pure3D Vector3</param>
        /// <returns>String form of Vector3</returns>
        public static string PrintVector3(Pure3D.Vector3 vector)
        {
            return $"({vector.X}, {vector.Y}, {vector.Z})";
        }
    }
}
