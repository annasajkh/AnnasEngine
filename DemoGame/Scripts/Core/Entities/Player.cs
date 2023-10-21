using AnnasEngine.Scripts.DataStructures;
using AnnasEngine.Scripts.GameObjects;
using AnnasEngine.Scripts.GameObjects.Components;
using AnnasEngine.Scripts.OpenGL.Shaders;
using AnnasEngine.Scripts.Rendering.Lights;
using AnnasEngine.Scripts.Utils;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace DemoGame.Scripts.Core.Entities
{
    public class Player : GameObject
    {
        public Vector3 Velocity { get; set; }

        private float speed = 10f;
        private float yaw;
        private float pitch;

        private float sensitivity = 0.4f;

        public Camera3D Camera { get; }

        public Player(Transform transform, Vector2 cameraSize, Shader shader)
            : base(transform)
        {
            Camera = new Camera3D(size: cameraSize,
                                  fov: 45,
                                  near: 0.1f,
                                  far: 10000f);

            AddComponent(Camera);
            AddComponent(new SpotLight(shader, shaderIndex: 0));
        }

        public void GetInput(KeyboardState keyboardState, MouseState mouseState, float delta)
        {
            float deltaX = mouseState.Position.X - mouseState.PreviousPosition.X;
            float deltaY = mouseState.Position.Y - mouseState.PreviousPosition.Y;

            yaw -= deltaX * sensitivity;
            pitch += deltaY * sensitivity;

            if (pitch > 89.0f)
            {
                pitch = 89.0f;
            }
            else if (pitch < -89.0f)
            {
                pitch = -89.0f;
            }

            Transform.rotation = QuaternionExtensions.Euler(pitch, yaw, 0);

            Vector3 dir = new Vector3();

            if (keyboardState.IsKeyDown(Keys.W))
            {
                dir += Camera.Front;
            }
            else if (keyboardState.IsKeyDown(Keys.S))
            {
                dir += Camera.Back;
            }

            if (keyboardState.IsKeyDown(Keys.A))
            {
                dir += Camera.Left;
            }
            else if (keyboardState.IsKeyDown(Keys.D))
            {
                dir += Camera.Right;
            }

            if (keyboardState.IsKeyDown(Keys.Space))
            {
                dir += Vector3.UnitY;
            }
            else if (keyboardState.IsKeyDown(Keys.LeftShift))
            {
                dir -= Vector3.UnitY;
            }

            dir.Normalize();

            Velocity = dir * speed;

            if (!(keyboardState.IsKeyDown(Keys.W) ||
                  keyboardState.IsKeyDown(Keys.A) ||
                  keyboardState.IsKeyDown(Keys.S) ||
                  keyboardState.IsKeyDown(Keys.D) ||
                  keyboardState.IsKeyDown(Keys.Space) ||
                  keyboardState.IsKeyDown(Keys.LeftShift)))
            {
                Velocity = Vector3.Zero;
            }
        }


        public void Update(KeyboardState keyboardState, MouseState mouseState, float delta)
        {
            GetInput(keyboardState, mouseState, delta);

            Transform.position += Velocity * delta;

            Update();
        }
    }
}
