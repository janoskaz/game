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
		
		public virtual bool PerformAction(Player p, out string msg)
		{
			msg = "";
			return true;
		}
	}
}

