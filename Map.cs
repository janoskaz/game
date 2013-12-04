using System;
using System.Threading;
using System.IO;

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
		
		public void Draw()
		{
			for (int i = 0; i<Width; i++)
			{
				for (int j = 0; j<Heigth; j++)
				{
					Console.CursorLeft = i;
					Console.CursorTop = j;
					if (this.location[i,j].Visible)
						Console.Write(this.location[i,j].Symbol());
					else
						Console.Write('?');
				}
			}
			
		}
		
		public void LoadMap(string filename)
		{
			string startupPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName,"files/"+filename);
			
			using (StreamReader sr = new StreamReader(startupPath))
			{
				string s;
				while ((s = sr.ReadLine()) != null)
				{
					try
					{
						string[] splitted = s.Split(';');
						string klass = splitted[0];
						int x = int.Parse (splitted[1]);
						int y = int.Parse (splitted[2]);
						switch(klass)
							{
							case "Map":
							{
								this.CreateMapField(x,y);
								break;
							}
							case "Wall":
							{
								this.AddLocation( new Location( x,y, new Wall() ) );
								break;
							}
						}
					}
					catch
					{
						Console.WriteLine("Configuration file is incorrect.");
					}
					
				}
			}
		}
	
	}
}

