using AnnasEngine.Scripts.GameObjects.Components;
using AnnasEngine.Scripts.OpenGL.Shaders;
using AnnasEngine.Scripts.OpenGL.VertexArrayObjects;
using AnnasEngine.Scripts.Utils;
using OpenTK.Graphics.OpenGL4;

namespace AnnasEngine.Scripts.Rendering
{
    public class Renderer : IDisposable
    {
        public Shader Shader { get; set; }
        public VertexArrayObject VertexArrayObject { get; set; }

        public Renderer(Shader shader, VertexArrayObject vertexArrayObject)
        {
            OpenGLSettings.Init();

            Shader = shader;
            VertexArrayObject = vertexArrayObject;

        }

        public void Begin(Camera3D camera)
        {
            Shader.Bind();
            VertexArrayObject.Bind();

            Shader.SetVector3("uViewPos", camera.GetParent().Transform.position);
            Shader.SetMatrix4("uView", false, camera.ViewMatrix);
            Shader.SetMatrix4("uProjection", false, camera.ProjectionMatrix);
        }

        public void Render(Model model)
        {
            Shader.SetMatrix4("uModel", false, model.ModelMatrix);

            model.Mesh.Bind();

            VertexArrayObject.ApplyAttributes();

            GL.DrawElements(PrimitiveType.Triangles, model.Mesh.Indices.Length * 3, DrawElementsType.UnsignedInt, 0);

            model.Mesh.Unbind();
        }

        public void End()
        {
            Shader.Unbind();
            VertexArrayObject.Unbind();
        }

        public void Dispose()
        {
            Shader.Dispose();
            VertexArrayObject.Dispose();

            GC.SuppressFinalize(this);
        }

        ~Renderer()
        {
            Dispose();
        }
    }
}
