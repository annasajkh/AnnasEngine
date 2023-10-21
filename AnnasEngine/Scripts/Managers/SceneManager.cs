using AnnasEngine.Scripts.Utils;

namespace AnnasEngine.Scripts.Managers
{
    public class SceneManager
    {
        public Scene CurrentScene { get; private set; }

        private Dictionary<string, Scene> scenes = new Dictionary<string, Scene>();

        public SceneManager(Scene initialScene)
        {
            CurrentScene = initialScene;
        }

        public void AddScene(string name, Scene scene)
        {
            scenes.Add(name, scene);
        }

        public void RemoveScene(string name)
        {
            scenes.Remove(name);
        }

        public void ChangeScene(string sceneName)
        {
            CurrentScene.Unload();
            CurrentScene = scenes[sceneName];
            CurrentScene.Load();
        }

    }
}
