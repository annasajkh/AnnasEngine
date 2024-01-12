using AnnasEngine.Scripts.Utils.Exceptions.Container;

namespace AnnasEngine.Scripts.DataStructures.Containers;

public class ContainerSet<C> where C : IComponent
{
    public delegate void ComponentChangedEvent(ContainerSet<C> sender, C component);
    public delegate void ComponentReplacedEvent(ContainerSet<C> sender, C oldComponent, C newComponent);

    public event ComponentChangedEvent? OnComponentAdded;
    public event ComponentChangedEvent? OnComponentRemoved;
    public event ComponentReplacedEvent? OnComponentReplaced;

    private Dictionary<Type, C> Components { get; } = new Dictionary<Type, C>();

    public void AddComponent(C component)
    {
        if (Components.ContainsKey(component.GetType()))
        {
            throw new AlreadyContainsComponentException($"ContainerSet already contains {component.GetType()} component");
        }

        Components.Add(component.GetType(), component);

        OnComponentAdded?.Invoke(this, component);
    }

    public void RemoveComponent<T>() where T : C
    {
        CheckContains<T>();

        C component = Components[typeof(T)];

        Components.Remove(typeof(T));

        OnComponentRemoved?.Invoke(this, component);
    }

    public void ReplaceComponent<T>(C component) where T : C
    {
        CheckContains<T>();

        C oldComponent = Components[typeof(T)];

        Components[typeof(T)] = component;

        OnComponentReplaced?.Invoke(this, oldComponent, component);
    }

    public T? GetComponent<T>() where T : C
    {
        if (!Components.ContainsKey(typeof(T)))
        {
            return default;
        }

        return (T)Components[typeof(T)];
    }

    public Dictionary<Type, C>.ValueCollection GetAllComponents()
    {
        return Components.Values;
    }

    public bool Contains<T>() where T : C
    {
        return Components.ContainsKey(typeof(T));
    }

    private void CheckContains<T>() where T : C
    {
        if (!Contains<T>())
        {
            throw new ComponentNotFoundException($"ContainerSet doesn't contains {typeof(T)} component");
        }
    }

    /// <summary>
    /// invoke a specific method in all of the components
    /// </summary>
    public void InvokeAll(string methodName, object[] parameters)
    {
        foreach (var component in Components)
        {
            Invoke(component.Value, methodName, parameters);
        }
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
