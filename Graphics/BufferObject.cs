using Silk.NET.OpenGL;

namespace GameEngine.Graphics
{
    internal class BufferObject<T> where T : unmanaged
    {
        private readonly GL _gl;
        private readonly uint _vbo;
        private readonly BufferTargetARB _target;
        public unsafe BufferObject(GL _pGl, T[] _data, BufferTargetARB _pTarget)
        {
            _gl = _pGl;
            _target = _pTarget;
            _vbo = _gl.GenBuffer();
            Bind();
            fixed (T* buf = _data)
                _gl.BufferData(_target, (nuint)(_data.Length * sizeof(T)), buf, BufferUsageARB.DynamicDraw);
        }

        public void Bind()
        {
            _gl.BindBuffer(_target, _vbo);
        }
    }
}
