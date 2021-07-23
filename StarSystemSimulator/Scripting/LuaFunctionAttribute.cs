using System;

namespace StarSystemSimulator.Scripting
{
	[AttributeUsage(AttributeTargets.Method)]
	public class LuaFunctionAttribute : Attribute
	{
		public readonly string FunctionName;

		public LuaFunctionAttribute(string functionName)
		{
			FunctionName = functionName;
		}
	}
}
