using AnnasEngine.Scripts.OpenGL.OpenGLObjects;
using OpenTK.Graphics.OpenGL4;

namespace AnnasEngine.Scripts.OpenGL.BufferObjects
{
    // VertexBufferObject is the thing that manage vertices
    public class VertexBufferObject : BufferObject<float>
    {
        public BufferUsageHint BufferUsageHint { get; set; }

        public VertexBufferObject(BufferUsageHint bufferUsageHint)
        {
            Handle = GL.GenBuffer();

            BufferUsageHint = bufferUsageHint;
        }

        public override void Data(float[] bufferData)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, Handle);
            GL.BufferData(BufferTarget.ArrayBuffer, bufferData.Length * sizeof(float), bufferData, BufferUsageHint);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public override void Bind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, Handle);
        }

        public override void Unbind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public override void Dispose()
        {
            Console.WriteLine($"VertexBufferObject: {Handle} is Unloaded");

            GL.DeleteBuffer(Handle);
            GC.SuppressFinalize(this);
        }
    }
}
