#version 130
precision highp float;

in vec4 vs_color;
in vec2 vs_texCoord;

out vec4 color;

void main(void)
{
    color = vs_color;
    if (color.a == 0.0)
        discard;
}