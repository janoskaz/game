using System;
using System.Xml;

namespace Game
{
	public interface IXml
	{
		XmlElement ToXml(XmlDocument doc, string elementName);
	}
}

