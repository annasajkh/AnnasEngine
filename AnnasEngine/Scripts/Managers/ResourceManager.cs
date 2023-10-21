using AnnasEngine.Scripts.OpenGL.OpenGLObjects;
using AnnasEngine.Scripts.OpenGL.Shaders;
using StbImageSharp;

namespace AnnasEngine.Scripts.Managers
{
    public class ResourceManager
    {
        public Dictionary<string, Shader> Shaders { get; } = new Dictionary<string, Shader>();
        public Dictionary<string, Texture2D> Textures { get; } = new Dictionary<string, Texture2D>();

        public void AddShader(string shaderName, string vertexShaderPath, string geometryShaderPath, string fragmentShaderPath)
        {
            Shaders.Add(shaderName, new Shader(vertexShaderPath: vertexShaderPath, geometryShaderPath: geometryShaderPath, fragmentShaderPath: fragmentShaderPath));
        }

        public void AddTexture(string name, string path)
        {
            Textures.Add(name, new Texture2D(ImageResult.FromStream(File.OpenRead(path), ColorComponents.RedGreenBlueAlpha), OpenTK.Graphics.OpenGL4.TextureUnit.Texture0));
        }
    }
}
