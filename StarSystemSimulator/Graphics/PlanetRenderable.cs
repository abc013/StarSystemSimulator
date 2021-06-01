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
			renderable.Render();
		}

		public static void Dispose()
		{
			renderable.Dispose();
		}

		static Vector[] getVectors(Vector3 position, Color4 color, float size = 1f)
		{
			return new[]
			{
				new Vector(new Vector4(position + new Vector3(-size, -size, 0), 1.0f), color, Vector2.Zero),
				new Vector(new Vector4(position + new Vector3(-size, size, 0), 1.0f), color, Vector2.Zero),
				new Vector(new Vector4(position + new Vector3(size, -size, 0), 1.0f), color, Vector2.Zero),
				new Vector(new Vector4(position + new Vector3(size, -size, 0), 1.0f), color, Vector2.Zero),
				new Vector(new Vector4(position + new Vector3(-size, size, 0), 1.0f), color, Vector2.Zero),
				new Vector(new Vector4(position + new Vector3(size, size, 0), 1.0f), color, Vector2.Zero),
			};
		}
	}
}
