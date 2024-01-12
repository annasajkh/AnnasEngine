using OpenTK.Mathematics;

namespace AnnasEngine.Scripts.DataStructures.Vertex.Components;

public class VertexPositionComponent : VertexComponent
{
    public Vector3 value;

    public VertexPositionComponent(Vector3 value) : base(attributeSize: 3)
    {
        this.value = value;
    }
}
