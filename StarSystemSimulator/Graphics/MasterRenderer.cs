using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using StarSystemSimulator.Simulations;

namespace StarSystemSimulator.Graphics
{
	public enum PolygonMode
	{
		Fill = 6914,
		Line = 6913,
		Point = 6912
	}

	/// <summary>
	/// Master class dedicated to controlling the interaction with GL.
	/// For more information about how GL works, visit https://glumpy.github.io/modern-gl.html.
	/// </summary>
	public static class MasterRenderer
	{
		public static UniformManager DefaultManager { get; private set; }
		public static int DefaultShader { get; private set; }

		public static UniformManager PlanetManager { get; private set; }
		public static int PlanetShader { get; private set; }

		static Point crosshair1, crosshair2;

		/// <summary>
		/// Initialization process.
		/// </summary>
		public static void Load()
		{
			Camera.Load();

			foreach (var name in FileManager.GetGraphShaderNames())
				ShaderManager.Add(name);

			SetDefaultShader("default");
			SetPlanetShader("planet");

			GL.ClearColor(Color4.Black);

			GL.Enable(EnableCap.Blend);
			GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.CullFace);
			GL.CullFace(CullFaceMode.Back);
			GL.LineWidth(2f);

			PointRenderable.Load();
			PlanetRenderable.Load();

			// Initialize the crosshair.
			crosshair1 = new Point(Vector3.Zero, Color4.Black, 0.012f);
			crosshair2 = new Point(new Vector3(0, 0, -0.000001f), Color4.White);

			Utils.CheckError("Load");
		}

		public static void SetDefaultShader(string file)
		{
			var newShader = ShaderManager.Fetch(file);

			if (newShader <= 0)
				throw new ShaderException(file);

			DefaultShader = newShader;
			DefaultManager = ShaderManager.FetchManager(DefaultShader);
		}

		public static void SetPlanetShader(string file)
		{
			var newShader = ShaderManager.Fetch(file);

			if (newShader <= 0)
				throw new ShaderException(file);

			PlanetShader = newShader;
			PlanetManager = ShaderManager.FetchManager(PlanetShader);
		}

		public static void RenderFrame()
		{
			Camera.CalculateMatrix();

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			GL.Enable(EnableCap.CullFace);
			GL.CullFace(CullFaceMode.Back);
			GL.LineWidth(2f);

			GL.UseProgram(PlanetShader);
			PlanetManager.Uniform();

			SimulationManager.Render();

			if (Settings.Points)
			{
				GL.UseProgram(DefaultShader);
				DefaultManager.Uniform();

				PointManager.Render();

				DefaultManager.UniformProjection(Camera.ScaleMatrix);
				crosshair1.Render();
				crosshair2.Render();
			}

			Utils.CheckError("Render");
		}

		public static void ResizeViewport(int width, int height)
		{
			GL.Viewport(0, 0, width, height);
			GL.Scissor(0, 0, width, height);

			Camera.ResizeViewport(width, height);

			Utils.CheckError("Resize");
		}

		/// <summary>
		/// Used source: https://stackoverflow.com/questions/5844858/how-to-take-screenshot-in-opengl
		/// </summary>
		public static void TakeScreenshot(int x, int y, int width, int height)
		{
			var data = new byte[width * height * 3];
			GL.ReadPixels(x, y, width, height, PixelFormat.Bgr, PixelType.UnsignedByte, data);

			Utils.CheckError("Screenshot");

			FileManager.SaveScreenshot(data, width, height);
		}

		public static void Dispose()
		{
			PointManager.Dispose();
			PointRenderable.Dispose();
			PlanetRenderable.Dispose();

			ShaderManager.Dispose();
		}
	}
}
