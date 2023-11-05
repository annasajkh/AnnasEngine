#version 400 core


// outputs
out DATA
{
    vec3 vFragPos;
    vec2 vUV;

    mat4 vProjection;

    vec3 vViewPos;
} data_out;


// inputs
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec2 aUV;

uniform mat4 uModel;
uniform mat4 uView;
uniform mat4 uProjection;

uniform vec3 uViewPos;


void main()
{
    data_out.vFragPos = vec3(uModel * vec4(aPosition, 1.0));
    data_out.vUV = aUV;

    data_out.vProjection = uProjection;

    data_out.vViewPos = uViewPos;

    gl_Position = uView * uModel * vec4(aPosition, 1.0);
}