using OpenTK.Mathematics;

namespace AnnasEngine.Scripts.Rendering.Lights
{
    public class DirectionalLight : Light
    {
        private Vector3 direction;

        public Vector3 Direction
        {
            get
            {
                return direction;
            }

            set
            {
                direction = value;
                Shader.SetVector3($"directionalLights[{directionalLights[this]}].direction", Direction);
            }
        }

        public DirectionalLight(Vector3 direction, Shader shader, int shaderIndex) : base("directionalLights", shader, shaderIndex)
        {
            Direction = direction;
            Shader = shader;

            Shader.SetVector3($"directionalLights[{directionalLights[this]}].direction", Direction);
            Shader.SetInt("directionalLightCount", directionalLights.Count);
        }
    }
}
