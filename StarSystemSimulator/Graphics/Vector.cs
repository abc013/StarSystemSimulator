using OpenTK.Mathematics;

namespace StarSystemSimulator.Graphics
{
	/// <summary>
	/// Class used for describing a vector that can be uploaded into GPU memory.
	/// Each vector has a position, color and texture coordinate.
	/// </summary>
	public struct Vector
	{
		/// <summary>
		/// Size of the vector in bytes.
		/// </summary>
		public const int Size = 4 * (4 + 4 + 4 + 2);

		public readonly Vector4 Position;
		public readonly Vector4 Normal;
		public readonly Color4 Color;
		public readonly Vector2 Coordinate;

		public Vector(Vector4 position, Vector4 normal, Color4 color, Vector2 coordinate)
		{
			Position = position;
			Normal = normal;
			Color = color;
			Coordinate = coordinate;
		}
	}
}
