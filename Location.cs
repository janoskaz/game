using System;

namespace Game
{
	public class Location :IPlace
	{
		public int X {get; set;}
		public int Y {get; set;}
		public IPlace Block {get; set;}
		
		public Location (int x, int y, IPlace o)
		{
			X = x;
			Y = y;
			Block = o;
		}
		
		public char Symbol()
		{
			return Block.Symbol();
		}
		
		public bool PerformAction(Player p, Location l, out string msg, out Location l2)
		{
			msg = "";
			l2 = l;
			bool action = Block.PerformAction(p, l, out msg, out l2);
			Console.WriteLine(msg);
			return action;
		}
	}
}

