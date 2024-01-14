using AnnasEngine.Scripts.Rendering.OpenGL.OpenGLObjects;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace AnnasEngine.Scripts.Rendering;

public class Shader : OpenGLObject
{
    private static Dictionary<string, int> uniformLocations = new Dictionary<string, int>();

    public Shader(string vertexShaderPath, string geometryShaderPath, string fragmentShaderPath)
    {
        string vertexShaderSource = File.ReadAllText(Path.GetFullPath(vertexShaderPath));
        string geometryShaderSource = File.ReadAllText(Path.GetFullPath(geometryShaderPath));
        string fragmentShaderSource = File.ReadAllText(Path.GetFullPath(fragmentShaderPath));

        int vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, vertexShaderSource);

        int geometryShader = GL.CreateShader(ShaderType.GeometryShader);
        GL.ShaderSource(geometryShader, geometryShaderSource);

        int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, fragmentShaderSource);


        GL.CompileShader(vertexShader);
        GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out int successVertex);

        if (successVertex == 0)
        {
            string infoLog = GL.GetShaderInfoLog(vertexShader);
            Console.WriteLine(infoLog);
        }


        GL.CompileShader(geometryShader);
        GL.GetShader(geometryShader, ShaderParameter.CompileStatus, out int successGeometry);

        if (successGeometry == 0)
        {
            string infoLog = GL.GetShaderInfoLog(geometryShader);
            Console.WriteLine(infoLog);
        }


        GL.CompileShader(fragmentShader);
        GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out int successFragment);

        if (successFragment == 0)
        {
            string infoLog = GL.GetShaderInfoLog(fragmentShader);
            Console.WriteLine(infoLog);
        }


        Handle = GL.CreateProgram();

        GL.AttachShader(Handle, vertexShader);
        GL.AttachShader(Handle, geometryShader);
        GL.AttachShader(Handle, fragmentShader);

        GL.LinkProgram(Handle);

        GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out int success);

        if (success == 0)
        {
            string infoLog = GL.GetProgramInfoLog(Handle);
            Console.WriteLine(infoLog);
        }


        GL.DetachShader(Handle, vertexShader);
        GL.DetachShader(Handle, geometryShader);
        GL.DetachShader(Handle, fragmentShader);


        GL.DeleteShader(vertexShader);
        GL.DeleteShader(geometryShader);
        GL.DeleteShader(fragmentShader);
    }

    public override void Bind()
    {
        GL.UseProgram(Handle);
    }

    public override void Unbind()
    {
        GL.UseProgram(0);
    }

    public void SetFloat(string name, float value)
    {
        Bind();
        GL.Uniform1(GetUniformLocation(name), value);
    }

    public void SetInt(string name, int value)
    {
        Bind();
        GL.Uniform1(GetUniformLocation(name), value);
    }

    public void SetVector2(string name, Vector2 data)
    {
        Bind();
        GL.Uniform2(GetUniformLocation(name), data);
    }

    public void SetVector3(string name, Vector3 data)
    {
        Bind();
        GL.Uniform3(GetUniformLocation(name), data);
    }

    public void SetMatrix4(string name, bool transposed, Matrix4 data)
    {
        Bind();
        GL.UniformMatrix4(GetUniformLocation(name), transposed, ref data);
    }


    private int GetUniformLocation(string name)
    {
        if (uniformLocations.ContainsKey(name))
        {
            return uniformLocations[name];
        }
        else
        {
            uniformLocations.Add(name, GL.GetUniformLocation(Handle, name));
            return uniformLocations[name];
        }
    }

    public override void Dispose()
    {
        Console.WriteLine($"Shader Program: {Handle} is Unloaded");

        GL.DeleteProgram(Handle);
        GC.SuppressFinalize(this);
    }
}
