namespace AnnasEngine.Scripts.Rendering.OpenGL.OpenGLObjects
{
    // BufferObject are opengl objects that store an array of unformatted memory allocated by the opengl context (AKA the GPU)
    public abstract class BufferObject<T> : OpenGLObject
    {
        public abstract void Data(T[] bufferData);
    }
}
