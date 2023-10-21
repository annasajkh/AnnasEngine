namespace AnnasEngine.Scripts.OpenGL.Shaders
{
    public class DefaultShader : Shader
    {
        public DefaultShader()
            : base(vertexShaderPath: Path.GetFullPath("Assets/Shaders/Default/shader.vert"),
                   geometryShaderPath: Path.GetFullPath("Assets/Shaders/Default/shader.geom"),
                   fragmentShaderPath: Path.GetFullPath("Assets/Shaders/Default/shader.frag"))
        {

        }
    }
}
