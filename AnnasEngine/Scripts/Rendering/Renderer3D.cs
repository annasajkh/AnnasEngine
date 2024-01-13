using AnnasEngine.Scripts.DataStructures.GameObjects;
using AnnasEngine.Scripts.Rendering.OpenGL.VertexArrayObjects;
using AnnasEngine.Scripts.Utils.Settings;
using OpenTK.Graphics.OpenGL4;

namespace AnnasEngine.Scripts.Rendering;

public class Renderer3D : IDisposable
{
    public Shader Shader { get; set; }
    public VertexArrayObject VertexArrayObject { get; set; }

    public Renderer3D(Shader shader, VertexArrayObject vertexArrayObject)
    {
        OpenGLSettings.Init();

        Shader = shader;
        VertexArrayObject = vertexArrayObject;
    }

    public void Begin(Camera3D camera)
    {
        Shader.Bind();
        VertexArrayObject.Bind();

        GameObject3D cameraParent = (GameObject3D)camera.GetParent();

        Shader.SetVector3("uViewPos", cameraParent.Transform.position);
        Shader.SetMatrix4("uView", false, camera.ViewMatrix);
        Shader.SetMatrix4("uProjection", false, camera.ProjectionMatrix);
    }

    public void Render(Model model)
    {
        Shader.SetMatrix4("uModel", false, model.ModelMatrix);

        for (int i = 0; i < model.Meshes.Count; i++)
        {
            model.Meshes[i].Bind();

            VertexArrayObject.ApplyAttributes();

            GL.DrawElements(PrimitiveType.Triangles, model.Meshes[i].Indices.Length * 3, DrawElementsType.UnsignedInt, 0);

            model.Meshes[i].Unbind();
        }
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

    ~Renderer3D()
    {
        Dispose();
    }
}
