using System;

namespace Game
{
	/// <summary>
	/// Interface Place - each class, which inherits this interface, has methods Symbol, and PerformAction.
	/// </summary>
	public interface IPlace :IXml
	{
		char Symbol();
		void SetSymbol(string x);
		bool CanMoveTo();
		bool CanDropItemOnto();
		IPlace DropItemOnto(Item i);
		IPlace AutomaticAction(Player p);
		void VoluntaryAction(Player p);
	}
}

