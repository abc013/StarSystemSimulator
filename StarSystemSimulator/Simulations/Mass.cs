using StarSystemSimulator.Scripting;
using System.Collections.Generic;
using System.IO;

namespace StarSystemSimulator.Simulations
{
	public static class Mass
	{
		const string file = "mass";
		static readonly Dictionary<string, double> dictionary = new Dictionary<string, double>();

		static Mass()
		{
			foreach ((var key, var dataValue) in FileManager.LoadNameValuePairs(file))
			{
				if (!double.TryParse(dataValue, out var value))
					throw new InvalidDataException($"Invalid data line: {key + '=' + dataValue} in file {file}");

				dictionary.Add(key, value);
			}
		}

		public static double Get(string name)
		{
			return dictionary[name];
		}

		public static void FillStates(LuaScriptWrapper wrapper)
		{
			foreach ((var key, var value) in dictionary)
				wrapper.UpdateSingleState($"Mass_{key}", value);
		}
	}
}
