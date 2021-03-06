/* Equipments extends Items
 * new slot - Body - is added, body tells, where the item is carried
 */
using System;
using System.Collections.Generic;
using System.Xml;

namespace Game
{
	public class Equipment: Item
	{
		// Specify, on which body parts is equiped (can be none, i.e.ring)
		public Body Body { get; private set; }
		
		// every item has to have characteristics (attack, defence etc.)
		public Characteristics Characteristics {get; private set;}

		/// <summary>
		/// Initializes a new instance of the <see cref="Game.Equipment"/> class.
		/// </summary>
		/// <param name='name'>
		/// Name.
		/// </param>
		/// <param name='ch'>
		/// Ch.
		/// </param>
		/// <param name='lst'>
		/// Lst.
		/// </param>
		public Equipment (string name, Characteristics ch, Array lst) :base (name)
		{
			this.CanBeEquiped = true;
			this.Characteristics = ch;
			this.Body = new Body(lst);
		}
		
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Game.Equipment"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current <see cref="Game.Equipment"/>.
		/// </returns>
		public override string ToString ()
		{
			return string.Format ("[Item:]\nName: {0}\n{1}\n[Equiped in slots:] {2}", this.Name, this.Characteristics.ToString(), this.Body.ToString());
		}
		
		public override XmlElement ToXml(XmlDocument doc, string elementName)
		{
			XmlElement equipment = doc.CreateElement(elementName);
			equipment.SetAttribute("name", Name);
			if (symbol.ToString() != null)
				equipment.SetAttribute("symbol", symbol.ToString());
			XmlElement ch = this.Characteristics.ToXml(doc, "Characteristics");
			equipment.AppendChild(ch);
			XmlElement body = this.Body.ToXml(doc, "Body");
			equipment.AppendChild(body);
			return equipment;
		}
	}
}
