using AnnasEngine.Scripts.DataStructures.Containers;

namespace AnnasEngine.Scripts.DataStructures.Vertex.Components;

public class VertexComponent : IComponent
{
    public uint AttributeSize { get; }

    public VertexComponent(uint attributeSize)
    {
        AttributeSize = attributeSize;
    }

}
