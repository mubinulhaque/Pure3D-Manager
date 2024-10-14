using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(65553)]
    public class VertexShader : Named
    {

        public VertexShader(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(Stream stream, long length)
        {
            base.ReadHeader(stream, length);
        }

        public override string ToString()
        {
            return $"Vertex Shader: {Name}";
        }

        public override string ToShortString()
        {
            return "Vertex Shader";
        }
    }
}
