using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace StarSystemSimulator.Simulations
{
	public class Simulation
	{
		public readonly List<MassObject> Objects = new List<MassObject>();
		public MassObject FollowedObject;

		// TIME in YEARS
		public float CurrentTime { get; private set; }

		public Simulation()
		{
			Objects.AddRange(asteroids());
		}

		public void Tick()
		{
			if (Settings.Paused)
				return;

			CurrentTime += Settings.TimeStep;

			foreach (var obj in Objects)
				obj.CalculateStep(Settings.TimeStep, Objects);
		}

		public void Render()
		{
			foreach (var obj in Objects)
				obj.Render();
		}

#pragma warning disable IDE0051
		static List<MassObject> sunSystem()
		{
			return new List<MassObject>
			{
				new MassObject(Mass.Get("sun") * 1.4, .03f, "Distractor")
				{
					Color = Color4.WhiteSmoke,
					Location = new Vector3(150 * Distance.Get("au"), -100 * Distance.Get("au"), 0),
					Velocity = new Vector3(-4 * Distance.Get("au"), 2 * Distance.Get("au"), 0)
				},
				new MassObject(Mass.Get("sun"), .07f, "Sun")
				{
					Color = Color4.LightYellow
				},
				new MassObject(Mass.Get("earth"), .02f, "Earth")
				{
					Color = Color4.BlueViolet,
					Location = new Vector3(Distance.Get("au"), 0, 0),
					Velocity = new Vector3(0, 2f * MathF.PI * Distance.Get("au"), 0)
				},
				new MassObject(Mass.Get("mars"), .01f, "Mars")
				{
					Color = Color4.IndianRed,
					Location = new Vector3(Distance.Get("mars"), 0, 0),
					Velocity = new Vector3(0, (-2f * MathF.PI * Distance.Get("mars")) / Time.Get("mars_revolution"), 0)
				},
				new MassObject(Mass.Get("jupiter"), .05f, "Jupiter")
				{
					Color = Color4.Firebrick,
					Location = new Vector3(0, -Distance.Get("jupiter"), 0),
					Velocity = new Vector3((2f * MathF.PI * Distance.Get("jupiter")) / Time.Get("jupiter_revolution"), 0, 0)
				},
				new MassObject(Mass.Get("saturn"), .04f, "Saturn")
				{
					Color = Color4.LightGray,
					Location = new Vector3(0, Distance.Get("saturn"), 0),
					Velocity = new Vector3((-2f * MathF.PI * Distance.Get("saturn")) / Time.Get("saturn_revolution"), 0, 0)
				}
			};
		}

		static List<MassObject> asteroids(int count = 300)
		{
			var list = new List<MassObject>
			{
				new MassObject(Mass.Get("sun") * 1.4, .5f, "Distractor")
				{
					Color = Color4.WhiteSmoke,
					Location = new Vector3(75 * Distance.Get("au"), -50 * Distance.Get("au"), 0),
					Velocity = new Vector3(-4 * Distance.Get("au"), 3 * Distance.Get("au"), 0)
				},
				new MassObject(Mass.Get("sun"), .4f, "Sun")
				{
					Color = Color4.Yellow
				}
			};

			var random = new Random();

			var m = 0.005f * Mass.Get("moon");
			for (int i = 0; i < count; i++)
			{
				const float fullAngle = 2 * MathF.PI;
				var angle = (float)(random.NextDouble() * fullAngle);

				var dist = Distance.Get("jupiter") + (random.Next(200) - 100) / 200f * Distance.Get("au");
				var x = MathF.Sin(angle) * dist;
				var y = MathF.Cos(angle) * dist;

				var revolution = (fullAngle * dist) / (Time.Get("jupiter_revolution") * dist / Distance.Get("jupiter"));
				var xSpeed = MathF.Sin(angle + fullAngle / 4) * revolution;
				var ySpeed = MathF.Cos(angle + fullAngle / 4) * revolution;

				list.Add(new MassObject(m, 4 / 100f, $"Rock {i}")
				{
					Color = Color4.DarkGray,
					Location = new Vector3(x, y, 0),
					Velocity = new Vector3(xSpeed, ySpeed, 0)
				});
			}

			for (int i = 0; i < count; i++)
			{
				const float fullAngle = 2 * MathF.PI;
				var angle = (float)(random.NextDouble() * fullAngle);

				var dist = Distance.Get("jupiter") + (random.Next(200) - 100) / 200f * Distance.Get("au");
				var y = MathF.Sin(angle) * dist;
				var z = MathF.Cos(angle) * dist;

				var revolution = (fullAngle * dist) / (Time.Get("jupiter_revolution") * dist / Distance.Get("jupiter"));
				var ySpeed = MathF.Sin(angle + fullAngle / 4) * revolution;
				var zSpeed = MathF.Cos(angle + fullAngle / 4) * revolution;

				list.Add(new MassObject(m, 4 / 100f, $"Rock {i}")
				{
					Color = Color4.DarkGray,
					Location = new Vector3(0, y, z),
					Velocity = new Vector3(0, ySpeed, zSpeed)
				});
			}

			return list;
		}

		static List<MassObject> binarySystem()
		{
			return new List<MassObject>
			{
				new MassObject(Mass.Get("sun"), .04f, "Sun A")
				{
					Color = Color4.Yellow,
					Location = new Vector3(-Distance.Get("au"), 0, 0),
					Velocity = new Vector3(0, -2f * Distance.Get("au"), 0)
				},
				new MassObject(Mass.Get("sun"), .04f, "Sun B")
				{
					Color = Color4.Red,
					Location = new Vector3(Distance.Get("au"), 0, 0),
					Velocity = new Vector3(0, 2f * Distance.Get("au"), 0)
				},
				new MassObject(Mass.Get("earth"), .02f, "Planet")
				{
					Color = Color4.CornflowerBlue,
					Location = new Vector3(0, 4 * Distance.Get("au"), 0),
					Velocity = new Vector3((6 * MathF.PI * Distance.Get("au")) / 4, 0, 0)
				}
			};
		}

		static List<MassObject> fiveStarSystem()
		{
			return new List<MassObject>
			{
				new MassObject(Mass.Get("sun") * 0.5f, .04f, "Sun A")
				{
					Color = Color4.Yellow,
					Location = new Vector3(-3 * Distance.Get("au"), 0, 0),
					Velocity = new Vector3(0, -(2f * MathF.PI * Distance.Get("au")) / 3, 0)
				},
				new MassObject(Mass.Get("sun") * 0.5f, .04f, "Sun B")
				{
					Color = Color4.Red,
					Location = new Vector3(3 * Distance.Get("au"), 0, 0),
					Velocity = new Vector3(0, (2f * MathF.PI * Distance.Get("au")) / 3, 0)
				},
				new MassObject(Mass.Get("sun") * 0.5f, .04f, "Sun C")
				{
					Color = Color4.Gold,
					Location = new Vector3(0, 3f * Distance.Get("au"), 0),
					Velocity = new Vector3(-(2f * MathF.PI * Distance.Get("au")) / 3, 0, 0)
				},
				new MassObject(Mass.Get("sun") * 0.5f, .04f, "Sun D")
				{
					Color = Color4.OrangeRed,
					Location = new Vector3(0, -3f * Distance.Get("au"), 0),
					Velocity = new Vector3((2f * MathF.PI * Distance.Get("au")) / 3, 0, 0)
				},
				new MassObject(Mass.Get("sun") * 1.4, .06f, "Sun E")
				{
					Color = Color4.White
				}
			};
		}
		static List<MassObject> earthMoonSystem()
		{
			return new List<MassObject>
			{
				new MassObject(Mass.Get("earth"), .02f, "Earth")
				{
					Color = Color4.BlueViolet
				},
				new MassObject(Mass.Get("moon"), .01f, "Moon")
				{
					Color = Color4.DarkGray,
					Location = new Vector3(0, Distance.Get("earth_radius") + Distance.Get("moon_to_earth"), 0),
					Velocity = new Vector3((365f / 29f) * 2f * MathF.PI * (Distance.Get("earth_radius") + Distance.Get("moon_to_earth")), 0, 0)
				}
			};
		}

		static List<MassObject> strangeSystem()
		{
			return new List<MassObject>
			{
				new MassObject(Mass.Get("sun"), .04f, "Sun A")
				{
					Color = Color4.Yellow,
					Location = new Vector3(-Distance.Get("au"), 0, 0),
					Velocity = new Vector3(0, -2f * MathF.PI * Distance.Get("au"), 0)
				},
				new MassObject(Mass.Get("sun") * 1.4f, .05f, "Sun B")
				{
					Color = Color4.Red,
					Location = new Vector3(Distance.Get("au"), 0, 0),
					Velocity = new Vector3(0, 2f * MathF.PI * Distance.Get("au"), 0)
				},
				new MassObject(Mass.Get("earth"), .02f, "Earth 1")
				{
					Color = Color4.BlueViolet,
					Location = new Vector3(Distance.Get("au"), 0, 0),
					Velocity = new Vector3(0, 2f * MathF.PI * Distance.Get("au"), 0)
				},
				new MassObject(Mass.Get("earth") * 0.1f, .01f, "Small Earth")
				{
					Color = Color4.CornflowerBlue,
					Location = new Vector3(1 * Distance.Get("au"), 2 * Distance.Get("au"), 0),
					Velocity = new Vector3(Distance.Get("au") * MathF.PI, 0, 0)
				},
				new MassObject(Mass.Get("earth"), .02f, "Earth 2")
				{
					Color = Color4.BlueViolet,
					Velocity = new Vector3(0.3f * Distance.Get("au") * MathF.PI, 0, 0)
				},
				new MassObject(Mass.Get("moon"), .01f, "Moon")
				{
					Color = Color4.DarkGray,
					Location = new Vector3(0, Distance.Get("earth_radius") + Distance.Get("moon_to_earth"), 0),
					Velocity = new Vector3(2f * MathF.PI * MathF.PI * (Distance.Get("earth_radius") + Distance.Get("moon_to_earth")) / 365f, 0, 0)
				}
			};
		}
#pragma warning restore IDE0051
	}
}
