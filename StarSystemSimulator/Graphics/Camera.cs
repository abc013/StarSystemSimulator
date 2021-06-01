﻿using OpenTK.Mathematics;

namespace StarSystemSimulator.Graphics
{
	/// <summary>
	/// Class to keep track of movement in virtual space.
	/// </summary>
	public static class Camera
	{
		public static float RelativeSpeed => Settings.CameraSpeed / Scale;

		public static readonly Matrix4 IdentityMatrix = Matrix4.Identity;
		public static Matrix4 CameraMatrix;
		public static Matrix4 ScaleMatrix;
		public static Matrix4 InverseScaleMatrix;

		/// <summary>
		/// Window ratio.
		/// </summary>
		public static float Ratio { get; private set; }

		public static Vector3 Location { get; private set; }

		public static float Scale { get; private set; }

		public static bool Changed { get; private set; }

		public static void Load()
		{
			Location = new Vector3(Settings.LocationX, Settings.LocationY, Settings.LocationZ);
			Scale = Settings.Scale;
			Changed = true;

			ResizeViewport(Settings.GraphWidth, Settings.GraphHeight);
		}

		public static void SetTranslation(float x, float y, float z)
		{
			Location = new Vector3(x, y, z);
			Changed = true;
		}

		public static void SetScale(float s)
		{
			Scale = s;
			Changed = true;
		}

		/// <summary>
		/// Move in the specified directions. The values will be multiplied by the camera speed as well.
		/// In order to allow movement in deeper regions, moving is also being divided by the current scale.
		/// </summary>
		public static void Translate(int x, int y, int z)
		{
			var speed = RelativeSpeed;
			Location += new Vector3(x * speed, y * speed, z * speed);

			Changed = true;
		}

		public static void Scaling(float s)
		{
			Scale += s * Scale;
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

			var locMatrix = Matrix4.CreateTranslation(new Vector3(-Location.X, -Location.Y, Location.Z));

			var scale = Scale / 2f;
			ScaleMatrix = Matrix4.CreateScale(scale / Ratio, scale, 0);
			InverseScaleMatrix = Matrix4.CreateScale(1 / scale);

			CameraMatrix = locMatrix * ScaleMatrix;
		}
	}
}
