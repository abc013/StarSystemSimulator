using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace StarSystemSimulator.Graphics
{
	/// <summary>
	/// Class designed to render a Point.
	/// Since all points look the same, only one renderable is necessary.
	/// This renderable can then be moved via a Matrix to its specific position and colored in the shader. This is why this class is static.
	/// </summary>
	public static class PointRenderable
	{
		static Renderable renderable;

		public static void Load()
		{
			renderable = new Renderable(getVectors(Vector3.Zero, Color4.White));
		}

		public static void Render()
		{
			renderable.Render(customLength: 6);
			renderable.Render(PrimitiveType.Lines, 6, 4);
		}

		public static void Dispose()
		{
			renderable.Dispose();
		}

		static Vector[] getVectors(Vector3 position, Color4 color, float size = 1f)
		{
			return new[]
			{
				new Vector(new Vector4(position + new Vector3(-size, -size, 0), 1.0f), Vector4.Zero, color, Vector2.Zero),
				new Vector(new Vector4(position + new Vector3(-size, size, 0), 1.0f), Vector4.Zero, color, Vector2.Zero),
				new Vector(new Vector4(position + new Vector3(size, -size, 0), 1.0f), Vector4.Zero, color, Vector2.Zero),
				new Vector(new Vector4(position + new Vector3(size, -size, 0), 1.0f), Vector4.Zero, color, Vector2.Zero),
				new Vector(new Vector4(position + new Vector3(-size, size, 0), 1.0f), Vector4.Zero, color, Vector2.Zero),
				new Vector(new Vector4(position + new Vector3(size, size, 0), 1.0f), Vector4.Zero, color, Vector2.Zero),
				new Vector(new Vector4(position + new Vector3(-3 * size, 0, 0), 1.0f), Vector4.Zero, color, Vector2.Zero),
				new Vector(new Vector4(position + new Vector3( 3 * size, 0, 0), 1.0f), Vector4.Zero, color, Vector2.Zero),
				new Vector(new Vector4(position + new Vector3(0, -3 * size, 0), 1.0f), Vector4.Zero, color, Vector2.Zero),
				new Vector(new Vector4(position + new Vector3(0,  3 * size, 0), 1.0f), Vector4.Zero, color, Vector2.Zero),
			};
		}
	}
}