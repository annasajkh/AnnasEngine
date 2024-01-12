using OpenTK.Mathematics;

namespace AnnasEngine.Scripts.Utils.Extensions;

public static class QuaternionExtensions
{
    public static Quaternion Euler(float pitch, float yaw, float roll)
    {
        Quaternion pitchRotation = Quaternion.FromEulerAngles(MathHelper.DegreesToRadians(pitch), 0, 0);
        Quaternion yawRotation = Quaternion.FromEulerAngles(0, MathHelper.DegreesToRadians(yaw), 0);
        Quaternion rollRotation = Quaternion.FromEulerAngles(0, 0, MathHelper.DegreesToRadians(roll));

        Quaternion quaternion = yawRotation * pitchRotation * rollRotation;
        quaternion.Normalize();

        return quaternion;
    }

}
