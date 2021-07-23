using OpenTK.Mathematics;
using StarSystemSimulator.Scripting;

namespace StarSystemSimulator.Graphics
{
	/// <summary>
	/// Class to keep track of movement in virtual space.
	/// </summary>
	public static class Camera
	{
		public static float MovementSpeed => Settings.CameraMovementSpeed;
		public static float RotationSpeed => Settings.CameraRotationSpeed;
		public static float ZoomSpeed => Settings.CameraZoomSpeed;

		public static readonly Matrix4 IdentityMatrix = Matrix4.Identity;
		public static Matrix4 CameraMatrix;
		public static Matrix4 ScaleMatrix;
		public static Matrix4 InverseScaleMatrix;

		public static float Ratio { get; private set; }

		public static Vector3 Location { get; private set; }

		public static float CurrentZoom { get; private set; }

		public static Vector3 EulerRotation;
		static Quaternion rotation;

		public static bool Changed { get; private set; }

		public static void Load()
		{
			Location = new Vector3(Settings.LocationX, Settings.LocationY, Settings.LocationZ);
			rotation = Quaternion.Identity;
			Changed = true;

			ResizeViewport(Settings.GraphWidth, Settings.GraphHeight);
		}

		[LuaFunction("SetViewTranslation")]
		public static void SetTranslation(float x, float y, float z)
		{
			Location = new Vector3(x, y, z);
			Changed = true;
		}

		[LuaFunction("SetViewRotation")]
		public static void SetRotation(float x, float y, float z)
		{
			EulerRotation = new Vector3(x, y, z);
			rotation = Quaternion.FromEulerAngles(EulerRotation);
			Changed = true;
		}

		[LuaFunction("SetViewZoom")]
		public static void SetZoom(float value)
		{
			CurrentZoom = value;
			Changed = true;
		}

		[LuaFunction("TranslateView")]
		public static void Translate(int x, int y, int z)
		{
			var speed = MovementSpeed;
			Location += new Vector3(x * speed, y * speed, z * speed);

			Changed = true;
		}

		[LuaFunction("RotateView")]
		public static void Rotate(float dx, float dy)
		{
			EulerRotation.X += dy * RotationSpeed;
			EulerRotation.Z += dx * RotationSpeed;
			rotation = Quaternion.FromEulerAngles(EulerRotation);

			Changed = true;
		}

		[LuaFunction("ZoomView")]
		public static void Zoom(float value)
		{
			CurrentZoom += value * ZoomSpeed;

			Changed = true;
		}

		public static void ResizeViewport(int width, int height)
		{
			Ratio = width / (float)height;
			Changed = true;
		}

		public static void CalculateMatrix()
		{
			if (!Changed)
				return;

			Changed = false;

			var rotMatrix = Matrix4.CreateFromQuaternion(rotation);
			var loc1Matrix = Matrix4.CreateTranslation(new Vector3(0, 0, CurrentZoom));
			var loc2Matrix = Matrix4.CreateTranslation(-Location);
			var matrix = Matrix4.CreatePerspectiveFieldOfView(0.75f, Ratio, 0.1f, 200f);

			CameraMatrix = loc2Matrix * rotMatrix * loc1Matrix * matrix;
		}
	}
}
