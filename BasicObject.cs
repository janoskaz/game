using System;

namespace Game
{
	public class BasicObject :IPlace
	{
		public BasicObject ()
		{
		}
		
		public virtual char Symbol()
		{
			return ' ';
		}
		
		public virtual bool PerformAction(Player p, Location l, out string msg, out Location l2)
		{
			msg = "";
			l2 = l;
			return true;
		}
	}
}

