using System;

namespace Game
{
	public interface IPlace
	{
		char Symbol();
		bool PerformAction(Player p, out string msg);
	}
}

