using Game;
using System;
using System.Collections.Generic;
using NLua;

class Program
{
	
	public static void Main()
	{
		// some vizualization
		Console.BackgroundColor = ConsoleColor.DarkGray;
		Console.ForegroundColor = ConsoleColor.Gray;		
		Console.Clear();
		
		//Lua lua = new Lua();
		//lua.DoFile("/home/zbynek/Plocha/csharp/Game/Game/files/luascripts/generator/dungeon1.lua");
		//klua.DoFile("/home/zbynek/Plocha/csharp/Game/Game/files/luascripts/generator/desert.lua");
		
		ThisGame.RunGame();
		
	}
	
}
