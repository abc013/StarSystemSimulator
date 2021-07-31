using StarSystemSimulator.Graphics;
using System.Collections.Generic;

namespace StarSystemSimulator.Simulations
{
	public static class SimulationManager
	{
		public static float CurrentTime => simulation.CurrentTime;

		static Simulation simulation;

		public static void Load(string system)
		{
			simulation?.Dispose();
			simulation = new Simulation(FileManager.Systems + system);
		}

		public static void Update()
		{
			simulation.Tick();

			if (simulation.FollowedObject != null)
			{
				var loc = simulation.FollowedObject.Location;
				Camera.SetTranslation(loc.X, loc.Y, loc.Z);
			}
		}

		public static void Render()
		{
			simulation.Render();
		}

		public static List<MassObject> GetObjects()
		{
			return simulation.Objects;
		}

		public static void SizeSettingsChanged()
		{
			foreach (var obj in simulation.Objects)
				obj.CalculateMatrix();
		}

		public static void ClearFollowObject() => FollowObject(null);

		public static void FollowObject(MassObject obj)
		{
			simulation.FollowedObject = obj;
		}

		public static void Dispose()
		{
			simulation.Dispose();
		}
	}
}
