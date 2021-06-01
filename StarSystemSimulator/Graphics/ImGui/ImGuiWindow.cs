using ImGuiNET;
using OpenTK.Mathematics;
using StarSystemSimulator.Simulations;
using System.Collections.Generic;

namespace StarSystemSimulator.Graphics
{
	public class ImGuiWindow
	{
		readonly GraphWindow window;
		readonly ImGuiController controller;

		int localTick;
		bool firstTick = true;
		bool showDialog;

		readonly Queue<float> lastMS = new Queue<float>();
		readonly Queue<float> lastVertexBufferSize = new Queue<float>();
		readonly Queue<float> lastIndexBufferSize = new Queue<float>();

		public ImGuiWindow(GraphWindow window, ImGuiController controller)
		{
			this.window = window;
			this.controller = controller;

			showDialog = Settings.ShowWelcomeDialog;
		}

		public void ShowWindow(long lastms, Vector3d cursorLocation)
		{
			if (firstTick && Settings.AutoResizeWindow)
			{
				ImGui.SetNextWindowPos(System.Numerics.Vector2.Zero);
				ImGui.SetNextWindowSize(new System.Numerics.Vector2(window.ClientSize.X / (Settings.UIScaling * 4), window.ClientSize.Y / Settings.UIScaling));
				firstTick = false;
			}

			ImGui.Begin("Information Window");
			ImGui.Text($"Current year: {SimulationManager.CurrentTime}");
			if (ImGui.Button((Settings.Paused ? "Resume" : "Pause"), new System.Numerics.Vector2(ImGui.GetWindowContentRegionWidth(), 20)))
				Settings.Paused = !Settings.Paused;

			ImGui.Spacing();

			if (ImGui.CollapsingHeader("Mass object settings"))
			{
				ImGui.Checkbox("Draw trails", ref Settings.DrawTrails);

				var before = Settings.SizeBasedOnMass;

				ImGui.Text("Make object size based on mass");
				if (ImGui.Checkbox("Mass-size", ref Settings.SizeBasedOnMass))
					SimulationManager.SizeSettingsChanged();

				ImGui.Text("Object size scaling factor");
				if (ImGui.SliderFloat("Size", ref Settings.ObjectScaleFator, 0.01f, 10f, "%.2f"))
					SimulationManager.SizeSettingsChanged();

				if (ImGui.TreeNode("Objects"))
				{
					var objects = SimulationManager.GetObjects();
					var pointsToRemove = new List<MassObject>();
					for (int i = 0; i < objects.Count; i++)
					{
						var obj = objects[i];
						if (ImGui.TreeNode(obj.Name ?? $"Object {i}"))
						{
							ImGui.Text($"Mass: {obj.Mass} Sun masses");
							ImGui.Text($"Location");
							var loc = new System.Numerics.Vector3(obj.Location.X, obj.Location.Y, obj.Location.Z);
							if (ImGui.InputFloat3("Loc", ref loc, "%.3f"))
								obj.Location = new Vector3(loc.X, loc.Y, loc.Z);

							ImGui.Text($"Velocity");
							var vel = new System.Numerics.Vector3(obj.Velocity.X, obj.Velocity.Y, obj.Velocity.Z);
							if (ImGui.InputFloat3("Vel", ref vel, "%.3f"))
								obj.Velocity = new Vector3(vel.X, vel.Y, vel.Z);

							ImGui.Text($"Acceleration");
							var acc = new System.Numerics.Vector3(obj.Acceleration.X, obj.Acceleration.Y, obj.Acceleration.Z);
							if (ImGui.InputFloat3("Acc", ref acc, "%.3f"))
								obj.Acceleration = new Vector3(acc.X, acc.Y, acc.Z);

							ImGui.Text($"Color");
							var c = new System.Numerics.Vector4(obj.Color.R, obj.Color.G, obj.Color.B, obj.Color.A);
							if (ImGui.ColorEdit4("Col", ref c))
								obj.Color = new Color4(c.X, c.Y, c.Z, c.W);

							if (ImGui.Button("Jump"))
							{
								Camera.SetTranslation(obj.Location.X, obj.Location.Y, obj.Location.Z);
								SimulationManager.ClearFollowObject();
							}

							ImGui.SameLine();
							ImGui.PushStyleColor(ImGuiCol.Button, new System.Numerics.Vector4(0, .5f, 0, 1));
							if (ImGui.Button("Follow"))
								SimulationManager.FollowObject(obj);
							ImGui.PopStyleColor();

							ImGui.SameLine();
							ImGui.PushStyleColor(ImGuiCol.Button, new System.Numerics.Vector4(1, 0, 0, 1));
							if (ImGui.Button("Remove"))
								pointsToRemove.Add(obj);
							ImGui.PopStyleColor();

							ImGui.TreePop();
						}
					}

					if (pointsToRemove.Count != 0)
						objects.RemoveAll(p => pointsToRemove.Contains(p));

					ImGui.TreePop();
				}
			}
			if (ImGui.CollapsingHeader("Simulation settings"))
			{
				ImGui.Text("Simulation Speed");
				helpButton("Determines how fast the simulation is.");
				ImGui.SliderFloat("S-Speed", ref Settings.TimeStep, 0.00001f, .01f, "%.5f");
			}
			if (ImGui.CollapsingHeader("Point settings"))
			{
				ImGui.Checkbox("Show points", ref Settings.Points);
				if (ImGui.TreeNode("Points"))
				{
					var points = PointManager.Points;
					var pointsToRemove = new List<Point>();
					for (int i = 0; i < points.Count; i++)
					{
						var point = points[i];
						if (ImGui.TreeNode($"Point{i}"))
						{
							ImGui.Text($"X: {point.Position.X}");
							ImGui.SameLine();
							ImGui.Text($"Y: {point.Position.Y}");

							if (ImGui.Button("Copy to clipboard"))
								ImGui.SetClipboardText($"{point.Position.X},{point.Position.Y}");

							var c = new System.Numerics.Vector4(point.Color.R, point.Color.G, point.Color.B, point.Color.A);
							if (ImGui.ColorEdit4("", ref c))
								point.Color = new Color4(c.X, c.Y, c.Z, c.W);

							if (ImGui.Button("Remove"))
								pointsToRemove.Add(point);
							ImGui.TreePop();
						}
					}
					ImGui.TreePop();

					if (pointsToRemove.Count != 0)
						points.RemoveAll(p => pointsToRemove.Contains(p));
				}
				ImGui.TextWrapped("Place points by clicking the right mouse button or using the button.");
				ImGui.Checkbox("Use random color", ref Settings.UseRandomColor);
				if (!Settings.UseRandomColor)
				{
					ImGui.NewLine();
					var c = new System.Numerics.Vector4(Utils.StandardColor.R, Utils.StandardColor.G, Utils.StandardColor.B, Utils.StandardColor.A);
					if (ImGui.ColorPicker4("Color", ref c))
						Utils.StandardColor = new Color4(c.X, c.Y, c.Z, c.W);
				}
			}
			if (ImGui.CollapsingHeader("Viewport settings"))
			{
				var posChanged = false;
				var x = Camera.Location.X;
				var y = Camera.Location.Y;

				ImGui.Text("Location");
				helpButton("Hotkeys: [Arrows]");

				posChanged |= ImGui.InputFloat("X", ref x, Camera.RelativeSpeed, Camera.RelativeSpeed * 5);
				posChanged |= ImGui.InputFloat("Y", ref y, Camera.RelativeSpeed, Camera.RelativeSpeed * 5);

				if (posChanged)
				{
					Camera.SetTranslation(x, y, 0);
					SimulationManager.ClearFollowObject();
				}

				var s = Camera.Scale;

				ImGui.Text("Scale");

				if (ImGui.InputFloat("S", ref s, .1f, .2f))
					Camera.SetScale(s);

				ImGui.NewLine();
				ImGui.Text("Camera Speed");
				helpButton("Determines how fast the viewport is moved around by pressing keys.");
				ImGui.SliderFloat("C-Speed", ref Settings.CameraSpeed, 0.001f, .02f, "%.3f");
			}
		
			lastMS.Enqueue(lastms);
			if (lastMS.Count > 300)
				lastMS.Dequeue();

			if (controller.BufferChanged)
			{
				lastVertexBufferSize.Enqueue(controller.VertexBufferSize);
				lastIndexBufferSize.Enqueue(controller.IndexBufferSize);
			}

			if (ImGui.CollapsingHeader("Debug"))
			{
				ImGui.Text($"current: {localTick++} ticks");
				ImGui.Text($"render: {lastms} ms");

				var array = lastMS.ToArray();
				ImGui.PlotLines("graph", ref array[0], array.Length);

				ImGui.Text("buffer history");
				var array2 = lastVertexBufferSize.ToArray();
				ImGui.PlotHistogram("vertex", ref array2[0], array2.Length);

				var array3 = lastIndexBufferSize.ToArray();
				ImGui.PlotHistogram("index", ref array3[0], array3.Length);

				if (ImGui.Button("Show welcome dialog", new System.Numerics.Vector2(ImGui.GetWindowContentRegionWidth(), 20)))
					showDialog = true;
			}

			ImGui.NewLine();
			if (ImGui.Button("Add Point at current position", new System.Numerics.Vector2(ImGui.GetWindowContentRegionWidth(), 20)))
				PointManager.Add(Camera.Location, Utils.GetColor());

			if (ImGui.Button("Take Screenshot", new System.Numerics.Vector2(ImGui.GetWindowContentRegionWidth(), 20)))
				window.DoScreenshot();

			ImGui.Checkbox("Show UI in Screenshot", ref Settings.ScreenshotUI);

			ImGui.NewLine();
			const string str = "00.000000000000";
			ImGui.Text($"cursor at\n{cursorLocation.X.ToString(str)}\n{cursorLocation.Y.ToString(str)}");

			ImGui.End();

			if (showDialog)
			{
				ImGui.SetNextWindowPos(new System.Numerics.Vector2(window.ClientSize.X / (Settings.UIScaling * 2) - 150, window.ClientSize.Y / (Settings.UIScaling * 2) - 110));
				ImGui.SetNextWindowSize(new System.Numerics.Vector2(300, 220));
				ImGui.Begin("Welcome!", ImGuiWindowFlags.NoDecoration);
				ImGui.Text("Welcome to StarSystemSimulator!");
				ImGui.TextWrapped("Click on the headers to open/close their contents.");
				ImGui.TextWrapped("You can get more information about the parameters when hovering above this symbol: ");
				ImGui.Text("Parameter");
				helpButton("Parameter information!");
				ImGui.TextWrapped("The information window can be resized by clicking and dragging the lower right corner. It can be closed by clicking on the arrow next to the title.");
				ImGui.Text("Enjoy!");
				if (ImGui.Button("Close this window", new System.Numerics.Vector2(ImGui.GetWindowContentRegionWidth(), 20)))
					showDialog = false;
				ImGui.End();
			}
		}

		static void helpButton(string description)
		{
			ImGui.SameLine();
			ImGui.TextDisabled("[?]");
			if (ImGui.IsItemHovered())
				ImGui.SetTooltip(description);
		}
	}
}
