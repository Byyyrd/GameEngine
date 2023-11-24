using GameEngine.Graphics;
using GameEngine.Physics;

namespace GameEngine
{
    internal static class MainController
    {
        private static readonly List<ILoadable> _loadables = new();
        private static readonly List<IUpdatable> _updatables = new();
        private static readonly List<IDrawable> _drawables = new();
        public static void Start()
        {
            Test test = new();
            _loadables.Add(test);
            _drawables.Add(test);
            _updatables.Add(test);
            foreach (ILoadable loadable in _loadables)
            {
                OpenGl.Load += loadable.OnLoad;
            }
            foreach (IUpdatable updatable in _updatables)
            {
                OpenGl.Update += updatable.OnUpdate;
            }
            foreach (IDrawable drawable in _drawables)
            {
                OpenGl.Render += drawable.OnDraw;
            }
            OpenGl.Start();
        }
    }
}
