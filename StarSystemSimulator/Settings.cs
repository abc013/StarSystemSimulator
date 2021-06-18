using System;
using System.IO;
using System.Linq;

namespace StarSystemSimulator
{
	public class Settings
	{
		/// <summary>
		/// Height of the default window bar (estimated).
		/// </summary>
		public const int WindowBar = 60;

		/// <summary>
		/// X-Coordinate of the window.
		/// </summary>
		public static int GraphX = 0;
		/// <summary>
		/// Y-Coordinate of the window.
		/// </summary>
		public static int GraphY = WindowBar;

		/// <summary>
		/// Width of the window. If it is bigger than the width of the screen, the width of the screen will be used instead.
		/// </summary>
		public static int GraphWidth = 1920;

		/// <summary>
		/// Height of the window. If it is bigger than the height of the screen, the height of the screen will be used instead.
		/// </summary>
		public static int GraphHeight = 1080;

		/// <summary>
		/// Enables the welcome dialog, which will be shown on startup.
		/// </summary>
		public static bool ShowWelcomeDialog = true;

		/// <summary>
		/// Automatically resizes and positions the information window. Disable this if you want to keep your latest size and position arguments saved.
		/// </summary>
		public static bool AutoResizeWindow = true;

		/// <summary>
		/// Enables scaling the UI window. Greater value means bigger. This is only active when <see cref="UseSystemUIScaling"/> is false.
		/// </summary>
		public static float UIScaling = 1.5f;

		/// <summary>
		/// Enable using the system dpi settings for scaling of the UI window.
		/// </summary>
		public static bool UseSystemUIScaling = true;

		/// <summary>
		/// Determines whether to include the UI in the screenshot.
		/// </summary>
		public static bool ScreenshotUI = false;

		/// <summary>
		/// Render points and viewport crosshair.
		/// </summary>
		public static bool Points = true;
		/// <summary>
		/// Always use a random color when generating a point.
		/// </summary>
		public static bool UseRandomColor = true;
		/// <summary>
		/// if <see cref="UseRandomColor"/> is set to false, use this color (defined as hex value) instead.
		/// </summary>
		public static string StandardColor = "#ffffffff";

		/// <summary>
		/// Allows modification of the camera movement speed.
		/// </summary>
		public static float CameraMovementSpeed = 0.01f;

		/// <summary>
		/// Allows modification of the camera rotation speed.
		/// </summary>
		public static float CameraRotationSpeed = 0.01f;

		/// <summary>
		/// Allows modification of the camera zoom speed.
		/// </summary>
		public static float CameraZoomSpeed = 0.1f;

		/// <summary>
		/// Default scale.
		/// </summary>
		public static float Scale = 2f;

		/// <summary>
		/// Default X-location.
		/// </summary>
		public static float LocationX = 0f;
		/// <summary>
		/// Default Y-location.
		/// </summary>
		public static float LocationY = 0f;
		/// <summary>
		/// Default Z-location.
		/// </summary>
		public static float LocationZ = 0f;

		/// <summary>
		/// TimeStep of the Simulation.
		/// </summary>
		public static float TimeStep = 0.0001f;
		/// <summary>
		/// Decides whether the Simulation is paused.
		/// </summary>
		public static bool Paused = true;

		/// <summary>
		/// Draw trails of the mass objects.
		/// </summary>
		public static bool DrawTrails = true;

		/// <summary>
		/// Calculate size based on size.
		/// </summary>
		public static bool SizeBasedOnMass = false;

		/// <summary>
		/// Object size factor.
		/// </summary>
		public static float ObjectScaleFator = 1f;

		public static void Initialize()
		{
			// Use a settings class to gain access to reflection methods, making assigning variables much easier
			new Settings();

			// Translate the Hex-string into a color
			Utils.StandardColor = System.Drawing.ColorTranslator.FromHtml(StandardColor);
		}

		Settings()
		{
			var file = FileManager.CheckFile("settings.txt");

			if (string.IsNullOrEmpty(file))
				return;

			var fields = GetType().GetFields().Where(f => f.IsStatic && f.IsPublic);

			using var reader = new StreamReader(file);

			while (!reader.EndOfStream)
			{
				var line = reader.ReadLine().Trim();

				if (line.StartsWith("//"))
					continue;

				var split = line.Split('=');
				var name = split[0].Trim();
				var value = split[1].Trim();

				var field = fields.FirstOrDefault(f => f.Name == name);
				if (field == null)
					throw new InvalidSettingsException($"Unable to load settings key '{name}' because it does not exist (value: {value}).");

				field.SetValue(this, convert(field.FieldType, name, value));
			}
		}

		static object convert(Type type, string key, string value)
		{
			if (type == typeof(int))
			{
				if (int.TryParse(value, out var res))
					return res;

				throw new InvalidSettingsException($"Invalid value {value} of {key}. {type} expected.");
			}
			else if (type == typeof(float))
			{
				if (float.TryParse(value, out var res))
					return res;

				throw new InvalidSettingsException($"Invalid value {value} of {key}. {type} expected.");
			}
			else if (type == typeof(double))
			{
				if (double.TryParse(value, out var res))
					return res;

				throw new InvalidSettingsException($"Invalid value {value} of {key}. {type} expected.");
			}
			else if (type == typeof(bool))
			{
				if (bool.TryParse(value, out var res))
					return res;

				throw new InvalidSettingsException($"Invalid value {value} of {key}. {type} expected.");
			}
			else if (type == typeof(string))
			{
				return value;
			}

			throw new InvalidSettingsException($"Missing conversion method for type {type}");
		}
	}
}
