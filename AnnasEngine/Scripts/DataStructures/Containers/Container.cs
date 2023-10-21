namespace AnnasEngine.Scripts.DataStructures.Containers
{
    public class Container
    {
        private Dictionary<int, IComponent> Components { get; } = new Dictionary<int, IComponent>();

        public delegate void ComponentChangedEvent(Container sender, IComponent component);
        public delegate void ComponentReplacedEvent(Container sender, IComponent oldComponent, IComponent newComponent);

        public event ComponentChangedEvent? OnComponentAdded;
        public event ComponentChangedEvent? OnComponentRemoved;
        public event ComponentReplacedEvent? OnComponentReplaced;

        /// <summary>
        /// Add a component to the container returns key of the component
        /// </summary>
        public int AddComponent(IComponent component)
        {
            int key = component.GetHashCode();

            if (Components.ContainsKey(key))
            {
                throw new Exception($"Error: Container already contains a component with hashcode {key}");
            }

            Components.Add(key, component);

            OnComponentAdded?.Invoke(this, component);

            return key;
        }

        public void RemoveComponent(IComponent component)
        {
            int key = component.GetHashCode();

            if (!Components.ContainsKey(key))
            {
                throw new Exception($"Error: Container doesn't contain a component with hashcode {key}");
            }

            Components.Remove(key);

            OnComponentRemoved?.Invoke(this, component);
        }

        public void RemoveComponent(int key)
        {
            if (!Components.ContainsKey(key))
            {
                throw new Exception($"Error: Container doesn't contain a component with hashcode {key}");
            }

            OnComponentRemoved?.Invoke(this, Components[key]);

            Components.Remove(key);
        }

        public void RemoveAllComponentOfType<T>() where T : IComponent
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
                throw new Exception($"Error: Container doesn't contain any components of type {typeof(T)}");
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

        public void RemoveComponents(List<IComponent> components)
        {
            foreach (var component in components)
            {
                RemoveComponent(component);
            }
        }

        public void ReplaceComponent(IComponent oldComponent, IComponent newComponent)
        {
            int oldKey = oldComponent.GetHashCode();
            int newKey = newComponent.GetHashCode();

            if (!Components.ContainsKey(oldKey))
            {
                throw new Exception($"Error: Container doesn't contain a component with hashcode {oldKey}");
            }

            Components.Remove(oldKey);

            Components.Add(newKey, newComponent);

            OnComponentReplaced?.Invoke(this, oldComponent, newComponent);
        }

        public IComponent? GetComponent(int hashCode)
        {
            if (!Components.ContainsKey(hashCode))
            {
                return null;
            }

            return Components[hashCode];
        }

        public IComponent? GetFirstComponentOfType<T>() where T : IComponent
        {
            foreach (var component in Components.Values)
            {
                if (component is T)
                {
                    return component;
                }
            }

            return null;
        }

        public List<IComponent> GetAllComponentOfType<T>() where T : IComponent
        {
            List<IComponent> componentsOfType = new List<IComponent>();

            foreach (var component in Components.Values)
            {
                if (component is T)
                {
                    componentsOfType.Add(component);
                }
            }

            if (componentsOfType.Count == 0)
            {
                throw new Exception($"Error: Container doesn't contain any components of type {typeof(T)}");
            }

            return componentsOfType;
        }

        public Dictionary<int, IComponent>.ValueCollection GetAllComponents()
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
        public object? GetField(IComponent component, string propertyName)
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
        public void SetField(IComponent component, string propertyName, object newValue)
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
        public void Invoke(IComponent component, string methodName, object[] parameters)
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

}
