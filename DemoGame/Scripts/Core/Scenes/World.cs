using AnnasEngine.Scripts.GameObjects;
using AnnasEngine.Scripts.GameObjects.Components;
using AnnasEngine.Scripts.OpenGL.Shaders;
using AnnasEngine.Scripts.OpenGL.VertexArrayObjects;
using AnnasEngine.Scripts.Physics.PhysicsObjects;
using AnnasEngine.Scripts.Rendering;
using AnnasEngine.Scripts.Rendering.Lights;
using DemoGame.Scripts.Core.Entities;
using MagicPhysX;
using MagicPhysX.Toolkit;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Quaternion = OpenTK.Mathematics.Quaternion;
using Scene = AnnasEngine.Scripts.Utils.Scene;
using Timer = AnnasEngine.Scripts.Utils.Timer;
using Transform = AnnasEngine.Scripts.DataStructures.Transform;

namespace DemoGame.Scripts.Core.Scenes
{
    public class World : Scene
    {
        PhysicsSystem PhysicsSystem { get; }
        PhysicsScene PhysicsScene { get; }
        public Renderer Renderer { get; }

        public static float TimeScale { get; set; } = 1;

        private Player player;

        private List<GameObject> cubes = new List<GameObject>();

        private GameObject floor;

        private Timer spawnTimer;
        private Timer physicsUpdate;

        private float time = 45;

        GameObject light;

        public World()
        {
            Renderer = new Renderer(new DefaultShader(), VertexArrayObject.CreateDefaultVertexArrayObject(false));

            Renderer.Shader.SetFloat("material.shininess", 32f);


            Renderer.Shader.SetVector3("lightColor", new Vector3(1, 1, 1));

            // bind uniform sampler2D to a texture unit
            // texture unit is like slot of the texture on the shader it's like layout
            Renderer.Shader.SetInt("material.diffuse", 0);
            Renderer.Shader.SetInt("material.specular", 0);


            // Setup the physics engine
            PhysicsSystem = new PhysicsSystem(enablePvd: false);
            PhysicsScene = PhysicsSystem.CreateScene();

            unsafe
            {
                PxMaterial* pxMaterial = PhysicsSystem.CreateMaterial(0.5f, 0.5f, 0.6f);


                floor = GameObject.CreateStaticBox(new Transform(Vector3.Zero, Quaternion.Identity, new Vector3(200, 1, 200)), PhysicsScene, pxMaterial);

                Transform transformCube = new Transform(position: new Vector3(Game.Random.NextSingle() * 50 - 25, 10, Game.Random.NextSingle() * 50 - 25),
                                                        rotation: Quaternion.FromEulerAngles(MathHelper.DegreesToRadians(45), MathHelper.DegreesToRadians(0), MathHelper.DegreesToRadians(45)),
                                                        scale: Vector3.One);

                light = new GameObject(new Transform(new Vector3(50, 10, 0), Quaternion.Identity, Vector3.One));

                PointLight pointLight = new PointLight(Renderer.Shader, 0);

                light.AddComponent(pointLight);


                // Setup the timers
                spawnTimer = new Timer(0.001f, () =>
                {
                    Console.WriteLine($"Box Count: {cubes.Count}");

                    Transform transform = new Transform(position: new Vector3(Game.Random.NextSingle() * 50 - 25, 10, Game.Random.NextSingle() * 50 - 25),
                                                        rotation: Quaternion.FromEulerAngles(Game.Random.NextSingle() * MathF.Tau, Game.Random.NextSingle() * MathF.Tau, Game.Random.NextSingle() * MathF.Tau),
                                                        scale: Vector3.One);

                    GameObject cube = GameObject.CreateDynamicBox(transform, 10, PhysicsScene, pxMaterial);

                    cubes.Add(cube);
                });
            }

            spawnTimer.Start();

            physicsUpdate = new Timer(1f / 60f, PhysicsUpdate);
            physicsUpdate.Start();



            Transform transform = new Transform(position: new Vector3(0, 5, 0),
                                                rotation: Quaternion.Identity,
                                                scale: Vector3.One);

            player = new Player(transform, new Vector2(Game.WindowWidth, Game.WindowHeight), Renderer.Shader);
        }

        public override void Load()
        {

        }

        public override void Unload()
        {

        }

        public override void WindowResized()
        {
            player.Camera.Size = (Game.WindowWidth, Game.WindowHeight);
        }

        public void GetInput(KeyboardState keyboardState, MouseState mouseState, float delta)
        {
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                time += delta * 100;
            }
            else if (keyboardState.IsKeyDown(Keys.Down))
            {
                time -= delta * 100;
            }

            if (keyboardState.IsKeyPressed(Keys.Space))
            {
                foreach (var item in cubes)
                {
                    if (item.Contains<PhysicsObject>())
                    {
                        item.RemoveComponent<PhysicsObject>();
                    }

                }
            }

            if (keyboardState.IsKeyDown(Keys.Tab))
            {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            }
            else
            {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            }
        }

        public void PhysicsUpdate()
        {
            floor.GetComponent<StaticPhysicsObject>()?.BeforePhysicsUpdate();

            for (int i = 0; i < cubes.Count; i++)
            {
                cubes[i].GetComponent<DynamicPhysicsObject>()?.BeforePhysicsUpdate();
            }

            PhysicsScene.Update();

            for (int i = 0; i < cubes.Count; i++)
            {
                cubes[i].GetComponent<DynamicPhysicsObject>()?.AfterPhysicsUpdate();
            }

            floor.GetComponent<StaticPhysicsObject>()?.AfterPhysicsUpdate();
        }

        public override void Update(KeyboardState keyboardState, MouseState mouseState, float delta)
        {
            GetInput(keyboardState, mouseState, delta);

            player.Update(keyboardState, mouseState, delta);

            spawnTimer.Step(delta);
            physicsUpdate.Step(delta);

            //directionalLight.Direction = new Vector3((float)MathHelper.Cos(MathHelper.DegreesToRadians(time)), (float)MathHelper.Sin(MathHelper.DegreesToRadians(time)), 0);

            //directionalLight.Update();


            if (time > 360)
            {
                time = 0;
            }
            else if (time < 0)
            {
                time = 360;
            }

            for (int i = 0; i < cubes.Count; i++)
            {
                cubes[i].Update();
            }

            light.Update();
        }

        public override void Render()
        {
            Renderer.Begin(player.Camera);

            Game.ResourceManager.Textures["annasvirtual"].Bind();

            for (int i = 0; i < cubes.Count; i++)
            {
                Model? cubeModel = cubes[i].GetComponent<Model>();

                if (cubeModel != null)
                {
                    Renderer.Render(cubeModel);
                }
            }

            Model? floorModel = floor.GetComponent<Model>();

            if (floorModel != null)
            {
                Renderer.Render(floorModel);
            }

            Renderer.End();
        }
    }
}
