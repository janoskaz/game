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
		
		public override bool PerformAction(Player p, out string msg)
		{
			msg = "You can't go there - there's a wall!";
			return false;
		}
		
	}
}

