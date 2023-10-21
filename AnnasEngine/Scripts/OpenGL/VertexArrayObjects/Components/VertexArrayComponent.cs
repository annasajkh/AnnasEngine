using AnnasEngine.Scripts.DataStructures.Containers;

namespace AnnasEngine.Scripts.OpenGL.VertexArrayObjects.Components
{
    public class VertexArrayComponent : IComponent
    {
        public uint AttributeSize { get; }

        public VertexArrayComponent(uint attributeSize)
        {
            AttributeSize = attributeSize;
        }
    }
}
