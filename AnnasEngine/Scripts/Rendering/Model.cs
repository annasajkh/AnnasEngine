using AnnasEngine.Scripts.DataStructures.GameObjects;
using OpenTK.Mathematics;
using System.Collections.ObjectModel;

namespace AnnasEngine.Scripts.Rendering
{
    public class Model : GameObjectComponent
    {
        public readonly List<Mesh> meshes;

        public ReadOnlyCollection<Mesh> Meshes => meshes.AsReadOnly();

        public Matrix4 ModelMatrix
        {
            get
            {
                return Matrix4.CreateScale(((GameObject3D)GetParent()).Transform.scale.X, ((GameObject3D)GetParent()).Transform.scale.Y, ((GameObject3D)GetParent()).Transform.scale.Z) *
                       Matrix4.CreateFromQuaternion(((GameObject3D)GetParent()).Transform.rotation) *
                       Matrix4.CreateTranslation(((GameObject3D)GetParent()).Transform.position.X, ((GameObject3D)GetParent()).Transform.position.Y, ((GameObject3D)GetParent()).Transform.position.Z);
            }
        }

        public Model(List<Mesh> meshes)
        {
            this.meshes = meshes;
        }

        public Model(Mesh mesh) : this(new List<Mesh>() { mesh })
        {

        }


    }
}
