using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace StarSystemSimulator
{
	/// <summary>
	/// Class that is responsible of all the IO activity going on.
	/// </summary>
	public static class FileManager
	{
		public static readonly char DirSeparator = Path.DirectorySeparatorChar;
		public static readonly string Current = Directory.GetCurrentDirectory() + DirSeparator;
		public static readonly string GraphShaders = Current + "Shaders" + DirSeparator;
		public static readonly string Palettes = Current + "Palettes" + DirSeparator;
		public static readonly string Screenhots = Current + "Screenshots" + DirSeparator;
		public static readonly string Constants = Current + "Constants" + DirSeparator;
		public static readonly string Systems = Current + "Systems" + DirSeparator;

		public static string CheckFile(string file)
		{
			var filePath = Current + file;

			if (File.Exists(filePath))
				return filePath;

			return string.Empty;
		}

		public static List<(string, string)> LoadNameValuePairs(string filename, string suffix = ".txt")
		{
			var result = new List<(string, string)>();

			var data = File.ReadAllLines(Constants + filename + suffix);

			foreach (var pair in data)
			{
				if (pair.StartsWith('#'))
					continue;

				if (string.IsNullOrWhiteSpace(pair))
					continue;

				var split = pair.Split('=');

				if (split.Length != 2)
					throw new InvalidDataException($"Invalid data line: {pair} in file {filename}");

				result.Add((split[0].Trim(), split[1].Trim()));
			}

			return result;
		}

		public static List<string> GetGraphShaderNames()
		{
			var files = Directory.GetFiles(GraphShaders).Where(f => f.EndsWith(".frag")).ToList();

			var results = new List<string>();

			foreach (var file in files)
			{
				var index = file.LastIndexOf('\\') + 1;
				var name = file.Substring(index, file.Length - index - 5);

				if (File.Exists(GraphShaders + name + ".vert"))
					results.Add(name);
			}

			return results;
		}

		public static List<string> GetScriptNames()
		{
			var files = Directory.GetFiles(Systems).Where(f => f.EndsWith(".lua")).ToList();

			var results = new List<string>();

			foreach (var file in files)
			{
				var index = file.LastIndexOf('\\') + 1;
				var name = file.Substring(index);

				if (name != "sandbox.lua" && name != "wrapper.lua")
					results.Add(name);
			}

			return results;
		}

		public static void SaveScreenshot(byte[] data, int width, int height)
		{
			if (!Directory.Exists(Screenhots))
				Directory.CreateDirectory(Screenhots);

			var file = Screenhots + "screenshot_" + DateTime.Now.ToString("HHmmss_ddMMyyyy") + ".png";

			using var img = new Bitmap(width, height, width * 3, System.Drawing.Imaging.PixelFormat.Format24bppRgb, Marshal.UnsafeAddrOfPinnedArrayElement(data, 0));

			img.RotateFlip(RotateFlipType.RotateNoneFlipY);
			img.Save(file);
		}
	}
}
