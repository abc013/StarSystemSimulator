using OpenTK.Windowing.Desktop;
using System;
using System.Globalization;

namespace StarSystemSimulator
{
	static class Program
	{
		[STAThread]
		public static void Main()
		{
			// Reset culture to invariant. This is needed because otherwise string parsing of floats in different countries can lead to different results/crashing.
			CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

			Log.Initialize();
			AppDomain.CurrentDomain.UnhandledException += handleError;
			Settings.Initialize();

			initWindow();
		}

		static void handleError(object handler, UnhandledExceptionEventArgs args)
		{
			Log.WriteException(args.ExceptionObject);
		}

		static void initWindow()
		{
			var gameSettings = GameWindowSettings.Default;
			var nativeSettings = new NativeWindowSettings()
			{
				Title = "StarSystemSimulator",
				APIVersion = new Version(3, 3),
				Location = new OpenTK.Mathematics.Vector2i(Settings.GraphX, Settings.GraphY),
				Size = new OpenTK.Mathematics.Vector2i(Settings.GraphWidth, Settings.GraphHeight)
			};

			var graphWindow = new GraphWindow(gameSettings, nativeSettings);
			graphWindow.Run();
		}

		public static void Exit()
		{
			Log.Exit();
		}
	}
}
