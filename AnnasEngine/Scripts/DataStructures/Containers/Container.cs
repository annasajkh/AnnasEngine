using AnnasEngine.Scripts.Utils.Exceptions.Container;

namespace AnnasEngine.Scripts.DataStructures.Containers;

public class Container<C> where C : IComponent
{
    private Dictionary<int, C> Components { get; } = new();

    public delegate void ComponentChangedEvent(Container<C> sender, C component);
    public delegate void ComponentReplacedEvent(Container<C> sender, C oldComponent, C newComponent);

    public event ComponentChangedEvent? OnComponentAdded;
    public event ComponentChangedEvent? OnComponentRemoved;
    public event ComponentReplacedEvent? OnComponentReplaced;

    /// <summary>
    /// Add a component to the container returns key of the component
    /// </summary>
    public int AddComponent(C component)
    {

        int key = component.GetHashCode();

        if (Components.ContainsKey(key))
        {
            throw new AlreadyContainsComponentException($"Container already contains a component with hashcode {key}");
        }

        Components.Add(key, component);

        OnComponentAdded?.Invoke(this, component);

        return key;
    }

    public void RemoveComponent(C component)
    {
        int key = component.GetHashCode();

        if (!Components.ContainsKey(key))
        {
            throw new ComponentNotFoundException($"Container doesn't contain a component with hashcode {key}");
        }

        Components.Remove(key);

        OnComponentRemoved?.Invoke(this, component);
    }

    public void RemoveComponent(int key)
    {
        if (!Components.ContainsKey(key))
        {
            throw new ComponentNotFoundException($"Container doesn't contain a component with hashcode {key}");
        }

        OnComponentRemoved?.Invoke(this, Components[key]);

        Components.Remove(key);
    }

    public void RemoveAllComponentOfType<T>() where T : C
    {
        List<int> keysToRemove = new List<int>();

        foreach (var component in Components)
        {
            if (component.Value is T)
            {
                keysToRemove.Add(component.Key);
                OnComponentRemoved?.Invoke(this, component.Value);
            }
        }

        if (keysToRemove.Count == 0)
        {
            throw new ComponentNotFoundException($"Container doesn't contain any components of type {typeof(T)}");
        }

        foreach (var key in keysToRemove)
        {
            Components.Remove(key);
        }
    }

    public void RemoveComponents(List<int> keys)
    {
        foreach (var key in keys)
        {
            RemoveComponent(key);
        }
    }

    public void RemoveComponents(List<C> components)
    {
        foreach (var component in components)
        {
            RemoveComponent(component);
        }
    }

    public void ReplaceComponent(C oldComponent, C newComponent)
    {
        int oldKey = oldComponent.GetHashCode();
        int newKey = newComponent.GetHashCode();

        if (!Components.ContainsKey(oldKey))
        {
            throw new ComponentNotFoundException($"Container doesn't contain a component with hashcode {oldKey}");
        }

        Components.Remove(oldKey);

        Components.Add(newKey, newComponent);

        OnComponentReplaced?.Invoke(this, oldComponent, newComponent);
    }

    public C? GetComponent(int hashCode)
    {
        if (!Components.ContainsKey(hashCode))
        {
            return default;
        }

        return Components[hashCode];
    }

    public C? GetFirstComponentOfType<T>() where T : C
    {
        foreach (var component in Components.Values)
        {
            if (component is T)
            {
                return component;
            }
        }

        return default;
    }

    public List<C> GetAllComponentOfType<T>() where T : C
    {
        List<C> componentsOfType = new List<C>();

        foreach (var component in Components.Values)
        {
            if (component is T)
            {
                componentsOfType.Add(component);
            }
        }

        if (componentsOfType.Count == 0)
        {
            throw new ComponentNotFoundException($"Container doesn't contain any components of type {typeof(T)}");
        }

        return componentsOfType;
    }

    public Dictionary<int, C>.ValueCollection GetAllComponents()
    {
        return Components.Values;
    }


    /// <summary>
    /// sets a specific property for all components, if the new value is assignable to the property type.
    /// </summary>
    public void SetAllFields(string propertyName, object newValue)
    {
        foreach (var component in Components)
        {
            SetField(component.Value, propertyName, newValue);
        }
    }

    /// <summary>
    /// set a specific property for a component of this container
    /// </summary>
    public object? GetField(C component, string propertyName)
    {
        var property = component.GetType().GetProperty(propertyName);

        if (property == null)
        {
            return null;
        }


        return property.GetValue(component);
    }

    /// <summary>
    /// set specific property for a component of this container
    /// </summary>
    public void SetField(C component, string propertyName, object newValue)
    {
        var property = component.GetType().GetProperty(propertyName);

        if (property == null)
        {
            return;
        }

        if (!property.PropertyType.IsAssignableFrom(newValue.GetType()))
        {
            return;
        }

        property.SetValue(component, newValue);
    }


    /// <summary>
    /// invoke a specific method for a component of this container
    /// </summary>
    public void Invoke(C component, string methodName, object[] parameters)
    {
        var method = component.GetType().GetMethod(methodName);

        if (method == null)
        {
            return;
        }

        var parametersInfo = method.GetParameters();

        // Check if the number of parameters matches
        if (parametersInfo.Length != parameters.Length)
        {
            return;
        }

        // Check if each parameter type matches
        bool typesMatch = true;

        for (int i = 0; i < parameters.Length; i++)
        {
            if (parametersInfo[i].ParameterType != parameters[i].GetType())
            {
                typesMatch = false;
                break;
            }
        }

        if (!typesMatch)
        {
            return;
        }

        method.Invoke(component, parameters);
    }
}
