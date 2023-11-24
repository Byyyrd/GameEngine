using System.Drawing;
using Silk.NET.Windowing;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace GameEngine.Graphics
{
    internal class OpenGl
    {
        private static IWindow? _window;
        private static GL? _gl;
        private static uint _program, _primitiveProgram;
        private static readonly Dictionary<Key, KeyHandler> _keys = new();
        public static readonly float WINDOW_WIDTH = 800;
        public static readonly float WINDOW_HEIGTH = 800;


        private static PrimitiveRenderer? _triangleRenderer;
        private static PrimitiveRenderer? _pointRenderer;
        private static PrimitiveRenderer? _lineRenderer;

        public static Action? Load;
        public static Action<float>? Render;
        public static Action<float>? Update;

        public static void Start()
        {
            WindowOptions options = WindowOptions.Default with
            {
                Size = new Vector2D<int>((int)WINDOW_WIDTH, (int)WINDOW_HEIGTH),
                Title = "GameEngine"
            };
            _window = Window.Create(options);
            _window.Load += OnLoad;
            _window.Update += OnUpdate;
            _window.Render += OnRender;
            _window.Run();
        }
        private unsafe static void OnLoad()
        {
            _gl = _window.CreateOpenGL();
            _gl.ClearColor(Color.CornflowerBlue);

            _program = _gl.CreateProgram();

            TexturedShader.Use(_gl, _program);

            _primitiveProgram = _gl.CreateProgram();
            PrimitiveShader.Use(_gl, _primitiveProgram);

            _triangleRenderer = new(_gl, 6 * sizeof(float), PrimitiveType.Triangles);
            _pointRenderer = new(_gl, 6 * sizeof(float), PrimitiveType.Points);
            _lineRenderer = new(_gl, 6 * sizeof(float), PrimitiveType.Lines);
            SetupInputHandeling();

            Load?.Invoke();

            _gl.BindVertexArray(0);
            _gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
            _gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, 0);

        }

        private static void OnUpdate(double deltaTime)
        {
            foreach (KeyHandler key in _keys.Values)
            {
                if (key.IsDown)
                    key.KeyAction?.Invoke(deltaTime);
            }
            Update?.Invoke((float)deltaTime);
        }

        private unsafe static void OnRender(double deltaTime)
        {
            _gl?.Clear(ClearBufferMask.ColorBufferBit);
            _gl?.UseProgram(_primitiveProgram);

            Render?.Invoke((float)deltaTime);

            

            _triangleRenderer?.Render();
            _lineRenderer?.Render();
            _pointRenderer?.Render();

        }

        public static void DrawPoint(Vector2D<float> pos, Vector3D<float> color)
        {
            pos = WindowPosToScreenPos(pos);

            uint[] indices = new uint[] {
                0u
            };
            float[] vertices = new float[]{
                pos.X,pos.Y,0f,color.X,color.Y,color.Z,
            };
        }
        public static void DrawLine(Vector2D<float> pos1, Vector2D<float> pos2, Vector3D<float> color)
        {
            pos1 = WindowPosToScreenPos(pos1);
            pos2 = WindowPosToScreenPos(pos2);
            uint[] indices = new uint[] {
                0u , 1u
            };
            float[] vertices = new float[]{
                pos1.X,pos1.Y,0f,color.X,color.Y,color.Z,
                pos2.X,pos2.Y,0f,color.X,color.Y,color.Z,
            };
        }

        public static void DrawRectangle(Vector2D<float> pos, float width, float heigth, Vector3D<float> color)
        {
            pos = WindowPosToScreenPos(pos);
            width = ValueToScreenValue(WINDOW_WIDTH, width);
            heigth = ValueToScreenValue(WINDOW_HEIGTH, heigth);
            uint[] indices = new uint[] {
                0u , 3u , 2u,
                2u , 1u , 0u
            };
            float[] vertices = new float[]{
                pos.X        ,pos.Y         ,0f,color.X,color.Y,color.Z,
                pos.X        ,pos.Y - heigth,0f,color.X,color.Y,color.Z,
                pos.X + width,pos.Y - heigth,0f,color.X,color.Y,color.Z,
                pos.X + width,pos.Y         ,0f,color.X,color.Y,color.Z,
            };
            _triangleRenderer?.AddElement(indices, vertices);
        }




        private static Vector2D<float> WindowPosToScreenPos(Vector2D<float> pos)
        {
            float x = Lerp(-1, 1, Normalize(0, WINDOW_WIDTH, pos.X));
            float y = Lerp(-1, 1, Normalize(0, WINDOW_HEIGTH, pos.Y));
            return new Vector2D<float>(x, -y);
        }
        private static float ValueToScreenValue(float max, float value)
        {
            return Lerp(0, 2, Normalize(0, max, value));
        }

        public static float Lerp(float min, float max, float value)
        {
            return min + (max - min) * value;
        }
        private static float Normalize(float min, float max, float value)
        {
            return value / max - min / max;
        }











        private static void SetupKeyEvents()
        {
            _keys.Add(Key.Escape, new KeyHandler((dt) => { _window?.Close(); }));
        }

        private static void SetupInputHandeling()
        {
            IInputContext? input = _window?.CreateInput();
            for (int i = 0; i < input?.Keyboards.Count; i++)
            {
                input.Keyboards[i].KeyDown += KeyDown;
                input.Keyboards[i].KeyUp += KeyUp;
            }
            SetupKeyEvents();
        }


        private static void KeyDown(IKeyboard keyboard, Key key, int keyCode)
        {
            _keys.TryGetValue(key, out KeyHandler? keyHandler);
            keyHandler?.SetIsDown(true);
        }
        private static void KeyUp(IKeyboard keyboard, Key key, int keyCode)
        {
            _keys.TryGetValue(key, out KeyHandler? keyHandler);
            keyHandler?.SetIsDown(false);
        }
        private class KeyHandler
        {
            public Action<double>? KeyAction { get; set; }
            public bool IsDown { get; set; }
            public KeyHandler() { }
            public KeyHandler(Action<double> _keyAction)
            {
                KeyAction = _keyAction;
            }
            public void SetIsDown(bool value)
            {
                IsDown = value;
            }
        }
    }
}
