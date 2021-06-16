using OpenTK.Mathematics;
using System;

namespace StarSystemSimulator.Graphics
{
	public static class PlanetRenderable
	{
		static Renderable renderable;

		public static void Load()
		{
			renderable = new Renderable(getVectors(Vector3.Zero, Color4.White));
		}

		public static void Render()
		{
			MasterRenderer.SetPolygonMode(PolygonMode.Line);
			renderable.Render();
			MasterRenderer.SetPolygonMode(PolygonMode.Fill);
		}

		public static void Dispose()
		{
			renderable.Dispose();
		}

		static Vector[] getVectors(Vector3 position, Color4 color, float size = 1f)
		{
			const int horizontalLines = 12;
			const int verticalLines = 12;

			// from https://stackoverflow.com/a/47416720
			var vertices = new Vector[horizontalLines * verticalLines];

			int index = 0;
			for (int m = 0; m < horizontalLines; m++)
			{
				for (int n = 0; n < verticalLines - 1; n++)
				{
					float x = MathF.Sin(MathF.PI * m / horizontalLines) * MathF.Cos(2 * MathF.PI * n / verticalLines);
					float y = MathF.Sin(MathF.PI * m / horizontalLines) * MathF.Sin(2 * MathF.PI * n / verticalLines);
					float z = MathF.Cos(MathF.PI * m / horizontalLines);
					vertices[index++] = new Vector(new Vector4(position + new Vector3(x, y, z) * size, 1.0f), color, Vector2.Zero);
				}
			}

			return vertices;
		}
	}
}
