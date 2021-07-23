using StarSystemSimulator.Graphics;
using ImGuiNET;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.ComponentModel;
using System.Diagnostics;
using StarSystemSimulator.Simulations;

namespace StarSystemSimulator
{
	/// <summary>
	/// Graph window supported by OpenTK.
	/// </summary>
	public class GraphWindow : GameWindow
	{
		public bool IsClosing { get; private set; }
		public bool IsLoaded { get; private set; }

		/// <summary>
		/// If set to true, a screenshot will be taken next or in this frame.
		/// </summary>
		bool screenshot;

		/// <summary>
		/// Stopwatch used for measuring the time each render frame needs.
		/// </summary>
		readonly Stopwatch watch;

		/// <summary>
		/// ImGui controller that is used to handle the ImGui windows.
		/// </summary>
		ImGuiController controller;
		/// <summary>
		/// ImGui window that contains some window states and controls what is drawn.
		/// </summary>
		ImGuiWindow window;

		/// <summary>
		/// Initialize the graph window. This calls a base constructor in OpenTK which handles window creation for us.
		/// </summary>
		public GraphWindow(GameWindowSettings gameSettings, NativeWindowSettings nativeSettings) : base(gameSettings, nativeSettings)
		{
			watch = new Stopwatch();

			// Initialize values
			int maxWidth, maxHeight;
			unsafe
			{
				var mode = GLFW.GetVideoMode(CurrentMonitor.ToUnsafePtr<Monitor>());
				maxWidth = mode->Width;
				maxHeight = mode->Height;
			}

			if (Settings.GraphWidth > maxWidth)
				Settings.GraphWidth = maxWidth;
			if (Settings.GraphHeight > maxHeight)
				Settings.GraphHeight = maxHeight;
		}

		/// <summary>
		/// Signals that the window is now loaded, which means we can load our graphics now.
		/// </summary>
		protected override void OnLoad()
		{
			base.OnLoad();
			MasterRenderer.Load();
			SimulationManager.Load();

			if (Settings.UseSystemUIScaling)
			{
				if (TryGetCurrentMonitorDpi(out float hdpi, out _))
					Settings.UIScaling = hdpi / 100;
				else
					Log.WriteInfo("Failed to fetch system dpi scaling.");
			}

			controller = new ImGuiController(ClientSize.X, ClientSize.Y);
			controller.SetScale(Settings.UIScaling);
			window = new ImGuiWindow(this, controller);

			var minimum = ClientRectangle.Min;
			ClientRectangle = new Box2i(minimum.X, minimum.Y, Settings.GraphWidth + minimum.X, Settings.GraphHeight + minimum.Y);

			IsLoaded = true;
		}

		long lastrenderms;
		//Vector3 cursorLocation;

		/// <summary>
		/// Render frame, which renders the whole window.
		/// </summary>
		protected override void OnRenderFrame(FrameEventArgs args)
		{
			watch.Start();

			//if (Camera.Changed)
			//	cursorLocation = getCursorLocation();

			base.OnRenderFrame(args);
			MasterRenderer.RenderFrame();

			window.ShowWindow(lastrenderms, lasttickms);

			// Without UI, since it isn't rendered yet
			if (screenshot && !Settings.ScreenshotUI)
				takeScreenshot();

			controller.Render();

			// With UI, since it is rendered now
			if (screenshot && Settings.ScreenshotUI)
				takeScreenshot();

			SwapBuffers();

			lastrenderms = watch.ElapsedMilliseconds;
			watch.Reset();
		}

		long lasttickms;

		/// <summary>
		/// Update frame, checking for any key movements.
		/// </summary>
		protected override void OnUpdateFrame(FrameEventArgs args)
		{
			watch.Start();

			base.OnUpdateFrame(args);

			controller.Update(this, (float)args.Time);
			SimulationManager.Update();

			if (ImGui.IsAnyItemActive())
				return;

			var x = checkKeyRegulator(Keys.Right, Keys.Left);
			var y = checkKeyRegulator(Keys.Up, Keys.Down);

			if (x != 0f || y != 0f)
			{
				Camera.Translate(x, y, 0);
				SimulationManager.ClearFollowObject();
			}

			lasttickms = watch.ElapsedMilliseconds;
			watch.Reset();
		}

