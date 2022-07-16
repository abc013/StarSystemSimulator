#version 330 core
precision highp float;

in vec4 vs_normal;
in vec4 vs_color;
in vec2 vs_texCoord;
in vec4 vs_position;
in vec4 vs_lightPos;
in float vs_emissive;

out vec4 color;

void main(void)
{
	vec4 normal = normalize(vs_normal);
	vec4 lightdir = normalize(vs_lightPos - vs_position);

	float emissive = vs_emissive;
	vec4 emissiveColor = emissive * vec4(1,1,1,1);

	float diffuse = max(dot(normal, lightdir), 0.0);
	vec4 diffuseColor = diffuse * vec4(1,1,1,1);

	float glossy = 0.0;
	if (diffuse > 0.0) {
		// glossy = pow(max(dot(normal, lightdir), 0.0), 32);
	}

    color = max(emissiveColor, diffuseColor) * vs_color;

	color.a = 1.0;
    // if (color.a == 0.0)
    //    discard;
}