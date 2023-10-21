using AnnasEngine.Scripts.DataStructures.Containers;
using AnnasEngine.Scripts.Rendering;
using OpenTK.Mathematics;

namespace AnnasEngine.Scripts.GameObjects.Components
{
    public class Model : GameObjectComponent
    {
        public Mesh Mesh { get; set; }

        public Matrix4 ModelMatrix
        {
            get
            {
                return Matrix4.CreateScale(GetParent().Transform.scale.X, GetParent().Transform.scale.Y, GetParent().Transform.scale.Z) *
                       Matrix4.CreateFromQuaternion(GetParent().Transform.rotation) *
                       Matrix4.CreateTranslation(GetParent().Transform.position.X, GetParent().Transform.position.Y, GetParent().Transform.position.Z);
            }
        }

        public Model(Mesh mesh)
        {
            Mesh = mesh;
        }
    }
}
