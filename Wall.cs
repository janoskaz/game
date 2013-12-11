using System;
using System.Xml;

namespace Game
{
	/// <summary>
	/// Wall.
	/// </summary>
	public class Wall: BasicObject
	{
		public Wall ()
		{
		}
		
		/// <summary>
		/// Symbol of the Wall - hash.
		/// </summary>
		public override char Symbol()
		{
			return '#';
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
			XmlElement wall = doc.CreateElement("Wall");
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
		public override bool PerformAction(Player p, Location l, out string msg, out Location l2)
		{
			msg = "You can't go there - there's a wall!";
			l2 = l;
			return false;
		}
		
	}
}

