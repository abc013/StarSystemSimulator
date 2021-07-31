using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using StarSystemSimulator.Scripting;

namespace StarSystemSimulator.Simulations
{
	public class Simulation
	{
		public readonly List<MassObject> Objects = new List<MassObject>();
		public MassObject FollowedObject;

		// TIME in YEARS
		public float CurrentTime { get; private set; }

		readonly LuaScriptWrapper wrapper;

		bool disposed;

		public Simulation(string file)
		{
			wrapper = new LuaScriptWrapper(this, file);

			Load();
		}

		public void Load()
		{
			wrapper.Load();
		}

		public void Tick()
		{
			if (Settings.Paused || disposed)
				return;

			wrapper.UpdateState(this);
			wrapper.Tick();

			CurrentTime += Settings.TimeStep;

			foreach (var obj in Objects)
				obj.CalculateStep(Settings.TimeStep, Objects);
		}

		public void Render()
		{
			if (disposed)
				return;

			foreach (var obj in Objects)
				obj.Render();
		}

		public void Dispose()
		{
			if (disposed)
				return;

			disposed = true;

			wrapper.Dispose();
		}

		[LuaFunction("AddObject")]
		public MassObject AddObject(double mass, float size, string name, Color4 color = default, Vector3 location = default, Vector3 velocity = default, Vector3 acceleration = default)
		{
			var @object = new MassObject(mass, size, name)
			{
				Color = color,
				Location = location,
				Velocity = velocity,
				Acceleration = acceleration
			};

			Objects.Add(@object);

			return @object;
		}

		[LuaFunction("FindObject")]
		public MassObject FindObject(string name) => Objects.Find(o => o.Name == name);

		[LuaFunction("RemoveObject")]
		public void RemoveObject(MassObject @object) => Objects.Remove(@object);
	}
}
