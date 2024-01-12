using OpenTK.Windowing.GraphicsLibraryFramework;

namespace AnnasEngine.Scripts.DataStructures;

public interface IUpdateable
{
    public void Update(KeyboardState keyboardState, MouseState mouseState, float delta);
}
