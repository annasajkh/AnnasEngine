using AnnasEngine.Scripts.DataStructures.Containers;
using OpenTK.Mathematics;

namespace AnnasEngine.Scripts.GameObjects.Components
{
    public class Camera3D : GameObjectComponent
    {
        public Vector3 Front
        {
            get
            {
                return Vector3.Transform(Vector3.UnitZ, GetParent().Transform.rotation);
            }
        }

        public Vector3 Back
        {
            get
            {
                return -Front;
            }
        }

        public Vector3 Right
        {
            get
            {
                return Vector3.Normalize(Vector3.Cross(Front, Vector3.UnitY));
            }
        }

        public Vector3 Left
        {
            get
            {
                return -Vector3.Normalize(Vector3.Cross(Front, Vector3.UnitY));
            }
        }

        public Vector2 Size { get; set; }

        public float Fov { get; set; }
        public float Near { get; set; }
        public float Far { get; set; }

        public Matrix4 ViewMatrix
        {
            get
            {
                return Matrix4.LookAt(GetParent().Transform.position, GetParent().Transform.position + Front, Vector3.UnitY);
            }
        }

        public Matrix4 ProjectionMatrix
        {
            get
            {
                float scaledFov = MathHelper.Clamp(Fov * GetParent().Transform.scale.Z, 1, 179);

                return Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(scaledFov), Size.X * GetParent().Transform.scale.X / (Size.Y * GetParent().Transform.scale.Y), Near, Far);
            }
        }

        public Camera3D(Vector2 size, float fov, float near, float far)
        {

            Size = size;

            Fov = fov;
            Near = near;
            Far = far;
        }
    }
}
