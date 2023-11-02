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
                return Matrix4.CreateScale(((GameObject3D)GetParent()).Transform.scale.X, ((GameObject3D)GetParent()).Transform.scale.Y, ((GameObject3D)GetParent()).Transform.scale.Z) *
                       Matrix4.CreateFromQuaternion(((GameObject3D)GetParent()).Transform.rotation) *
                       Matrix4.CreateTranslation(((GameObject3D)GetParent()).Transform.position.X, ((GameObject3D)GetParent()).Transform.position.Y, ((GameObject3D)GetParent()).Transform.position.Z);
            }
        }

        public Model(Mesh mesh)
        {
            Mesh = mesh;
        }
    }
}
