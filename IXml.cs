using System;
using System.Xml;

namespace Game
{
	/// <summary>
	/// Interface Xml - defines function, which allows objects to be written to XML format.
	/// </summary>
	public interface IXml
	{
		XmlElement ToXml(XmlDocument doc, string elementName);
	}
}

