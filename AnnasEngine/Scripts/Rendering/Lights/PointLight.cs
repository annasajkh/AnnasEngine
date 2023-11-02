using AnnasEngine.Scripts.GameObjects;
using AnnasEngine.Scripts.OpenGL.Shaders;

namespace AnnasEngine.Scripts.Rendering.Lights
{
    public class PointLight : Light
    {
        // TODO: Implement the rest of the transform
        private float constant = 1;
        public float Constant
        {
            get
            {
                return constant;
            }
            set
            {
                constant = value;
                Shader.SetFloat($"pointLights[{pointLights[this]}].constant", Constant);
            }
        }

        private float linear = 0.014f;
        public float Linear
        {
            get
            {
                return linear;
            }
            set
            {
                linear = value;
                Shader.SetFloat($"pointLights[{pointLights[this]}].linear", Linear);
            }
        }

        private float quadratic = 0.0007f;
        public float Quadratic
        {
            get
            {
                return quadratic;
            }
            set
            {
                quadratic = value;
                Shader.SetFloat($"pointLights[{pointLights[this]}].quadratic", Quadratic);
            }
        }

        public PointLight(Shader shader, int shaderIndex) : base("pointLights", shader, shaderIndex)
        {
            Shader = shader;

            Shader.SetFloat($"pointLights[{pointLights[this]}].constant", Constant);
            Shader.SetFloat($"pointLights[{pointLights[this]}].linear", Linear);
            Shader.SetFloat($"pointLights[{pointLights[this]}].quadratic", Quadratic);

            Shader.SetInt("pointLightCount", pointLights.Count);
        }

        public void Update()
        {
            Shader.SetVector3($"pointLights[{pointLights[this]}].position", ((GameObject3D)GetParent()).Transform.position);
        }
    }
}
