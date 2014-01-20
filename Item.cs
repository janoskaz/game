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
		
		protected char symbol;
		
		// every item can have script, which tells what happens, when the item is used
		public string Script { get; set;}	
		
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
			this.Script = null;
		}
		
		public void SetScript(string scriptName)
		{
			Script = scriptName;
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
			return symbol;
		}
		
		public virtual void SetSymbol(string x)
		{
			this.symbol = char.Parse(x);
		}
		
		public virtual bool CanMoveTo()
		{
			return false;
		}
		
		public virtual bool CanDropItemOnto()
		{
			return false;
		}
		
		public string Use()
		{
			if (Script != null)
			{
				ThisGame.lua.DoFile(ThisGame.filePath + "/luascripts/dungeons/" + ThisGame.player.currentDungeon + "/" + Script);
				string newscript = (string)ThisGame.lua["newscript"];
				if (newscript == "null")
					Script = null;
				string message = (string)ThisGame.lua["message"];
				ThisGame.lua["message"] = null; // set message to null in order not to interfere with other scripts
				return message;
			}
			else
			{
				return "Nothing special can be done with this item";
			}
				
		}
		
		public virtual IPlace DropItemOnto(Item i)
		{
			Chest chest = new Chest("Rubble on the ground", new Inventory(100));
			chest.Content.Add(i);
			return chest;
		}
		
		public virtual IPlace AutomaticAction(Player p)
		{
			return this;
		}		
		
		public virtual void VoluntaryAction(Player p)
		{}
		
		public virtual XmlElement ToXml(XmlDocument doc, string elementName="Item")
		{
			XmlElement item = doc.CreateElement(elementName);
			item.SetAttribute("name", Name);
			if (symbol.ToString() != null)
				item.SetAttribute("symbol", symbol.ToString());
			if (Script != null)
				item.SetAttribute("script", Script);
			return item;
		}
	}
}

