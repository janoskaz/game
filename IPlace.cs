using System;

namespace Game
{
	/// <summary>
	/// Interface Place - each class, which inherits this interface, has methods Symbol, and PerformAction.
	/// </summary>
	public interface IPlace :IXml
	{
		char Symbol();
		bool AutomaticAction(Player p, Location l, out Location  l2);
		void VoluntaryAction(Player p);
	}
}

