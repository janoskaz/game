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
		
		// calculate visibility - how far does the player see?
		public void CalculateVisibility(Player p, int visibility)
		{
			int x = p.X;
			int y = p.Y;
			double dist;
			
			for (int i = Math.Max(x-visibility,0); i <= Math.Min(x+visibility,this.Width-1); i++)
			{
				for (int j = Math.Max(y-visibility,0); j <= Math.Min(y+visibility,this.Heigth-1); j++)
				{
					dist = Math.Sqrt((i-x)*(i-x) + (j-y)*(j-y));
					if (dist < visibility)
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
						else
							Console.Write('?');
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
			string path = ThisGame.filePath;
			
			XmlDocument doc = new XmlDocument();
			
			XmlDeclaration header = doc.CreateXmlDeclaration("1.0", "utf-8", null);
			doc.AppendChild(header);
			XmlElement root = doc.CreateElement("map");
			root.SetAttribute("width", Width.ToString());
			root.SetAttribute("heigth", Heigth.ToString());
			
			foreach (Location l in location)
			{
				XmlElement loc = l.ToXml(doc, "location");
				root.AppendChild(loc);
			}
			
			doc.AppendChild(root);
			doc.Save( Path.Combine(path, mapName + "_map.xml") );
			
		}
		
	}
}

