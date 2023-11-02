using AnnasEngine.Scripts.GameObjects;

namespace AnnasEngine.Scripts.DataStructures.Containers
{
    public abstract class GameObjectComponent : IComponent
    {
        private GameObject? parent;

        public GameObject GetParent()
        {
            if (parent == null)
            {
                throw new Exception("Error: this GameObjectComponent is not assign to a GameObject");
            }

            return parent;
        }
    }
}