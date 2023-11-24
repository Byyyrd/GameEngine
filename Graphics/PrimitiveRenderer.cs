using Silk.NET.OpenGL;

namespace GameEngine.Graphics
{
    internal class PrimitiveRenderer
    {
        private readonly List<float> _vertices = new();
        private readonly List<uint> _indices = new();
        private readonly PrimitiveType _renderType;
        private readonly GL _gl;
        private readonly uint _stride;
        public PrimitiveRenderer(GL gl, uint stride, PrimitiveType renderType)
        {
            _gl = gl;
            _stride = stride;
            _renderType = renderType;
        }


        public unsafe void Render()
        {
            BufferObject<float> vbo = new(_gl, _vertices.ToArray(), BufferTargetARB.ArrayBuffer);
            BufferObject<uint> ebo = new(_gl, _indices.ToArray(), BufferTargetARB.ElementArrayBuffer);
            VertexArrayObject<float, uint> vao = new(_gl, vbo, ebo);
            vao.AttributePointer(0, 3, _stride, 0);
            vao.AttributePointer(1, 3, _stride, 3);
            //Draw triangles
            vao.Bind();
            _gl.DrawElements(_renderType, (uint)_indices.Count, DrawElementsType.UnsignedInt, (void*)(0 * sizeof(uint)));
            _vertices.Clear();
            _indices.Clear();
        }
        public void AddElement(uint[] indices, float[] vertices)
        {
            for (int i = 0; i < indices.Length; i++)
            {
                indices[i] += (uint)(_vertices.Count / 6);
            }
            _indices.AddRange(indices);
            _vertices.AddRange(vertices);
        }


    }
}
