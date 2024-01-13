using AnnasEngine.Scripts.DataStructures.GameObjects;
using AnnasEngine.Scripts.Physics.PhysicsObjects;
using AnnasEngine.Scripts.Rendering;
using AnnasEngine.Scripts.Rendering.Lights;
using AnnasEngine.Scripts.Rendering.OpenGL.VertexArrayObjects;
using AnnasEngine.Scripts.Rendering.OpenGL.VertexArrayObjects.Components;
using AnnasEngine.Scripts.Utils;
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

namespace DemoGame.Scripts.Core.Scenes;

public class World : Scene
{
    PhysicsSystem PhysicsSystem { get; }
    PhysicsScene PhysicsScene { get; }
    public Renderer3D WorldObjectRenderer { get; }

    public static float TimeScale { get; set; } = 1;

    private Player player;

    private List<GameObject3D> chairs = new List<GameObject3D>();

    private GameObject3D floor;

    private Timer spawnTimer;
    private Timer physicsUpdate;

    private float time = 45;

    private List<Mesh> chairMeshes;

    GameObject3D light;

    public World()
    {

        Shader worldObjectShader = new Shader(vertexShaderPath: Path.GetFullPath("Assets/Shaders/WorldObject/shader.vert"),
                                              geometryShaderPath: Path.GetFullPath("Assets/Shaders/WorldObject/shader.geom"),
                                              fragmentShaderPath: Path.GetFullPath("Assets/Shaders/WorldObject/shader.frag"));

        VertexArrayObject worldObjectVertexArray = new VertexArrayObject(false);

        worldObjectVertexArray.Container.AddComponent(new VertexArrayPositionComponent());
        worldObjectVertexArray.Container.AddComponent(new VertexArrayTextureCoordinateComponent());


        chairMeshes = Helpers.LoadModelFromFile("Assets/Models/chair.obj", BufferUsageHint.StaticDraw, worldObjectVertexArray);

        WorldObjectRenderer = new Renderer3D(worldObjectShader, worldObjectVertexArray);


        WorldObjectRenderer.Shader.SetFloat("material.shininess", 32f);
        WorldObjectRenderer.Shader.SetVector3("lightColor", new Vector3(1, 1, 1));

        // Bind uniform sampler2D to a texture unit
        // Texture unit is like slot of the texture on the shader it's like layout
        WorldObjectRenderer.Shader.SetInt("material.diffuse", 0);
        WorldObjectRenderer.Shader.SetInt("material.specular", 0);


        // Setup the physics engine
        PhysicsSystem = new PhysicsSystem(enablePvd: false);
        PhysicsScene = PhysicsSystem.CreateScene();

        unsafe
        {
            PxMaterial* pxMaterial = PhysicsSystem.CreateMaterial(0.5f, 0.5f, 0.6f);

            floor = GameObject3D.CreateStaticBox(new Transform(Vector3.Zero, Quaternion.Identity, new Vector3(200, 1, 200)), PhysicsScene, pxMaterial);

            // Setup the timers
            spawnTimer = new Timer(0.1f, () =>
            {
                Transform transform = new Transform(position: new Vector3(Game.Random.NextSingle() * 50 - 25, 10, Game.Random.NextSingle() * 50 - 25),
                                                    rotation: Quaternion.FromEulerAngles(Game.Random.NextSingle() * MathF.Tau, Game.Random.NextSingle() * MathF.Tau, Game.Random.NextSingle() * MathF.Tau),
                                                    scale: Vector3.One);

                chairs.Add(GameObject3D.CreateDynamicBox(transform, 10, PhysicsScene, pxMaterial));

                Console.WriteLine($"Count: {chairs.Count}");
            });

            spawnTimer.Start();
        }

        light = new GameObject3D(new Transform(new Vector3(50, 10, 0), Quaternion.Identity, Vector3.One));
        light.AddComponent(new PointLight(WorldObjectRenderer.Shader, 0));


        physicsUpdate = new Timer(1f / 60f, PhysicsUpdate);
        physicsUpdate.Start();



        Transform transform = new Transform(position: new Vector3(0, 5, 0),
                                            rotation: Quaternion.Identity,
                                            scale: Vector3.One);

        player = new Player(transform, new Vector2(Game.WindowWidth, Game.WindowHeight), WorldObjectRenderer.Shader);
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
            foreach (var item in chairs)
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

        for (int i = 0; i < chairs.Count; i++)
        {
            chairs[i].GetComponent<DynamicPhysicsObject>()?.BeforePhysicsUpdate();
        }

        PhysicsScene.Update();

        for (int i = 0; i < chairs.Count; i++)
        {
            chairs[i].GetComponent<DynamicPhysicsObject>()?.AfterPhysicsUpdate();
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

        for (int i = 0; i < chairs.Count; i++)
        {
            chairs[i].Update();
        }

        light.Update();
    }

    public override void Render()
    {
        WorldObjectRenderer.Begin(player.Camera);

        Game.ResourceManager.Textures["annasvirtual"].Bind();

        for (int i = 0; i < chairs.Count; i++)
        {
            Model? chairModel = chairs[i].GetComponent<Model>();

            if (chairModel != null)
            {
                WorldObjectRenderer.Render(chairModel);
            }
        }

        Model? floorModel = floor.GetComponent<Model>();

        if (floorModel != null)
        {
            WorldObjectRenderer.Render(floorModel);
        }

        WorldObjectRenderer.End();
    }
}
