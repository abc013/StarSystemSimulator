using NLua;
using StarSystemSimulator.Graphics;
using StarSystemSimulator.Simulations;
using System;
using System.Linq;

namespace StarSystemSimulator.Scripting
{
	public class LuaScriptWrapper
	{
		readonly Lua luaState;
		readonly LuaFunction tick;
		readonly LuaFunction register;
		
		public LuaScriptWrapper(Simulation simulation, string file)
		{
			luaState = new Lua();

			var wrapperFile = FileManager.Systems + "wrapper.lua";
			luaState.DoFile(wrapperFile);

			register = luaState["registerGlobal"] as LuaFunction;

			registerFunctions(typeof(LuaScriptWrapper));
			registerFunctions(typeof(LuaFunctions));
			registerFunctions(typeof(Camera));
			registerFunctions(typeof(Simulation), simulation);

			Distance.FillStates(this);
			Mass.FillStates(this);
			Time.FillStates(this);

			using var load = luaState["loadScript"] as LuaFunction;
			load.Call(System.IO.File.ReadAllText(file));

			tick = luaState["tick"] as LuaFunction;
		}

		void registerFunctions(Type type, object obj = null)
		{
			foreach (var method in type.GetMethods())
			{
				var attribute = method.GetCustomAttributes(false).FirstOrDefault(c => c is LuaFunctionAttribute);

				if (attribute != null)
				{
					var functionName = ((LuaFunctionAttribute)attribute).FunctionName;
					register.Call(functionName, luaState.RegisterFunction(functionName, obj, method));
				}
			}
		}

		public void Load()
		{
			using var init = luaState["init"] as LuaFunction;
			init.Call();
		}

		public void Tick()
		{
			tick.Call();
		}

		public void UpdateState(Simulation simulation)
		{
			UpdateSingleState("CurrentTime", simulation.CurrentTime);
			UpdateSingleState("FollowedObject", simulation.FollowedObject);
			UpdateSingleState("Objects", simulation.Objects);
		}

		public void UpdateSingleState(string name, object value)
		{
			register.Call(name, value);
		}

		public void Dispose()
		{
			tick.Dispose();
			register.Dispose();
		}

		[LuaFunction("DebugMessage")]
		public static void DebugMessage(object message)
		{
			Log.WriteInfo($"(script)->{message}");
		}

		[LuaFunction("ErrorMessage")]
		public static void ErrorMessage(object error)
		{
			Log.WriteException($"(script)->{error}");
		}
	}
}
