using System;
using System.Threading;
using System.IO;
using System.Xml;

namespace Game
{
	public class Map
	{
		public int Heigth {get; private set;}
		public int Width {get; private set;}
		
		public Location[,] location;
		
		public Map ()
		{
		}
		
		public void CreateMapField(int x, int y)
		{
			Heigth = y;
			Width = x;
			
			location = new Location[x,y];
			
			for (int i = 0; i<x; i++)
			{
				for (int j = 0; j<y; j++)
				{
					location[i,j] = new Location(i, j, new BasicObject());
				}
			}
		}
		
		public void AddLocation(Location l)
		{
			int x = l.X;
			int y = l.Y;
			location[x, y] = l;
		}
		
		public void CalculateVisibility(Player p)
		{
			int x = p.X;
			int y = p.Y;
			double dist;
			
			for (int i = Math.Max(x-2,0); i <= Math.Min(x+2,this.Width-1); i++)
			{
				for (int j = Math.Max(y-2,0); j <= Math.Min(y+2,this.Heigth-1); j++)
				{
					dist = Math.Sqrt((i-x)*(i-x) + (j-y)*(j-y));
					if (dist <= 2)
					{
						location[i,j].Visible = true;
					}						
				}
			}
		}
		
		public void Draw(Player p)
		{
			int w = Console.WindowWidth;
			int h = ThisGame.mapHeight;
			
			int diffx = (int)Math.Ceiling((double)(w/2)) - p.X;
			int diffy = (int)Math.Ceiling((double)(h/2)) - p.Y;
			
			for (int i = 3; i<w-2; i++)
			{
				for (int j = 2; j<h-1; j++)
				{
					try
					{
						Console.CursorLeft = i;
						Console.CursorTop = j;
						if (this.location[i-diffx,j-diffy].Visible)
							Console.Write(this.location[i-diffx,j-diffy].Symbol());
					}
					catch
					{
						Console.Write('?');
					}
				}
			}			
			for (int i = 0; i<w; i++)
			{
				Console.CursorLeft = i;
				Console.CursorTop = h;
				Console.Write('=');
			}
		}
		
		public void ToXml(string mapName)
		{
			string path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName,"files/");
			
			XmlDocument doc = new XmlDocument();
			
			XmlDeclaration header = doc.CreateXmlDeclaration("1.0", "utf-8", null);
			doc.AppendChild(header);
			XmlElement root = doc.CreateElement("map");
			root.SetAttribute("width", Width.ToString());
			root.SetAttribute("heigth", Heigth.ToString());
			
			foreach (Location l in location)
			{
				XmlElement loc = doc.CreateElement("location");
				// attributes of location
				loc.SetAttribute("x", l.X.ToString());
				loc.SetAttribute("y", l.Y.ToString());
				loc.SetAttribute("visible", l.Visible.ToString());
				// create block with the name of the inner class
				XmlElement block = doc.CreateElement("block");
				string type = l.Block.GetType().ToString();
				block.SetAttribute("type", type);
				// append content of block				
				XmlElement innerObject = l.Block.ToXml(doc, type.Split('.')[1]);
				block.AppendChild(innerObject);
				// append block
				loc.AppendChild(block);
				// append loc
				root.AppendChild(loc);
			}
			
			doc.AppendChild(root);
			doc.Save( Path.Combine(path, mapName + "_map.xml") );
			
		}
		
	}
}

