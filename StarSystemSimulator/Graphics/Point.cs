using OpenTK.Mathematics;

namespace StarSystemSimulator.Graphics
{
	public class Point
	{
		public readonly Vector3 Position;
		public Color4 Color;

		readonly Matrix4 objectMatrix;

		public Point(Vector3 position, Color4 color, float size = 0.01f)
		{
			Position = position;
			Color = color;

			objectMatrix = Matrix4.CreateScale(size) * Matrix4.CreateTranslation(position);
		}

		public void Render()
		{
			MasterRenderer.DefaultManager.UniformModelView(Camera.InverseScaleMatrix * objectMatrix);
			MasterRenderer.DefaultManager.UniformColor(Color);
			PointRenderable.Render();
		}
	}
}
