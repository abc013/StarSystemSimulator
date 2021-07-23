using OpenTK.Mathematics;
using System.Drawing;

namespace StarSystemSimulator.Scripting
{
	static class LuaFunctions
	{
		[LuaFunction("Vector")]
		public static Vector3 CreateVector(float x, float y, float z) => new Vector3(x, y, z);

		[LuaFunction("ColorFromName")]
		public static Color4 CreateColor(string name) => Color.FromName(name);

		[LuaFunction("ColorFromRGB")]
		public static Color4 CreateColor(float r, float g, float b) => new Color4(r, g, b, 1f);

		[LuaFunction("ColorFromRGBA")]
		public static Color4 CreateColor(float r, float g, float b, float a) => new Color4(r, g, b, a);
	}
}
