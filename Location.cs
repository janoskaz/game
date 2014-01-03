using System;
using System.Xml;
using NLua;

namespace Game
{
	/// <summary>
	/// Location is a square in a map. Contains coordinates X, Y, indicator, whether is visible, and block, which stores object inheriting interface IPlace.
	/// </summary>
	public class Location :IPlace
	{
		public int X {get; set;}
		public int Y {get; set;}
		public IPlace Block {get; set;}
		public bool Visible {get; set;}
		public char symbol;
		// script, if available. default is null
		public string Script { get; set;}		
		
		public Location (int x, int y, IPlace o)
		{
			X = x;
			Y = y;
			Block = o;
			Visible = false;
			Script = null;
			symbol = Block.Symbol();
		}
		
		public void SetScript(string scriptName)
		{
			this.Script = scriptName;
		}
		
		/// <summary>
		/// Pass to the symbol.
		/// </summary>
		public char Symbol()
		{
			return symbol;
		}
		
		public virtual void SetSymbol(string x)
		{
			this.symbol = char.Parse(x);
		}
		
		public void UpdateSymbol()
		{
			this.symbol = this.Block.Symbol();
		}
		
		public bool CanMoveTo()
		{
			return this.Block.CanMoveTo();
		}
		
		public virtual bool CanDropItemOnto()
		{
			return this.Block.CanDropItemOnto();
		}
		
		public virtual void DropItemOnLocation(Item i)
		{
			this.Block = this.Block.DropItemOnto(i);
		}
		
		public virtual IPlace DropItemOnto(Item i)
		{
			return this;
		}
		
		/// <summary>
		/// Passes method to block.
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
		/// <param name='l2'>
		/// If set to <c>true</c> l2.
		/// </param>
		public IPlace AutomaticAction(Player p)
		{
			if (Script == null)
				this.Block = Block.AutomaticAction(p);
			else
			{
				ThisGame.lua["block"] = Block;
				ThisGame.lua.DoFile(ThisGame.filePath + "/luascripts/" + Script);
				Block = (IPlace)ThisGame.lua["out"];
				bool keepScript = (bool)ThisGame.lua["keepscript"];
				string message = (string)ThisGame.lua["message"];
				if (message != "null")
					ThisGame.messageLog.Enqueue(message);
				if (!keepScript)
				{
					if ((string)ThisGame.lua["newscript"] == "null")
						Script = null;
					else
						Script = (string)ThisGame.lua["newscript"];
				}
				UpdateSymbol();
				ThisGame.lua["block"] = null;
				ThisGame.lua["out"] = null;
				ThisGame.lua["keepscript"] = null;
				ThisGame.lua["newscript"] = null;
				ThisGame.lua["message"] = null;
			}
			
			return this;
		}
		
		public void VoluntaryAction(Player p)
		{
			Block.VoluntaryAction(p);
			if (Block is Chest)
				if (((Chest)Block).removeIfEmpty)
					if (((Chest)Block).IsEmpty())
						Block = new BasicObject();
			this.UpdateSymbol();
		}
		
		/// <summary>
		/// Writes location to Xml.
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
			XmlElement loc = doc.CreateElement(elementName);
			// attributes of location
			loc.SetAttribute("x", X.ToString());
			loc.SetAttribute("y", Y.ToString());
			//loc.SetAttribute("symbol", Symbol().ToString());
			loc.SetAttribute("visible", Visible.ToString());
			if (Script != null)
				loc.SetAttribute("script", Script);
			// create block with the name of the inner class
			XmlElement block = doc.CreateElement("block");
			string type = Block.GetType().ToString();
			block.SetAttribute("type", type);
			// append content of block - pass to inner object	
			XmlElement innerObject = Block.ToXml(doc, type.Split('.')[1]);
			block.AppendChild(innerObject);
			// append block
			loc.AppendChild(block);
			return loc;
		}
	}
}

