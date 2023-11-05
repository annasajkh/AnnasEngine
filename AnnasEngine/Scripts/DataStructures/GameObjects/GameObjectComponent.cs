using AnnasEngine.Scripts.DataStructures.Containers;
using AnnasEngine.Scripts.Utils.Exceptions.GameObject;

namespace AnnasEngine.Scripts.DataStructures.GameObjects
{
    public abstract class GameObjectComponent : IComponent
    {
        private GameObject? parent;

        public GameObject GetParent()
        {
            if (parent == null)
            {
                throw new ParentNotFoundException("this GameObjectComponent is not assign to a GameObject");
            }

            return parent;
        }
    }
}