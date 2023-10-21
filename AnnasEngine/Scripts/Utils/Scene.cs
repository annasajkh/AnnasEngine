using AnnasEngine.Scripts.DataStructures.Containers;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace AnnasEngine.Scripts.Utils
{
    public abstract class Scene : Container
    {
        public abstract void Load();

        public abstract void Unload();

        public abstract void WindowResized();

        public abstract void Update(KeyboardState keyboardState, MouseState mouseState, float delta);

        public abstract void Render();

    }
}
