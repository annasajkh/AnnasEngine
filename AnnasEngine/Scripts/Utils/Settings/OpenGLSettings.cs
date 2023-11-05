using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace AnnasEngine.Scripts.Utils.Settings
{
    public static class OpenGLSettings
    {
        public static void Init()
        {
            GL.Enable(EnableCap.CullFace);

            GL.Enable(EnableCap.DepthTest);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            StbImage.stbi_set_flip_vertically_on_load(1);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);
        }
    }
}
