/* Basic class to store infomation, which slots of body are occupied,is a dictionary
 * For equipement, Body tells, which parts of body are covered by this piece of equipement
 * For Beings, it tells, which parts of body are covered by any equipement
 */
using System;
using System.Collections.Generic;
using System.Xml;

namespace Game
{
	public class Body: Dictionary<string, bool>
	{	
		/// <summary>
		/// Initializes a new instance of the <see cref="Game.Body"/> class.
		/// </summary>
		/// <param name='lst'>
		/// Lst.
		/// </param>
		public Body ( List<string> lst )
		{
			this["head"] = lst.Contains("head");
			this["body"] = lst.Contains("body");
			this["legs"] = lst.Contains("legs");
			this["boots"] = lst.Contains("boots");
			this["weapon"] = lst.Contains("weapon");
			this["shield"] = lst.Contains("shield");
		}
		
		/// <summary>
		/// Determines whether this item can be equiped. First check, if the item is equipment. If not, it can be worn. If it is, it must be determined, whether all body parts are free.
		/// </summary>
		/// <returns>
		/// <c>true</c> if this instance can equip the specified i; otherwise, <c>false</c>.
		/// </returns>
		/// <param name='i'>
		/// If set to <c>true</c> i.
		/// </param>
		public bool CanEquip(Item i)
		{
			// if the type of item is equipment, check, if can be worn
			if (i.CanBeEquiped)
			{
				// list of body parts,covered by equipment
				List<string> slots = new List<string>();
				foreach(var pair in ((Equipment)i).Body)
					if (pair.Value)
						slots.Add(pair.Key);
				// check, if all body parts are free
				bool all = true;
				if (slots.Count > 0)
				{
					foreach(string s in slots)
						if (this[s])
							all = false;						
					return all;
				}				
				else
					return true;						
			}				
			else // if the item is not equipment, it can be worn
				return false;
		}
		
		/// <summary>
		/// Updates free slots of a body, after an Item is Equiped/Droped.
		/// </summary>
		/// <param name='i'>
		/// I.
		/// </param>
		public void UpdateBody(Equipment i, bool equip)
		{
			foreach(var pair in i.Body)
				if (pair.Value)
					this[pair.Key] = equip; // if item is equiped, change to true, if item is dropped, change to false
			
		}
		
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Game.Body"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current <see cref="Game.Body"/>.
		/// </returns>
		public override string ToString ()
		{
			string output = "";
			foreach (var pair in this)
			{
			    if (pair.Value)
					output += String.Format("\n{0}", pair.Key);
			}
			return output;
		}
		
		public XmlElement ToXml(XmlDocument doc, string elementName)
		{
			XmlElement bodylist = doc.CreateElement(elementName);
			
			foreach (var pair in this)
			{
				bodylist.SetAttribute(pair.Key, pair.Value.ToString());
			}
			
			return bodylist;
		}
	}
}

