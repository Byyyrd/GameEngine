using GameEngine.Graphics;
using GameEngine.Physics;
using System.Numerics;

namespace GameEngine
{
    internal abstract class GraphicsObject : IDrawable, IUpdatable, ILoadable
    {
        protected Vector2 pos;
        protected Vector3 rot;
        protected Vector2 size;
        protected float radius;

        public abstract void OnLoad();
        public abstract void OnDraw(float dt);
        public abstract void OnUpdate(float dt);

    }
}
