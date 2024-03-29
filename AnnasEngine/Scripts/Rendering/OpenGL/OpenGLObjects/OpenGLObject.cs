﻿namespace AnnasEngine.Scripts.Rendering.OpenGL.OpenGLObjects;

// Opengl object is an opengl construct that contains some state
public abstract class OpenGLObject : IDisposable
{
    public int Handle { get; protected set; }

    public abstract void Bind();

    public abstract void Unbind();

    public abstract void Dispose();

    ~OpenGLObject()
    {
        Dispose();
    }
}
