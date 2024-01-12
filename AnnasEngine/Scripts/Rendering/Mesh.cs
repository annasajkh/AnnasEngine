using AnnasEngine.Scripts.Rendering.OpenGL.BufferObjects;
using AnnasEngine.Scripts.Rendering.OpenGL.OpenGLObjects;
using OpenTK.Graphics.OpenGL4;

namespace AnnasEngine.Scripts.Rendering;

// mesh is a collection of buffer objects and will be render according to its indices
public class Mesh : OpenGLObject
{
    public VertexBufferObject VertexBufferObject { get; }
    public ElementBufferObject ElementBufferObject { get; }

    public BufferUsageHint BufferUsageHint { get; set; }

    private float[] vertices;
    private uint[] indices;


    public float[] Vertices
    {
        get
        {
            return vertices;
        }

        set
        {
            vertices = value;
            VertexBufferObject.Data(value);
        }
    }

    public uint[] Indices
    {
        get
        {
            return indices;
        }

        set
        {
            indices = value;
            ElementBufferObject.Data(value);
        }
    }

    public Mesh(BufferUsageHint bufferUsageHint, float[] vertices, uint[] indices)
    {
        this.vertices = vertices;
        this.indices = indices;

        VertexBufferObject = new VertexBufferObject(bufferUsageHint);
        ElementBufferObject = new ElementBufferObject(bufferUsageHint);

        Vertices = vertices;
        Indices = indices;

        BufferUsageHint = bufferUsageHint;
    }

    public override void Bind()
    {
        VertexBufferObject.Bind();
        ElementBufferObject.Bind();
    }

    public override void Unbind()
    {
        VertexBufferObject.Unbind();
        ElementBufferObject.Unbind();
    }

    public override void Dispose()
    {
        VertexBufferObject.Dispose();
        ElementBufferObject.Dispose();

        GC.SuppressFinalize(this);
    }

    ~Mesh()
    {
        Dispose();
    }
}
