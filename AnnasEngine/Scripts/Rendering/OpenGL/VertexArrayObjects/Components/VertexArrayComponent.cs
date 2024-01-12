using AnnasEngine.Scripts.DataStructures.Containers;

namespace AnnasEngine.Scripts.Rendering.OpenGL.VertexArrayObjects.Components;

public class VertexArrayComponent : IComponent
{
    public uint AttributeSize { get; }

    public VertexArrayComponent(uint attributeSize)
    {
        AttributeSize = attributeSize;
    }
}
