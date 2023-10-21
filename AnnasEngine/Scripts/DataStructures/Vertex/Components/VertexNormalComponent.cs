using OpenTK.Mathematics;

namespace AnnasEngine.Scripts.DataStructures.Vertex.Components
{
    public class VertexNormalComponent : VertexComponent
    {
        public Vector3 value;

        public VertexNormalComponent(Vector3 value) : base(attributeSize: 4)
        {
            this.value = value;
        }
    }
}
