using AnnasEngine.Scripts.DataStructures.Containers;
using AnnasEngine.Scripts.Utils.Exceptions.GameObject;
using System.Reflection;

namespace AnnasEngine.Scripts.DataStructures.GameObjects;

public class GameObject : ContainerSet<GameObjectComponent>
{
    public GameObject()
    {
        OnComponentAdded += GameObjectComponentAdded;
        OnComponentRemoved += GameObjectComponentRemoved;
        OnComponentReplaced += GameObjectComponentReplaced;
    }

    public void Update()
    {
        InvokeAll("Update", new object[] { });
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
            throw new ParentNotFoundException("parent field not found, only component that inherite from GameObjectComponent should be added to GameObject");
        }

        fieldInfo.SetValue(component, gameObject);
    }

    private void GameObjectComponentAdded(ContainerSet<GameObjectComponent> sender, IComponent component)
    {
        SetComponentParent(component, this);
    }

    private void GameObjectComponentRemoved(ContainerSet<GameObjectComponent> sender, IComponent component)
    {
        SetComponentParent(component, null);
    }

    private void GameObjectComponentReplaced(ContainerSet<GameObjectComponent> sender, IComponent oldComponent, IComponent newComponent)
    {
        PropertyInfo propertyInfo = typeof(GameObjectComponent).GetProperty("parent", BindingFlags.NonPublic | BindingFlags.Instance)!;

        propertyInfo.SetValue(oldComponent, null);
        propertyInfo.SetValue(newComponent, this);
    }
}
