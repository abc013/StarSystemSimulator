using System.Collections.Generic;
using System.IO;

namespace StarSystemSimulator.Simulations
{
	public static class Distance
	{
		const string file = "distance";
		static readonly Dictionary<string, float> dictionary = new Dictionary<string, float>();

		static Distance()
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
