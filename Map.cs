using System;

namespace Game
{
	public class Map
	{
		public int Heigth {get; private set;}
		public int Width {get; private set;}
		
		public Location[,] location;
		
		public Map (int x, int y)
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
		
		public void Draw()
		{
			for (int i = 0; i<Width; i++)
			{
				for (int j = 0; j<Heigth; j++)
				{
					Console.CursorLeft = i;
					Console.CursorTop = j;
					Console.Write(this.location[i,j].Symbol());
				}
			}
			
		}
	
	}
}

