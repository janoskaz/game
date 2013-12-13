/* Basic class to store information about items
 * Every item has name and characteristics (attack, defence, hitpoints etc)
 * */
using System;
using System.Xml;

namespace Game
{
	public class Item: IPlace
	{
		// every item has to have a name
		public string Name {get; private set;}
		
		// can be equiped?
		public bool CanBeEquiped { get; set;}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Game.Item"/> class.
		/// </summary>
		/// <param name='name'>
		/// Name.
		/// </param>
		/// <param name='ch'>
		/// Ch.
		/// </param>
		public Item (string name)
		{
			this.Name = name;
			this.CanBeEquiped = false;
		}
		
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Game.Item"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current <see cref="Game.Item"/>.
		/// </returns>
		public override string ToString ()
		{
			return string.Format ("[Item:]\nName: {0}", this.Name);
		}
		
		public virtual char Symbol()
		{
			return '*';
		}
		
		public virtual bool AutomaticAction(Player p, Location l, out Location l2)
		{
			l2 = l;
			return true;
		}
		
		public virtual void VoluntaryAction(Player p)
		{}
		
		public virtual XmlElement ToXml(XmlDocument doc, string elementName="Item")
		{
			XmlElement item = doc.CreateElement(elementName);
			item.SetAttribute("name", Name);
			return item;
		}
	}
}

