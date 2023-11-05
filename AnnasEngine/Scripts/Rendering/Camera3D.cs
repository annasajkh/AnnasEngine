using AnnasEngine.Scripts.DataStructures.GameObjects;
using OpenTK.Mathematics;

namespace AnnasEngine.Scripts.Rendering
{
    public class Camera3D : GameObjectComponent
    {
        public Vector3 Front
        {
            get
            {
                return Vector3.Transform(Vector3.UnitZ, ((GameObject3D)GetParent()).Transform.rotation);
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
                return Matrix4.LookAt(((GameObject3D)GetParent()).Transform.position, ((GameObject3D)GetParent()).Transform.position + Front, Vector3.UnitY);
            }
        }

        public Matrix4 ProjectionMatrix
        {
            get
            {
                float scaledFov = MathHelper.Clamp(Fov * ((GameObject3D)GetParent()).Transform.scale.Z, 1, 179);

                return Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(scaledFov), Size.X * ((GameObject3D)GetParent()).Transform.scale.X / (Size.Y * ((GameObject3D)GetParent()).Transform.scale.Y), Near, Far);
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
