using AnnasEngine.Scripts.DataStructures.Containers;
using AnnasEngine.Scripts.GameObjects.Components;
using AnnasEngine.Scripts.Physics.PhysicsObjects;
using AnnasEngine.Scripts.Physics.PhysicsShapes;
using AnnasEngine.Scripts.Rendering;
using MagicPhysX;
using MagicPhysX.Toolkit;
using OpenTK.Mathematics;
using System.Reflection;
using Transform = AnnasEngine.Scripts.DataStructures.Transform;

namespace AnnasEngine.Scripts.GameObjects
{
    public class GameObject : ContainerSet
    {

        public Transform Transform { get; set; }

        public GameObject(Transform transform)
        {
            Transform = transform;

            OnComponentAdded += GameObjectComponentAdded;
            OnComponentRemoved += GameObjectComponentRemoved;
            OnComponentReplaced += GameObjectComponentReplaced;
        }

        public void Update()
        {
            InvokeAll("Update", new object[] { });
        }

        // TODO: Implement all shapes

        public static unsafe GameObject CreateDynamicBox(Transform transform, float density, PhysicsScene physicsScene, PxMaterial* pxMaterial)
        {
            GameObject dynamicBox = new GameObject(transform);

            dynamicBox.AddComponent(new Model(MeshInstance.Cube));
            dynamicBox.AddComponent(new DynamicPhysicsObject(transform, density, new BoxShape(Vector3.One / 2), physicsScene, pxMaterial));

            return dynamicBox;
        }

        public static unsafe GameObject CreateStaticBox(Transform transform, PhysicsScene physicsScene, PxMaterial* pxMaterial)
        {
            GameObject staticBox = new GameObject(transform);

            staticBox.AddComponent(new Model(MeshInstance.Cube));
            staticBox.AddComponent(new StaticPhysicsObject(transform, new BoxShape(Vector3.One / 2), physicsScene, pxMaterial));

            return staticBox;
        }

        public static unsafe GameObject CreateKinematicBox(Transform transform, float density, PhysicsScene physicsScene, PxMaterial* pxMaterial)
        {
            GameObject KinematicBox = new GameObject(transform);

            KinematicBox.AddComponent(new Model(MeshInstance.Cube));
            KinematicBox.AddComponent(new KinematicPhysicsObject(transform, density, new BoxShape(Vector3.One / 2), physicsScene, pxMaterial));

            return KinematicBox;
        }

        /// <summary>
        /// this will set the private field of a component called "parent"
        /// if it doesn't exist it will throw an exception, any component of a GameObject should inherite from GameObjectComponent not IComponent
        /// </summary>
        private void SetComponentParent(IComponent component, GameObject? gameObject)
        {
            FieldInfo? fieldInfo = typeof(GameObjectComponent).GetField("parent", BindingFlags.NonPublic | BindingFlags.Instance);

            if (fieldInfo == null)
            {
                throw new Exception("Error: parent field not found, only component that inherite from GameObjectComponent should be added to GameObject");
            }

            fieldInfo.SetValue(component, gameObject);
        }

        private void GameObjectComponentAdded(ContainerSet sender, IComponent component)
        {
            SetComponentParent(component, this);
        }

        private void GameObjectComponentRemoved(ContainerSet sender, IComponent component)
        {
            SetComponentParent(component, null);
        }

        private void GameObjectComponentReplaced(ContainerSet sender, IComponent oldComponent, IComponent newComponent)
        {
            PropertyInfo? propertyInfo = typeof(GameObjectComponent).GetProperty("parent", BindingFlags.NonPublic | BindingFlags.Instance);

            if (propertyInfo == null)
            {
                throw new Exception("Error: parent field not found, only component that inherite from GameObjectComponent should be added to GameObject");
            }

            propertyInfo.SetValue(oldComponent, null);
            propertyInfo.SetValue(newComponent, this);
        }
    }
}