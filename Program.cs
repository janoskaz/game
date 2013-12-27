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
		
		Lua lua = new Lua();
		lua.DoFile("/home/zbynek/Plocha/csharp/Game/Game/files/luascripts/dungeon1.lua");
		
		ThisGame.RunGame();
		
	}
	
}
