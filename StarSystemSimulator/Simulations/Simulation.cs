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
		const double mmoon = 7.342e25;				// Moon mass in g
		const double mearth = 5.97219e27;			// Earth mass in g
		const double msun = 1.989e33;				// Sun mass in g
		const double mmars = 6.419e26;				// Mars mass in g
		const double mjupiter = 1.8982e30;			// Jupiter mass in g
		const double msaturn = 1.8982e30;			// Saturn mass in g

		const float au = 1.495978707e13f;			// au in cm
		const float dmars = au * 1.524f;			// Mars dist to Sun in cm
		const float djupiter = au * 5.2044f;		// Jupiter dist to Sun in cm
		const float dsaturn = au * 9.5826f;			// Saturn dist to Sun in cm
		const float rearth = 6371e5f;				// Earth dameter in cm
		const float dmoon = 384400e5f;				// Distance earth to moon in cm
		const float tyear = 3.1556926e7f;			// year in seconds
		const float tjyear = 11.862f;				// jupiter year in years
		const float tsyear = 29.4571f;				// jupiter year in years
		const float tmyear = (686.971f / 365.25f);	// mars year in years

		static List<MassObject> sunSystem()
		{
			return new List<MassObject>
			{
				new MassObject(msun * 1.4, .03f, "Distractor")
				{
					Color = Color4.WhiteSmoke,
					Location = new Vector3(150 * au, -100 * au, 0),
					Velocity = new Vector3(-4 * au, 2 * au, 0)
				},
				new MassObject(msun, .07f, "Sun")
				{
					Color = Color4.LightYellow
				},
				new MassObject(mearth, .02f, "Earth")
				{
					Color = Color4.BlueViolet,
					Location = new Vector3(au, 0, 0),
					Velocity = new Vector3(0, 2f * MathF.PI * au, 0)
				},
				new MassObject(mmars, .01f, "Mars")
				{
					Color = Color4.IndianRed,
					Location = new Vector3(dmars, 0, 0),
					Velocity = new Vector3(0, (-2f * MathF.PI * dmars) / tmyear, 0)
				},
				new MassObject(mjupiter, .05f, "Jupiter")
				{
					Color = Color4.Firebrick,
					Location = new Vector3(0, -djupiter, 0),
					Velocity = new Vector3((2f * MathF.PI * djupiter) / tjyear, 0, 0)
				},
				new MassObject(msaturn, .04f, "Saturn")
				{
					Color = Color4.LightGray,
					Location = new Vector3(0, dsaturn, 0),
					Velocity = new Vector3((-2f * MathF.PI * dsaturn) / tsyear, 0, 0)
				}
			};
		}

		static List<MassObject> asteroids(int count = 300)
		{
			var list = new List<MassObject>
			{
				new MassObject(msun * 1.4, .03f, "Distractor")
				{
					Color = Color4.WhiteSmoke,
					Location = new Vector3(150 * au, -100 * au, 0),
					Velocity = new Vector3(-4 * au, 2 * au, 0)
				},
				new MassObject(msun, 40 / 100f, "Sun")
				{
					Color = Color4.Yellow
				}
			};

			var random = new Random();

			const double m = 0.005f * mmoon;
			for (int i = 0; i < count; i++)
			{
				const float fullAngle = 2 * MathF.PI;
				var angle = (float)(random.NextDouble() * fullAngle);

				var dist = djupiter + (random.Next(200) - 100) / 200f * au;
				var x = MathF.Sin(angle) * dist;
				var y = MathF.Cos(angle) * dist;

				var revolution = (fullAngle * dist) / (tjyear * dist / djupiter);
				var xSpeed = MathF.Sin(angle + fullAngle / 4) * revolution;
				var ySpeed = MathF.Cos(angle + fullAngle / 4) * revolution;

				list.Add(new MassObject(m, 4 / 100f, $"Rock {i}")
				{
					Color = Color4.DarkGray,
					Location = new Vector3(x, y, 0),
					Velocity = new Vector3(xSpeed, ySpeed, 0)
				});
			}

			return list;
		}

		static List<MassObject> binarySystem()
		{
			return new List<MassObject>
			{
				new MassObject(msun, .04f, "Sun A")
				{
					Color = Color4.Yellow,
					Location = new Vector3(-au, 0, 0),
					Velocity = new Vector3(0, -2f * au, 0)
				},
				new MassObject(msun, .04f, "Sun B")
				{
					Color = Color4.Red,
					Location = new Vector3(au, 0, 0),
					Velocity = new Vector3(0, 2f * au, 0)
				},
				new MassObject(mearth, .02f, "Planet")
				{
					Color = Color4.CornflowerBlue,
					Location = new Vector3(0, 4 * au, 0),
					Velocity = new Vector3((6 * MathF.PI * au) / 4, 0, 0)
				}
			};
		}

		static List<MassObject> fiveStarSystem()
		{
			return new List<MassObject>
			{
				new MassObject(msun * 0.5f, .04f, "Sun A")
				{
					Color = Color4.Yellow,
					Location = new Vector3(-3 * au, 0, 0),
					Velocity = new Vector3(0, -(2f * MathF.PI * au) / 3, 0)
				},
				new MassObject(msun * 0.5f, .04f, "Sun B")
				{
					Color = Color4.Red,
					Location = new Vector3(3 * au, 0, 0),
					Velocity = new Vector3(0, (2f * MathF.PI * au) / 3, 0)
				},
				new MassObject(msun * 0.5f, .04f, "Sun C")
				{
					Color = Color4.Gold,
					Location = new Vector3(0, 3f * au, 0),
					Velocity = new Vector3(-(2f * MathF.PI * au) / 3, 0, 0)
				},
				new MassObject(msun * 0.5f, .04f, "Sun D")
				{
					Color = Color4.OrangeRed,
					Location = new Vector3(0, -3f * au, 0),
					Velocity = new Vector3((2f * MathF.PI * au) / 3, 0, 0)
				},
				new MassObject(msun * 1.4, .06f, "Sun E")
				{
					Color = Color4.White
				}
			};
		}
		static List<MassObject> earthMoonSystem()
		{
			return new List<MassObject>
			{
				new MassObject(mearth, .02f, "Earth")
				{
					Color = Color4.BlueViolet
				},
				new MassObject(mmoon, .01f, "Moon")
				{
					Color = Color4.DarkGray,
					Location = new Vector3(0, rearth + dmoon, 0),
					Velocity = new Vector3((365f / 29f) * 2f * MathF.PI * (rearth + dmoon), 0, 0)
				}
			};
		}

		static List<MassObject> strangeSystem()
		{
			return new List<MassObject>
			{
				new MassObject(msun, .04f, "Sun A")
				{
					Color = Color4.Yellow,
					Location = new Vector3(-au, 0, 0),
					Velocity = new Vector3(0, -2f * MathF.PI * au, 0)
				},
				new MassObject(msun * 1.4f, .05f, "Sun B")
				{
					Color = Color4.Red,
					Location = new Vector3(au, 0, 0),
					Velocity = new Vector3(0, 2f * MathF.PI * au, 0)
				},
				new MassObject(mearth, .02f, "Earth 1")
				{
					Color = Color4.BlueViolet,
					Location = new Vector3(au, 0, 0),
					Velocity = new Vector3(0, 2f * MathF.PI * au, 0)
				},
				new MassObject(mearth * 0.1f, .01f, "Small Earth")
				{
					Color = Color4.CornflowerBlue,
					Location = new Vector3(1 * au, 2 * au, 0),
					Velocity = new Vector3(au * MathF.PI, 0, 0)
				},
				new MassObject(mearth, .02f, "Earth 2")
				{
					Color = Color4.BlueViolet,
					Velocity = new Vector3(0.3f * au * MathF.PI, 0, 0)
				},
				new MassObject(mmoon, .01f, "Moon")
				{
					Color = Color4.DarkGray,
					Location = new Vector3(0, rearth + dmoon, 0),
					Velocity = new Vector3(2f * MathF.PI * MathF.PI * (rearth + dmoon) / 365f, 0, 0)
				}
			};
		}
#pragma warning restore IDE0051
	}
}
