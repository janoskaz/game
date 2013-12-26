using Game;
using System;
using System.Collections.Generic;
using NLua;

class Program
{
	
	public static void Main()
	{
		// some vizualization
		Console.BackgroundColor = ConsoleColor.White;
		Console.ForegroundColor = ConsoleColor.Black;		
		Console.Clear();
		Console.SetWindowSize(81,10);
		
		//Lua lua = new Lua();
		//lua.DoFile("/home/zbynek/Plocha/csharp/Game/Game/files/luascripts/dungeon1.lua");
		
		ThisGame.RunGame();
		
	}
	
}
