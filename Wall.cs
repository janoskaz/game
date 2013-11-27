using System;

namespace Game
{
	public class Wall: BasicObject
	{
		public Wall ()
		{
		}
		
		public override char Symbol()
		{
			return '#';
		}
		
		public override bool PerformAction(Player p, Location l, out string msg, out Location l2)
		{
			msg = "You can't go there - there's a wall!";
			l2 = l;
			return false;
		}
		
	}
}

