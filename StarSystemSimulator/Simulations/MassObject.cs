using System.Collections.Generic;
using OpenTK.Mathematics;
using StarSystemSimulator.Graphics;

namespace StarSystemSimulator
{
	public class MassObject
	{
		public readonly string Name;
		readonly float size;
		float sizeBasedOnMass => (float)Mass;

		readonly Vector3[] lastLocations = new Vector3[150];

		public Color4 Color;
		public float EmissionStrength;

		// planets are defined in following units
		// mass: SOL_MASS
		// location: AU
		// velocity: AU
		public readonly double Mass;

		public Vector3 Location
		{
			get => location;
			set { location = value / Constants.Dist_Unit; CalculateMatrix(); }
		}
		Vector3 location;
		public Vector3 Velocity
		{
			get => velocity;
			set => velocity = value / Constants.Dist_Unit;
		}
		Vector3 velocity;
		public Vector3 Acceleration
		{
			get => acceleration;
			set => acceleration = value / Constants.Dist_Unit;
		}
		Vector3 acceleration;

		Matrix4 objectMatrix;

		public MassObject(double mass, float size = 1f, string name = null)
		{
			Mass = mass / Constants.Mass_Unit;

			this.size = size;
			Name = name;

			CalculateMatrix();
		}

		public void CalculateStep(float timeStep, List<MassObject> objects)
		{
			// LEAPFROG
			calculateLocation(timeStep / 2f);
			calculateAcceleration(objects);
			calculateVelocity(timeStep);
			calculateLocation(timeStep / 2f);
		}

		void calculateAcceleration(List<MassObject> objects)
		{
			acceleration = Vector3.Zero;
			foreach(var obj in objects)
			{
				if (obj == this)
					continue;

				var diff = location - obj.Location;
				var dist = diff.Length;
				var gravitation = (float)(obj.Mass * Constants.GConst / (dist * dist * dist));

				if (gravitation < 0.000001f)
					continue;

				acceleration += diff * -gravitation;
			}
		}

		void calculateVelocity(float timestep)
		{
			velocity += acceleration * timestep;
		}

		void calculateLocation(float timestep)
		{
			location += velocity * timestep;
			CalculateMatrix();

			updateLastLocations();
		}

		void updateLastLocations()
		{
			for (int i = lastLocations.Length -1; i >= 0; i--)
			{
				if (i == 0)
				{
					lastLocations[i] = Location;
					continue;
				}

				if (lastLocations[i - 1] == Vector3.Zero)
					continue;

				lastLocations[i] = lastLocations[i - 1];
			}
		}

		public void CalculateMatrix()
		{
			var scale = Matrix4.CreateScale((Settings.SizeBasedOnMass ? sizeBasedOnMass : size) * Settings.ObjectScaleFactor);
			//var rotX = Matrix4.CreateRotationX(Velocity.X);
			//var rotY = Matrix4.CreateRotationX(Velocity.X);
			//var rotZ = Matrix4.CreateRotationX(Velocity.X);
			var loc = Matrix4.CreateTranslation(Location);

			objectMatrix = scale /** rotX * rotY * rotZ*/ * loc;
		}

		public void Render()
		{
			MasterRenderer.PlanetManager.UniformModelView(objectMatrix);
			MasterRenderer.PlanetManager.UniformColor(Color);
			MasterRenderer.PlanetManager.UniformEmissionStrength(Settings.HighlightAll ? 1f : EmissionStrength);
			PlanetRenderable.Render();
		}

		public override string ToString()
		{
			return $"{Name}: Mass {Mass}, Location {Location}, Velocity {Velocity}";
		}
	}
}
