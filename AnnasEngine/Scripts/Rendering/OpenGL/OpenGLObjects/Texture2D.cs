using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace AnnasEngine.Scripts.Rendering.OpenGL.OpenGLObjects;

public class Texture2D : OpenGLObject
{
    // Texture unit is like slot of the texture on the shader it's like shader layout
    public TextureUnit TextureUnit { get; set; }

    public Texture2D(ImageResult textureImage, TextureUnit textureUnit)
    {
        Handle = GL.GenTexture();
        TextureUnit = textureUnit;

        GL.BindTexture(TextureTarget.Texture2D, Handle);

        GL.TexImage2D(target: TextureTarget.Texture2D,
                      level: 0,
                      internalformat: PixelInternalFormat.Rgba,
                      width: textureImage.Width,
                      height: textureImage.Height,
                      border: 0,
                      format: PixelFormat.Rgba,
                      type: PixelType.UnsignedByte,
                      pixels: textureImage.Data);

        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

        GL.BindTexture(TextureTarget.Texture2D, 0);
    }

    public override void Bind()
    {
        GL.ActiveTexture(TextureUnit);
        GL.BindTexture(TextureTarget.Texture2D, Handle);
    }

    public override void Unbind()
    {
        GL.BindTexture(TextureTarget.Texture2D, 0);
    }

    public override void Dispose()
    {
        Console.WriteLine($"Texture2D: {Handle} is Unloaded");

        GL.DeleteBuffer(Handle);
        GC.SuppressFinalize(this);
    }
}
