using System.Collections.Generic;
using System.IO;

namespace StarSystemSimulator.Simulations
{
	public static class Time
	{
		const string file = "time";
		static readonly Dictionary<string, float> dictionary = new Dictionary<string, float>();

		static Time()
		{
			foreach ((var key, var dataValue) in FileManager.LoadNameValuePairs(file))
			{
				if (!float.TryParse(dataValue, out var value))
					throw new InvalidDataException($"Invalid data line: {key + dataValue} in file {file}");

				dictionary.Add(key, value);
			}
		}

		public static float Get(string name)
		{
			return dictionary[name];
		}
	}
}
