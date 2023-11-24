using GameEngine.Graphics;
using GameEngine.Physics;

namespace GameEngine
{
    internal class Test : PhysicsObject2D
    {
        public override void OnDraw(float dt)
        {
            OpenGl.DrawRectangle(new(0, 0), 100, 100, new(0, 1, 0));
            OpenGl.DrawRectangle(new(100, 100), 100, 100, new(0, 1, 0));
            OpenGl.DrawRectangle(new(200, 200), 100, 100, new(0, 1, 0));
            OpenGl.DrawRectangle(new(300, 300), 100, 100, new(0, 1, 0));
        }

        public override void OnLoad()
        {
           
        }

        public override void OnUpdate(float dt)
        {
            
        }
    }
}
