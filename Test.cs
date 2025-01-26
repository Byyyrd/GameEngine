using GameEngine.Graphics;
using GameEngine.Physics;
using Silk.NET.Input;

namespace GameEngine
{
    internal class Test : PhysicsObject2D
    {

        public override void OnDraw(float dt)
        {
            OpenGl.DrawRectangle(new(pos.X + 0, 0), 100, 100, new(0, 1, 0));
            OpenGl.DrawRectangle(new(pos.X + 100, 100), 100, 100, new(0, 1, 0));
            OpenGl.DrawRectangle(new(pos.X + 200, 200), 100, 100, new(0, 1, 0));
            OpenGl.DrawRectangle(new(pos.X + 300, 300), 100, 100, new(0, 1, 0));
        }

        public override void OnLoad()
        {
           
        }

        public override void OnUpdate(float dt)
        {
            if (OpenGl.IsKeyDown(Key.D))
            {
				pos.X += 100 * dt;
			}
		}
    }
}
