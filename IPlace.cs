using System;

namespace Game
{
	public interface IPlace :IXml
	{
		char Symbol();
		bool PerformAction(Player p, Location l, out string msg, out Location  l2);
	}
}

