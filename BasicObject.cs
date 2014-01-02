using System;
using System.Xml;

namespace Game
{
	/// <summary>
	/// Basic object is only filling of empty map squares.
	/// </summary>
	public class BasicObject :IPlace
	{
		private char symbol;
		
		public BasicObject ()
		{
			this.symbol = ' ';
		}
		
		/// <summary>
		/// Symbol of basic object is empty space.
		/// </summary>
		public virtual char Symbol()
		{
			return this.symbol;
		}
		
		public virtual void SetSymbol(string x)
		{
			this.symbol = char.Parse(x);
		}
		
		public virtual bool CanMoveTo()
		{
			return true;
		}
		
		public virtual bool CanDropItemOnto()
		{
			return true;
		}
		
		public virtual IPlace DropItemOnto(Item i)
		{
			Chest chest = new Chest("Rubble on the ground", new Inventory(100));
			chest.Content.Add(i);
			return chest;
		}
		
		/// <summary>
		/// Write to the xml - just a simple node with no childs or attributes.
		/// </summary>
		/// <returns>
		/// The xml.
		/// </returns>
		/// <param name='doc'>
		/// Document.
		/// </param>
		/// <param name='elementName'>
		/// Element name.
		/// </param>
		public virtual XmlElement ToXml(XmlDocument doc, string elementName)
		{
			XmlElement basicObject = doc.CreateElement("BasicObject");
			return basicObject;
		}
		
		/// <summary>
		/// Performs the action on basicObject - just moves player to the object.
		/// </summary>
		/// <returns>
		/// The action.
		/// </returns>
		/// <param name='p'>
		/// If set to <c>true</c> p.
		/// </param>
		/// <param name='l'>
		/// If set to <c>true</c> l.
		/// </param>
		/// <param name='msg'>
		/// If set to <c>true</c> message.
		/// </param>
		/// <param name='l2'>
		/// If set to <c>true</c> l2.
		/// </param>
		public virtual IPlace AutomaticAction(Player p)
		{
			return this;
		}
		
		public virtual void VoluntaryAction(Player p)
		{}
	}
}

