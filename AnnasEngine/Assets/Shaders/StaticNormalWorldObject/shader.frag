#version 400 core

#define MAX_DIRECTIONAL_LIGHT 1
#define MAX_POINT_LIGHT 8
#define MAX_SPOT_LIGHT 8

// From https://learnopengl.com/code_viewer_gh.php?code=src/2.lighting/6.multiple_lights/6.multiple_lights.fs
struct Material {
    sampler2D diffuse;
    sampler2D specular;

    float shininess;
}; 

struct DirectionalLight
{
	vec3 direction;

	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
};

struct PointLight {    
    vec3 position;
    
    float constant;
    float linear;
    float quadratic;  

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

struct SpotLight {
    vec3 position;
    vec3 direction;
    float cutOff;
    float outerCutOff;
  
    float constant;
    float linear;
    float quadratic;
  
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;       
};

vec3 CalculateDirectionalLight(DirectionalLight light, vec3 normal, vec3 viewDir);
vec3 CalculatePointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir);
vec3 CalculateSpotLight(SpotLight light, vec3 normal, vec3 fragPos, vec3 viewDir);

// outputs
out vec4 FragColor;

// inputs
in vec3 gFragPos;  
in vec4 gColor;
in vec3 gNormal;
in vec2 gUV;
in vec3 gViewPos;

uniform Material material;
uniform vec3 lightColor;

uniform int directionalLightCount;
uniform int pointLightCount;
uniform int spotLightCount;

uniform DirectionalLight directionalLights[MAX_DIRECTIONAL_LIGHT];
uniform PointLight pointLights[MAX_POINT_LIGHT];
uniform SpotLight spotLights[MAX_SPOT_LIGHT];


void main()
{
    vec3 viewDir = normalize(gViewPos - gFragPos);
    
    vec3 result = vec3(0, 0, 0);

    // phase 1: directional lights
    for(int i = 0; i < directionalLightCount; i++)
    {
        result += CalculateDirectionalLight(directionalLights[i], gNormal, viewDir);   
    }

    // phase 2: point lights
    for(int i = 0; i < pointLightCount; i++)
    {
        result += CalculatePointLight(pointLights[i], gNormal, gFragPos, viewDir);
    }

    // phase 3: spot lights
    for(int i = 0; i < spotLightCount; i++)
    {        
        result += CalculateSpotLight(spotLights[i], gNormal, gFragPos, viewDir);
    }

    FragColor = vec4(result, 1.0);
}

// calculates the color when using a directional light.
vec3 CalculateDirectionalLight(DirectionalLight light, vec3 normal, vec3 viewDir)
{
    vec3 lightDir = normalize(-light.direction);

    // diffuse shading
    float diff = max(dot(normal, lightDir), 0.0);

    // specular shading
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    
    // combine results
    vec3 ambient = light.ambient * vec3(texture(material.diffuse, gUV));
    vec3 diffuse = light.diffuse * diff * vec3(texture(material.diffuse, gUV));
    vec3 specular = light.specular * spec * vec3(texture(material.specular, gUV));

    return (ambient + diffuse + specular);
}

// calculates the color when using a point light.
vec3 CalculatePointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir)
{
    vec3 lightDir = normalize(light.position - fragPos);

    // diffuse shading
    float diff = max(dot(normal, lightDir), 0.0);
    
    // specular shading
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    
    // attenuation
    float distance = length(light.position - fragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));    
    
    // combine results
    vec3 ambient = light.ambient * vec3(texture(material.diffuse, gUV));
    vec3 diffuse = light.diffuse * diff * vec3(texture(material.diffuse, gUV));
    vec3 specular = light.specular * spec * vec3(texture(material.specular, gUV));
    
    ambient *= attenuation;
    diffuse *= attenuation;
    specular *= attenuation;
    
    return (ambient + diffuse + specular);
}

// calculates the color when using a spot light.
vec3 CalculateSpotLight(SpotLight light, vec3 normal, vec3 fragPos, vec3 viewDir)
{
    vec3 lightDir = normalize(light.position - fragPos);
    
    // diffuse shading
    float diff = max(dot(normal, lightDir), 0.0);
    
    // specular shading
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    
    // attenuation
    float distance = length(light.position - fragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));    
    
    // spotlight intensity
    float theta = dot(lightDir, normalize(-light.direction)); 
    float epsilon = light.cutOff - light.outerCutOff;
    float intensity = clamp((theta - light.outerCutOff) / epsilon, 0.0, 1.0);
    
    // combine results
    vec3 ambient = light.ambient * vec3(texture(material.diffuse, gUV));
    vec3 diffuse = light.diffuse * diff * vec3(texture(material.diffuse, gUV));
    vec3 specular = light.specular * spec * vec3(texture(material.specular, gUV));
    
    ambient *= attenuation * intensity;
    diffuse *= attenuation * intensity;
    specular *= attenuation * intensity;
    
    return (ambient + diffuse + specular);
}