using OpenTK.Mathematics;
using System;

namespace StarSystemSimulator.Graphics
{
	public static class PlanetRenderable
	{
		static Renderable renderable;

		public static void Load()
		{
			renderable = new Renderable(generateSphere(Color4.White, 16, 16));
		}

		public static void Render()
		{
			renderable.Render();
		}

		public static void Dispose()
		{
			renderable.Dispose();
		}

		static Vector[] generateSphere(Color4 color, int numLatitudeLines, int numLongitudeLines, float radius = 1f)
		{
			int numVertices = (numLatitudeLines * (numLongitudeLines + 1)) + 2;
			Vector4[] positions = new Vector4[numVertices];
			Vector4[] normals = new Vector4[numVertices];
			Vector2[] texcoords = new Vector2[numVertices];
			// North pole.
			positions[0] = new Vector4(0, radius, 0, 1f);
			normals[0] = new Vector4(0, 1, 0, 1f);
			texcoords[0] = new Vector2(0, 1);
			// South pole.
			positions[numVertices - 1] = new Vector4(0, -radius, 0, 1f);
			normals[numVertices - 1] = new Vector4(0, -1, 0, 1f);
			texcoords[numVertices - 1] = new Vector2(0, 0);
			// +1.0f because there's a gap between the poles and the first parallel.
			float latitudeSpacing = 1.0f / (numLatitudeLines + 1.0f);
			float longitudeSpacing = 1.0f / numLongitudeLines;

			// start writing new vertices at position 1
			int v = 1;
			for (int latitude = 0; latitude < numLatitudeLines; latitude++)
			{
				for (int longitude = 0; longitude <= numLongitudeLines; longitude++)
				{
					// Scale coordinates into the 0...1 texture coordinate range,
					// with north at the top (y = 1).
					texcoords[v] = new Vector2(longitude * longitudeSpacing, 1.0f - ((latitude + 1) * latitudeSpacing));
					// Convert to spherical coordinates:
					// theta is a longitude angle (around the equator) in radians.
					// phi is a latitude angle (north or south of the equator).
					float theta = texcoords[v].X * 2.0f * MathF.PI;
					float phi = (texcoords[v].Y - 0.5f) * MathF.PI;
					// This determines the radius of the ring of this line of latitude.
					// It's widest at the equator, and narrows as phi increases/decreases.
					float c = (float)MathF.Cos(phi);
					// Usual formula for a vector in spherical coordinates.
					// You can exchange x & z to wind the opposite way around the sphere.
					normals[v] = new Vector4(c * MathF.Cos(theta), MathF.Sin(phi), c * MathF.Sin(theta), 1f);
					positions[v] = normals[v] * radius;
					// Proceed to the next vertex.
					v++;
				}
			}

			// Convert Vertices to triangles
			int numTriangles = numLatitudeLines * numLongitudeLines * 2;
			var vertices = new Vector[numTriangles * 3];

			v = 0;
			for (int i = 0; i < numLongitudeLines; i++)
			{
				vertices[v++] = new Vector(positions[0], normals[0], color, texcoords[0]);
				vertices[v++] = new Vector(positions[i + 2], normals[i + 2], color, texcoords[i + 2]);
				vertices[v++] = new Vector(positions[i + 1], normals[i + 1], color, texcoords[i + 1]);
			}

			// Each row has one more unique vertex than there are lines of longitude,
			// since we double a vertex at the texture seam.
			int rowLength = numLongitudeLines + 1;
			for (int latitude = 0; latitude < numLatitudeLines - 1; latitude++)
			{
				// Plus one for the pole.
				int rowStart = (latitude * rowLength) + 1;
				for (int longitude = 0; longitude < numLongitudeLines; longitude++)
				{
					int firstCorner = rowStart + longitude;
					// First triangle of quad: Top-Left, Bottom-Left, Bottom-Right
					vertices[v++] = new Vector(positions[firstCorner], normals[firstCorner], color, texcoords[firstCorner]);
					vertices[v++] = new Vector(positions[firstCorner + rowLength + 1], normals[firstCorner + rowLength + 1], color, texcoords[firstCorner + rowLength + 1]);
					vertices[v++] = new Vector(positions[firstCorner + rowLength], normals[firstCorner + rowLength], color, texcoords[firstCorner + rowLength]);
					// Second triangle of quad: Top-Left, Bottom-Right, Top-Right
					vertices[v++] = new Vector(positions[firstCorner], normals[firstCorner], color, texcoords[firstCorner]);
					vertices[v++] = new Vector(positions[firstCorner + 1], normals[firstCorner + 1], color, texcoords[firstCorner + 1]);
					vertices[v++] = new Vector(positions[firstCorner + rowLength + 1], normals[firstCorner + rowLength + 1], color, texcoords[firstCorner + rowLength + 1]);
				}
			}

			int pole = positions.Length - 1;
			int bottomRow = ((numLatitudeLines - 1) * rowLength) + 1;
			for (int i = 0; i < numLongitudeLines; i++)
			{
				vertices[v++] = new Vector(positions[pole], normals[pole], color, texcoords[pole]);
				vertices[v++] = new Vector(positions[bottomRow + i], normals[bottomRow + i], color, texcoords[bottomRow + i]);
				vertices[v++] = new Vector(positions[bottomRow + i + 1], normals[bottomRow + i + 1], color, texcoords[bottomRow + i + 1]);
			}

			return vertices;
		}
	}
}
