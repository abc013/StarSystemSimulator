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
		
		public LuaScriptWrapper(Simulation simulation, string file)
		{
			luaState = new Lua();
			luaState.DoString(@"import = function () end");
			luaState.DoString(@"init = function () end");
			luaState.DoString(@"tick = function () end");

			registerFunctions(typeof(LuaScriptWrapper));
			registerFunctions(typeof(LuaFunctions));
			registerFunctions(typeof(Camera));
			registerFunctions(typeof(Simulation), simulation);

			Distance.FillStates(this);
			Mass.FillStates(this);
			Time.FillStates(this);

			luaState.DoFile(file);

			tick = luaState["tick"] as LuaFunction;
		}

		void registerFunctions(Type type, object obj = null)
		{
			foreach (var method in type.GetMethods())
			{
				var attribute = method.GetCustomAttributes(false).FirstOrDefault(c => c is LuaFunctionAttribute);

				if (attribute != null)
					luaState.RegisterFunction(((LuaFunctionAttribute)attribute).FunctionName, obj, method);
			}
		}

		public void Load()
		{
			var init = luaState["init"] as LuaFunction;
			init.Call();
			init.Dispose();
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
			luaState.Push(value);
			luaState.State.SetGlobal(name);
		}

		public void Dispose()
		{
			tick.Dispose();
		}

		[LuaFunction("DebugMessage")]
		public static void DebugMessage(object message)
		{
			Log.WriteInfo($"(script)->{message}");
			Graphics.Camera.Translate(1, 0, 0);
		}

		[LuaFunction("ErrorMessage")]
		public static void ErrorMessage(object error)
		{
			Log.WriteException($"(script)->{error}");
			Graphics.Camera.Translate(1, 0, 0);
		}
	}
}
