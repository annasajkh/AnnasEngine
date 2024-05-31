using AnnasEngine.Scripts.DataStructures.GameObjects;
using OpenTK.Mathematics;

namespace AnnasEngine.Scripts.Rendering.Lights;

public abstract class Light : GameObjectComponent
{
    public Shader Shader { get; protected set; }

    private Vector3 ambient = new Vector3(0.2f, 0.2f, 0.2f);
    public Vector3 Ambient
    {
        get
        {
            return ambient;
        }
        set
        {
            ambient = value;
            Shader.SetVector3($"{LightUniformName}[{ShaderIndex}].ambient", Ambient);
        }
    }

    private Vector3 diffuse = new Vector3(0.8f, 0.8f, 0.8f);
    public Vector3 Diffuse
    {
        get
        {
            return diffuse;
        }
        set
        {
            diffuse = value;
            Shader.SetVector3($"{LightUniformName}[{ShaderIndex}].diffuse", Diffuse);
        }
    }

    private Vector3 specular = new Vector3(0.2f, 0.2f, 0.2f);
    public Vector3 Specular
    {
        get
        {
            return specular;
        }
        set
        {
            specular = value;
            Shader.SetVector3($"{LightUniformName}[{ShaderIndex}].specular", Specular);
        }
    }

    public string LightUniformName { get; }

    protected static Dictionary<DirectionalLight, int> directionalLights = new Dictionary<DirectionalLight, int>();
    protected static Dictionary<PointLight, int> pointLights = new Dictionary<PointLight, int>();
    protected static Dictionary<SpotLight, int> spotLights = new Dictionary<SpotLight, int>();

    public int ShaderIndex { get; }

    public Light(string lightUniformName, Shader shader, int shaderIndex)
    {
        LightUniformName = lightUniformName;
        Shader = shader;
        ShaderIndex = shaderIndex;

        Shader.SetVector3($"{LightUniformName}[{ShaderIndex}].ambient", Ambient);
        Shader.SetVector3($"{LightUniformName}[{ShaderIndex}].diffuse", Diffuse);
        Shader.SetVector3($"{LightUniformName}[{ShaderIndex}].specular", Specular);

        switch (this)
        {
            case DirectionalLight:
                directionalLights.Add((DirectionalLight)this, shaderIndex);
                break;

            case PointLight:
                pointLights.Add((PointLight)this, shaderIndex);
                break;

            case SpotLight:
                spotLights.Add((SpotLight)this, shaderIndex);
                break;
        }
    }
}
