
using AnnasEngine.Scripts.Physics.PhysicsObjects;
using AnnasEngine.Scripts.Physics.PhysicsShapes;
using AnnasEngine.Scripts.Rendering;
using MagicPhysX;
using MagicPhysX.Toolkit;
using OpenTK.Mathematics;

namespace AnnasEngine.Scripts.DataStructures.GameObjects;

public class GameObject3D : GameObject
{
    public Transform Transform { get; set; }

    public GameObject3D(Transform transform)
    {
        Transform = transform;
    }

    // TODO: Implement all shapes

    public static unsafe GameObject3D CreateDynamicBox(Transform transform, float density, PhysicsScene physicsScene, PxMaterial* pxMaterial)
    {
        GameObject3D dynamicBox = new GameObject3D(transform);

        dynamicBox.AddComponent(new Model(MeshInstance.Cube));
        dynamicBox.AddComponent(new DynamicPhysicsObject(transform, density, new BoxShape(Vector3.One / 2), physicsScene, pxMaterial));

        return dynamicBox;
    }

    public static unsafe GameObject3D CreateStaticBox(Transform transform, PhysicsScene physicsScene, PxMaterial* pxMaterial)
    {
        GameObject3D staticBox = new GameObject3D(transform);

        staticBox.AddComponent(new Model(MeshInstance.Cube));
        staticBox.AddComponent(new StaticPhysicsObject(transform, new BoxShape(Vector3.One / 2), physicsScene, pxMaterial));

        return staticBox;
    }

    public static unsafe GameObject3D CreateKinematicBox(Transform transform, float density, PhysicsScene physicsScene, PxMaterial* pxMaterial)
    {
        GameObject3D KinematicBox = new GameObject3D(transform);

        KinematicBox.AddComponent(new Model(MeshInstance.Cube));
        KinematicBox.AddComponent(new KinematicPhysicsObject(transform, density, new BoxShape(Vector3.One / 2), physicsScene, pxMaterial));

        return KinematicBox;
    }
}
