using System;
using System.Xml;

namespace Game
{
	/// <summary>
	/// Wall.
	/// </summary>
	public class Impassable: BasicObject
	{
		private string name;
		
		public Impassable (string name)
		{
			this.name = name;
			this.symbol = '#';
		}
		
		/// <summary>
		/// Symbol of the Wall - hash.
		/// </summary>
		public override char Symbol()
		{
			return this.symbol;
		}
		
		public override bool CanMoveTo()
		{
			return false;
		}
		
		/// <summary>
		/// write the xml - just a node with no childs or attributes.
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
		public override XmlElement ToXml(XmlDocument doc, string elementName)
		{
			XmlElement wall = doc.CreateElement("Impassable");
			wall.SetAttribute("name", name.ToString());
			wall.SetAttribute("symbol", Symbol().ToString());
			return wall;
		}
		
		/// <summary>
		/// Performs the action - tells player, that he can not move there.
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
		public override IPlace AutomaticAction(Player p)
		{
			ThisGame.messageLog.Enqueue(String.Format("You can't go there - there's a {0}!", name));
			return this;
		}
		
	}
}

