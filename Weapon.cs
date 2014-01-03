/* Weapon inherits from Equipment and is extended by havind a dice
 */
using System;
using System.Collections.Generic;
using System.Xml;

namespace Game
{
	public class Weapon: Equipment
	{
		// Weapon has a dice, which tells, how big the attack is
		public readonly int NrFacets;
		
		public Weapon (string name, Characteristics ch, Array lst, int nrFacets) :base(name, ch, lst)
		{
			this.NrFacets = nrFacets;
		}
		
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Game.Weapon"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current <see cref="Game.Weapon"/>.
		/// </returns>
		public override string ToString ()
		{
			return string.Format ("[Item:]\nName: {0}\n{1}\n[Equiped in slots:] {2}\nAttack roll with with {3}-sided dice", 
			                      this.Name, this.Characteristics.ToString(), this.Body.ToString(), this.NrFacets);
			
		}
		
		public override XmlElement ToXml(XmlDocument doc, string elementName)
		{
			XmlElement equipment = doc.CreateElement(elementName);
			equipment.SetAttribute("name", Name);
			equipment.SetAttribute("nrfacets", NrFacets.ToString());
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

