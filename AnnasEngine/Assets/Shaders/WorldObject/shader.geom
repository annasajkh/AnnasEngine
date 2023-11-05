#version 400 core


// outputs
out vec3 gFragPos;
out vec3 gNormal;
out vec2 gUV;

out vec3 gViewPos;


// inputs
layout (triangles) in;
layout (triangle_strip, max_vertices = 3) out;

in DATA
{
    vec3 vFragPos;
    vec2 vUV;

    mat4 vProjection;

    vec3 vViewPos;
} data_in[];


void main()
{
    vec3 vectorFirst = vec3(data_in[1].vFragPos - data_in[0].vFragPos);
    vec3 vectorSecond = vec3(data_in[2].vFragPos - data_in[0].vFragPos);

    vec3 surfaceNormal = normalize(cross(vectorFirst, vectorSecond));

    gl_Position = data_in[0].vProjection * gl_in[0].gl_Position;
    
    gFragPos = data_in[0].vFragPos;
    gNormal = surfaceNormal;
    gUV = data_in[0].vUV;

    gViewPos = data_in[0].vViewPos;

    EmitVertex();

    gl_Position = data_in[1].vProjection * gl_in[1].gl_Position;
    
    gFragPos = data_in[1].vFragPos;
    gNormal = surfaceNormal;
    gUV = data_in[1].vUV;

    gViewPos = data_in[1].vViewPos;

    EmitVertex();

    gl_Position = data_in[2].vProjection * gl_in[2].gl_Position;
    
    gFragPos = data_in[2].vFragPos;
    gNormal = surfaceNormal;
    gUV = data_in[2].vUV;

    gViewPos = data_in[2].vViewPos;

    EmitVertex();

    EndPrimitive();
}