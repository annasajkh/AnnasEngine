using AnnasEngine.Scripts.DataStructures.Containers;
using AnnasEngine.Scripts.DataStructures.Vertex.Components;
using OpenTK.Mathematics;
using System.Text;

namespace AnnasEngine.Scripts.DataStructures.Vertex
{
    // Component based vertex so you can build your own custom Vertex
    public class Vertex : ContainerSet
    {
        public uint AllAttributeSize { get; private set; }

        private static List<float> vertices = new List<float>();

        private static StringBuilder stringBuilder = new StringBuilder();
        public Vertex()
        {
            OnComponentAdded += VertexComponentAdded;
            OnComponentRemoved += VertexComponentRemoved;
            OnComponentReplaced += VertexComponentReplaced;
        }

        public static Vertex CreateDefaultVertexEntity(Vector3 position, Color4 color, Vector3 normal, Vector2 textureCoordinate)
        {
            Vertex vertexEntity = new Vertex();

            vertexEntity.AddComponent(new VertexPositionComponent(position));
            vertexEntity.AddComponent(new VertexColorComponent(color));
            vertexEntity.AddComponent(new VertexNormalComponent(normal));
            vertexEntity.AddComponent(new VertexTextureCoordinateComponent(textureCoordinate));

            return vertexEntity;
        }

        private void VertexComponentAdded(ContainerSet sender, IComponent component)
        {
            AllAttributeSize += ((VertexComponent)component).AttributeSize;
        }

        private void VertexComponentRemoved(ContainerSet sender, IComponent component)
        {
            AllAttributeSize -= ((VertexComponent)component).AttributeSize;
        }

        private void VertexComponentReplaced(ContainerSet sender, IComponent oldComponent, IComponent newComponent)
        {
            AllAttributeSize = 0;

            foreach (VertexComponent component in GetAllComponents())
            {
                AllAttributeSize += component.AttributeSize;
            }
        }

        // build the vertex
        public List<float> Build()
        {
            if (!(vertices.Count == 0))
            {
                vertices.Clear();
            }

            vertices.Capacity = (int)AllAttributeSize;

            foreach (VertexComponent component in GetAllComponents())
            {
                switch (component)
                {
                    case VertexPositionComponent:
                        var vertexPositionComponent = (VertexPositionComponent)component;

                        vertices.Add(vertexPositionComponent.value.X);
                        vertices.Add(vertexPositionComponent.value.Y);
                        vertices.Add(vertexPositionComponent.value.Z);
                        break;

                    case VertexColorComponent:
                        var vertexColorComponent = (VertexColorComponent)component;

                        vertices.Add(vertexColorComponent.value.R);
                        vertices.Add(vertexColorComponent.value.G);
                        vertices.Add(vertexColorComponent.value.B);
                        vertices.Add(vertexColorComponent.value.A);
                        break;

                    case VertexNormalComponent:
                        var vertexNormalComponent = (VertexNormalComponent)component;

                        vertices.Add(vertexNormalComponent.value.X);
                        vertices.Add(vertexNormalComponent.value.Y);
                        vertices.Add(vertexNormalComponent.value.Z);
                        break;

                    case VertexTextureCoordinateComponent:
                        var vertexTextureCoordinateComponent = (VertexTextureCoordinateComponent)component;

                        vertices.Add(vertexTextureCoordinateComponent.value.X);
                        vertices.Add(vertexTextureCoordinateComponent.value.Y);
                        break;
                }
            }

            return vertices;

        }


        public override string ToString()
        {
            stringBuilder.Clear();

            foreach (VertexComponent component in GetAllComponents())
            {
                switch (component)
                {
                    case VertexPositionComponent:
                        var vertexPositionComponent = (VertexPositionComponent)component;

                        stringBuilder.Append("Position: { ");

                        stringBuilder.Append($"X: {vertexPositionComponent.value.X}, ");
                        stringBuilder.Append($"Y: {vertexPositionComponent.value.Y}, ");
                        stringBuilder.Append($"Z: {vertexPositionComponent.value.Z} ");

                        stringBuilder.Append("}\n");
                        break;

                    case VertexColorComponent:
                        var vertexColorComponent = (VertexColorComponent)component;

                        stringBuilder.Append("Color: { ");

                        stringBuilder.Append($"R: {vertexColorComponent.value.R}, ");
                        stringBuilder.Append($"G: {vertexColorComponent.value.G}, ");
                        stringBuilder.Append($"B: {vertexColorComponent.value.B}, ");
                        stringBuilder.Append($"A: {vertexColorComponent.value.A} ");

                        stringBuilder.Append("}\n");
                        break;

                    case VertexNormalComponent:
                        var vertexNormalComponent = (VertexNormalComponent)component;

                        stringBuilder.Append("Normal: { ");

                        stringBuilder.Append($"X: {vertexNormalComponent.value.X}, ");
                        stringBuilder.Append($"Y: {vertexNormalComponent.value.Y}, ");
                        stringBuilder.Append($"Z: {vertexNormalComponent.value.Z} ");

                        stringBuilder.Append("}\n");
                        break;

                    case VertexTextureCoordinateComponent:
                        var vertexTextureCoordinateComponent = (VertexTextureCoordinateComponent)component;

                        stringBuilder.Append("TextureCoordinate: { ");

                        stringBuilder.Append($"X: {vertexTextureCoordinateComponent.value.X}, ");
                        stringBuilder.Append($"Y: {vertexTextureCoordinateComponent.value.Y} ");

                        stringBuilder.Append("}\n");
                        break;
                }
            }

            return stringBuilder.ToString().TrimEnd();
        }
    }
}
