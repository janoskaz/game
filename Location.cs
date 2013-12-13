using System;
using System.Xml;

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
		
		public Location (int x, int y, IPlace o)
		{
			X = x;
			Y = y;
			Block = o;
			Visible = false;
		}
		
		/// <summary>
		/// Pass to the symbol.
		/// </summary>
		public char Symbol()
		{
			return Block.Symbol();
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
		public bool PerformAction(Player p, Location l, out Location l2)
		{
			l2 = l;
			bool action = Block.PerformAction(p, l, out l2);
			return action;
		}
		
		public void VoluntaryAction(Player p)
		{
			Block.VoluntaryAction(p);
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
			loc.SetAttribute("visible", Visible.ToString());
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

