using OpenTK.Mathematics;

namespace AnnasEngine.Scripts.DataStructures.Vertex.Components
{
    public class VertexTextureCoordinateComponent : VertexComponent
    {
        public Vector2 value;

        public VertexTextureCoordinateComponent(Vector2 value) : base(attributeSize: 2)
        {
            this.value = value;
        }
    }
}
