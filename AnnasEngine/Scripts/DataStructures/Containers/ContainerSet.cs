namespace AnnasEngine.Scripts.DataStructures.Containers
{
    public class ContainerSet
    {
        public delegate void ComponentChangedEvent(ContainerSet sender, IComponent component);
        public delegate void ComponentReplacedEvent(ContainerSet sender, IComponent oldComponent, IComponent newComponent);

        public event ComponentChangedEvent? OnComponentAdded;
        public event ComponentChangedEvent? OnComponentRemoved;
        public event ComponentReplacedEvent? OnComponentReplaced;

        private Dictionary<Type, IComponent> Components { get; } = new Dictionary<Type, IComponent>();

        public void AddComponent(IComponent component)
        {
            if (Components.ContainsKey(component.GetType()))
            {
                throw new Exception($"Error: Entity already contains {component.GetType()} component");
            }

            Components.Add(component.GetType(), component);

            OnComponentAdded?.Invoke(this, component);
        }

        public void RemoveComponent<T>() where T : IComponent
        {
            CheckContains<T>();

            IComponent component = Components[typeof(T)];

            Components.Remove(typeof(T));

            OnComponentRemoved?.Invoke(this, component);
        }

        public void ReplaceComponent<T>(IComponent component) where T : IComponent
        {
            CheckContains<T>();

            IComponent oldComponent = Components[typeof(T)];

            Components[typeof(T)] = component;

            OnComponentReplaced?.Invoke(this, oldComponent, component);
        }

        public T? GetComponent<T>() where T : IComponent
        {
            if (!Components.ContainsKey(typeof(T)))
            {
                return default;
            }

            return (T)Components[typeof(T)];
        }

        public Dictionary<Type, IComponent>.ValueCollection GetAllComponents()
        {
            return Components.Values;
        }

        public bool Contains<T>() where T : IComponent
        {
            return Components.ContainsKey(typeof(T));
        }

        private void CheckContains<T>() where T : IComponent
        {
            if (!Contains<T>())
            {
                throw new Exception($"Error: SetContainer doesn't contains {typeof(T)} component");
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
