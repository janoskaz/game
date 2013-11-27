using System;

namespace Game
{
	public interface IPlace
	{
		char Symbol();
		bool PerformAction(Player p, Location l, out string msg, out Location  l2);
	}
}

