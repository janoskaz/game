using System;
using System.Xml;

namespace Game
{
	public class Location :IPlace
	{
		public int X {get; set;}
		public int Y {get; set;}
		public IPlace Block {get; set;}
		public bool Visible {get; set;}
		
		public Location (int x, int y, IPlace o)
		{
			X = x;
			Y = y;
			Block = o;
			Visible = false;
		}
		
		public char Symbol()
		{
			return Block.Symbol();
		}
		
		public bool PerformAction(Player p, Location l, out string msg, out Location l2)
		{
			msg = "";
			l2 = l;
			bool action = Block.PerformAction(p, l, out msg, out l2);
			Console.WriteLine(msg);
			return action;
		}
		
		public virtual XmlElement ToXml(XmlDocument doc, string elementName)
		{
			XmlElement loc = doc.CreateElement(elementName);
			// attributes of location
			loc.SetAttribute("x", X.ToString());
			loc.SetAttribute("y", Y.ToString());
			loc.SetAttribute("visible", Visible.ToString());
			// create block with the name of the inner class
			XmlElement block = doc.CreateElement("block");
			string type = Block.GetType().ToString();
			block.SetAttribute("type", type);
			// append content of block				
			XmlElement innerObject = Block.ToXml(doc, type.Split('.')[1]);
			block.AppendChild(innerObject);
			// append block
			loc.AppendChild(block);
			
			return loc;
		}
	}
}

