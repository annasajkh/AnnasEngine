using AnnasEngine.Scripts.Utils.Managers;
using DemoGame.Scripts.Core.Scenes;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Drawing;

#pragma warning disable CS8618

namespace DemoGame.Scripts.Core
{
    public class Game : GameWindow
    {
        public static ResourceManager ResourceManager { get; private set; }
        public static SceneManager SceneManager { get; private set; }
        public static Random Random { get; } = new Random();
        public static int WindowWidth { get; private set; }
        public static int WindowHeight { get; private set; }
        public static bool Paused { get; set; }

        public Game(string title, int width, int height) : base(GameWindowSettings.Default,
                        new NativeWindowSettings()
                        {
                            Title = title,
                            Size = (width, height)
                        })
        {
            WindowWidth = width;
            WindowHeight = height;

            CenterWindow();

            ResourceManager = new ResourceManager();

            ResourceManager.AddTexture("annasvirtual", Path.GetFullPath("Assets/Textures/annasvirtual.png"));


            SceneManager = new SceneManager(new World());
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            SceneManager.CurrentScene.Load();
        }

        protected override void OnResize(ResizeEventArgs resizeEventArgs)
        {
            base.OnResize(resizeEventArgs);

            WindowWidth = resizeEventArgs.Width;
            WindowHeight = resizeEventArgs.Height;

            GL.Viewport(0, 0, WindowWidth, WindowHeight);

            SceneManager.CurrentScene.WindowResized();
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            SceneManager.CurrentScene.Unload();
        }

        protected override void OnUpdateFrame(FrameEventArgs frameEventArgs)
        {
            base.OnUpdateFrame(frameEventArgs);

            KeyboardState keyboardState = KeyboardState;


            if (keyboardState.IsKeyPressed(Keys.P))
            {
                Paused = !Paused;
            }

            if (Paused)
            {
                CursorState = CursorState.Normal;
            }
            else
            {
                CursorState = CursorState.Grabbed;


                if (keyboardState.IsKeyDown(Keys.Escape))
                {
                    Close();
                }

                SceneManager.CurrentScene.Update(keyboardState, MouseState, (float)frameEventArgs.Time);
            }
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.ClearColor(Color.CornflowerBlue);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            SceneManager.CurrentScene.Render();

            SwapBuffers();
        }
    }
}
