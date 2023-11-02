using AnnasEngine.Scripts.GameObjects;
using AnnasEngine.Scripts.OpenGL.Shaders;
using OpenTK.Mathematics;

namespace AnnasEngine.Scripts.Rendering.Lights
{
    public class SpotLight : Light
    {
        // TODO: Implement the rest of the transform
        public Vector3 Direction
        {
            get
            {
                return Vector3.Transform(Vector3.UnitZ, ((GameObject3D)GetParent()).Transform.rotation);
            }
        }

        private float cutOff = (float)Math.Cos(MathHelper.DegreesToRadians(25f));
        public float CutOff
        {
            get
            {
                return cutOff;
            }
            set
            {
                cutOff = value;
                Shader.SetFloat($"spotLights[{spotLights[this]}].cutOff", CutOff);
            }
        }

        private float outerCutOff = (float)Math.Cos(MathHelper.DegreesToRadians(30.0f));
        public float OuterCutOff
        {
            get
            {
                return outerCutOff;
            }
            set
            {
                outerCutOff = value;
                Shader.SetFloat($"spotLights[{spotLights[this]}].outerCutOff", OuterCutOff);
            }
        }

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
                Shader.SetFloat($"spotLights[{spotLights[this]}].constant", Constant);
            }
        }

        private float linear = 0.007f;
        public float Linear
        {
            get
            {
                return linear;
            }
            set
            {
                linear = value;
                Shader.SetFloat($"spotLights[{spotLights[this]}].linear", Linear);
            }
        }

        private float quadratic = 0.0002f;
        public float Quadratic
        {
            get
            {
                return quadratic;
            }
            set
            {
                quadratic = value;
                Shader.SetFloat($"spotLights[{spotLights[this]}].quadratic", Quadratic);
            }
        }

        public SpotLight(Shader shader, int shaderIndex) : base("spotLights", shader, shaderIndex)
        {
            Shader = shader;

            Shader.SetFloat($"spotLights[{spotLights[this]}].cutOff", CutOff);
            Shader.SetFloat($"spotLights[{spotLights[this]}].outerCutOff", OuterCutOff);

            Shader.SetFloat($"spotLights[{spotLights[this]}].constant", Constant);
            Shader.SetFloat($"spotLights[{spotLights[this]}].linear", Linear);
            Shader.SetFloat($"spotLights[{spotLights[this]}].quadratic", Quadratic);

            Shader.SetInt("spotLightCount", spotLights.Count);
        }

        public void Update()
        {
            Shader.SetVector3($"spotLights[{spotLights[this]}].position", ((GameObject3D)GetParent()).Transform.position);
            Shader.SetVector3($"spotLights[{spotLights[this]}].direction", Direction);
        }

    }
}
