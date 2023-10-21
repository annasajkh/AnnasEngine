using AnnasEngine.Scripts.DataStructures.Containers;
using AnnasEngine.Scripts.DataStructures.Vertex.Components;
using AnnasEngine.Scripts.OpenGL.OpenGLObjects;
using AnnasEngine.Scripts.OpenGL.VertexArrayObjects.Components;
using OpenTK.Graphics.OpenGL4;

namespace AnnasEngine.Scripts.OpenGL.VertexArrayObjects
{
    // VertexArrayObject is a specification of how to pass data from VertexBufferObject to the Shader
    // this is also component based so you can build your own custom VertexArrayObject
    public class VertexArrayObject : OpenGLObject
    {
        public Container Container { get; }
        public uint AllAttributeSize { get; protected set; }
        public bool Normalized { get; set; }

        public VertexArrayObject(bool normalized)
        {
            Handle = GL.GenVertexArray();

            Container = new Container();

            Normalized = normalized;

            Container.OnComponentAdded += VertexArrayObjectComponentAdded;
            Container.OnComponentRemoved += VertexArrayObjectComponentRemoved;
            Container.OnComponentReplaced += VertexArrayObjectComponentReplaced;
        }

        public static VertexArrayObject CreateDefaultVertexArrayObject(bool normalized)
        {
            VertexArrayObject vertexArrayObject = new VertexArrayObject(normalized);

            vertexArrayObject.Container.AddComponent(new VertexArrayPositionComponent());
            vertexArrayObject.Container.AddComponent(new VertexArrayColorComponent());
            vertexArrayObject.Container.AddComponent(new VertexArrayNormalComponent());
            vertexArrayObject.Container.AddComponent(new VertexArrayTextureCoordinateComponent());

            return vertexArrayObject;
        }

        private void VertexArrayObjectComponentAdded(Container sender, IComponent component)
        {
            AllAttributeSize += ((VertexArrayComponent)component).AttributeSize;
        }

        private void VertexArrayObjectComponentRemoved(Container sender, IComponent component)
        {
            AllAttributeSize -= ((VertexArrayComponent)component).AttributeSize;
        }

        private void VertexArrayObjectComponentReplaced(Container sender, IComponent oldComponent, IComponent newComponent)
        {
            AllAttributeSize = 0;

            foreach (VertexComponent component in Container.GetAllComponents())
            {
                AllAttributeSize += component.AttributeSize;
            }
        }

        public void ApplyAttributes()
        {
            int offset = 0;
            int index = 0;

            foreach (VertexArrayComponent component in Container.GetAllComponents())
            {
                GL.VertexAttribPointer(index: index,
                                       size: (int)component.AttributeSize,
                                       type: VertexAttribPointerType.Float,
                                       normalized: Normalized,
                                       stride: (int)(AllAttributeSize * sizeof(float)),
                                       offset: offset * sizeof(float));
                GL.EnableVertexAttribArray(index);

                offset += (int)component.AttributeSize;
                index++;
            }
        }

        public override void Bind()
        {
            GL.BindVertexArray(Handle);
        }

        public override void Unbind()
        {
            GL.BindVertexArray(0);
        }

        public override void Dispose()
        {
            Console.WriteLine($"{GetType()}: {Handle} is Unloaded");

            GL.DeleteBuffer(Handle);
            GC.SuppressFinalize(this);
        }
    }
}