		/// <summary>
		/// Checks whether two keys are pressed and determines a value based on it.
		/// </summary>
		/// <returns>if <c>up</c> is pressed, 1. if <c>down</c> is pressed, -1. if both or none are pressed, 0.</returns>
		int checkKeyRegulator(Keys up, Keys down)
		{
			var delta = 0;

			if (KeyboardState.IsKeyDown(up))
				delta++;
			if (KeyboardState.IsKeyDown(down))
				delta--;

			return delta;
		}

		/// <summary>
		/// Tells that this or next frame, a screenshot has to be taken.
		/// </summary>
		public void DoScreenshot()
		{
			screenshot = true;
		}

		/// <summary>
		/// Takes a screenshot.
		/// </summary>
		void takeScreenshot()
		{
			MasterRenderer.TakeScreenshot(0, 0, ClientSize.X, ClientSize.Y);
			screenshot = false;
		}

		/// <summary>
		/// Adds a point when the mouse is getting clicked on the right button.
		/// Moves to the viewport center to the cursor location when the left button is clicked.
		/// </summary>
		protected override void OnMouseDown(MouseButtonEventArgs e)
		{
			if (ImGui.IsWindowHovered(ImGuiHoveredFlags.AnyWindow))
				return;

			// TODO: does not work anymore!
			/*
			if (e.Button == MouseButton.Right)
				PointManager.Add(cursorLocation, Utils.GetColor());
			else if (e.Button == MouseButton.Left)
			{
				Camera.SetTranslation(cursorLocation.X, cursorLocation.Y, cursorLocation.Z);
				SimulationManager.ClearFollowObject();

				MousePosition = new Vector2(Bounds.HalfSize.X, Bounds.HalfSize.Y);
			}*/
		}


		/// <summary>
		/// Updates the cursor location.
		/// </summary>
		protected override void OnMouseMove(MouseMoveEventArgs e)
		{
			if (ImGui.IsWindowFocused(ImGuiFocusedFlags.AnyWindow))
				return;

			if (MouseState.IsButtonDown(MouseButton.Left))
			{
				var dx = e.DeltaX;
				var dy = e.DeltaY;

				Camera.Rotate(dx, dy);
			}
			//cursorLocation = getCursorLocation();
		}

		// TODO does not work anymore !!
		/*
		/// <summary>
		/// Calculates the cursor location from screen to virtual space.
		/// </summary>
		Vector3 getCursorLocation()
		{
			var screenX = (MousePosition.X / ClientSize.X) * 4 - 2;
			var screenY = (MousePosition.Y / ClientSize.Y) * 4 - 2;
			screenX *= Camera.Ratio;
			screenX += Camera.Location.X;
			screenY += Camera.Location.Y;

			return new Vector3(screenX, screenY, 0);
		}*/

		/// <summary>
		/// Scales when the mouse wheel is used.
		/// </summary>
		protected override void OnMouseWheel(MouseWheelEventArgs e)
		{
			if (ImGui.IsWindowHovered(ImGuiHoveredFlags.AnyWindow))
				return;

			Camera.Zoom(e.OffsetY);
		}

		protected override void OnTextInput(TextInputEventArgs e)
		{
			foreach (var c in e.AsString)
				controller.PressChar(c);
		}

		protected override void OnKeyDown(KeyboardKeyEventArgs e)
		{
			if (ImGui.IsAnyItemActive())
				return;

			if (e.Key == Keys.Space)
				Settings.Paused = !Settings.Paused;
		}

		/// <summary>
		/// Updates the MasterRenderer when the viewport is being resized.
		/// </summary>
		protected override void OnResize(ResizeEventArgs e)
		{
			MasterRenderer.ResizeViewport(e.Width, e.Height);
			controller.WindowResized(e.Width, e.Height);
		}

		/// <summary>
		/// Closes the pipe and the MasterRenderer.
		/// </summary>
		protected override void OnClosing(CancelEventArgs e)
		{
			IsClosing = true;

			MasterRenderer.Dispose();
			controller.Dispose();
			SimulationManager.Dispose();
		}
	}
}
