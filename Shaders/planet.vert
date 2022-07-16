#version 330 core
precision highp float;

in vec4 position;
in vec4 normal;
in vec4 color;
in vec2 texCoord;

uniform mat4 projection;
uniform mat4 modelView;
uniform vec4 modelColor;
uniform float modelEmissive;

out vec4 vs_normal;
out vec4 vs_color;
out vec2 vs_texCoord;
out vec4 vs_position;
out vec4 vs_lightPos;
out float vs_emissive;

void main(void)
{
    gl_Position = projection * modelView * position;
	vs_normal = normal;
    vs_color = color * modelColor;
	vs_texCoord = texCoord;
	vs_position = modelView * position;
	vs_lightPos = vec4(0, 0, 0, 0);
	vs_emissive = modelEmissive;
}