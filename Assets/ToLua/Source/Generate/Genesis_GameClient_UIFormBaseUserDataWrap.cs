﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class Genesis_GameClient_UIFormBaseUserDataWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(Genesis.GameClient.UIFormBaseUserData), typeof(System.Object));
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("ShouldOpenImmediately", get_ShouldOpenImmediately, set_ShouldOpenImmediately);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ShouldOpenImmediately(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			Genesis.GameClient.UIFormBaseUserData obj = (Genesis.GameClient.UIFormBaseUserData)o;
			bool ret = obj.ShouldOpenImmediately;
			LuaDLL.lua_pushboolean(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index ShouldOpenImmediately on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_ShouldOpenImmediately(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			Genesis.GameClient.UIFormBaseUserData obj = (Genesis.GameClient.UIFormBaseUserData)o;
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			obj.ShouldOpenImmediately = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index ShouldOpenImmediately on a nil value" : e.Message);
		}
	}
}

