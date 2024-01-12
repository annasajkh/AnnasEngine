using OpenTK.Mathematics;

namespace AnnasEngine.Scripts.DataStructures.Vertex.Components;

public class VertexColorComponent : VertexComponent
{
    public Color4 value;

    public VertexColorComponent(Color4 value) : base(attributeSize: 4)
    {
        this.value = value;
    }
}
