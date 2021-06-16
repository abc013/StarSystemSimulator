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

			SetShader("default", true);
			SetShader("default");

			GL.ClearColor(Color4.Black);

			GL.Enable(EnableCap.Blend);
			GL.Enable(EnableCap.DepthTest);
			GL.LineWidth(2f);

			PointRenderable.Load();
			PlanetRenderable.Load();

			// Initialize the crosshair.
			crosshair1 = new Point(Vector3.Zero, Color4.Black, 0.012f);
			crosshair2 = new Point(new Vector3(0, 0, -0.000001f), Color4.White);

			Utils.CheckError("Load");
		}

		public static void SetPolygonMode(PolygonMode mode)
		{
			GL.PolygonMode(MaterialFace.FrontAndBack, (OpenTK.Graphics.OpenGL.PolygonMode)mode);
		}

		public static void SetShader(string name, bool @default = false)
		{
			var newShader = ShaderManager.Fetch(name);
			if (newShader > 0)
			{
				if (@default)
				{
					DefaultShader = newShader;
					DefaultManager = ShaderManager.FetchManager(DefaultShader);
				}
				else
				{
					PlanetShader = newShader;
					PlanetManager = ShaderManager.FetchManager(PlanetShader);
				}
			}
			else
			{
				Log.WriteInfo($"Failed to fetch shader {name}.");

				if (@default)
					throw new DefaultShaderException();
			}
		}

		public static void RenderFrame()
		{
			Camera.CalculateMatrix();

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

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
