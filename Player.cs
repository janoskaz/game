using System;

namespace Game
{
	public class Player: Being
	{
		public int X {get; set;}
		public int Y {get; set;}
		public string Message {get; set;}
		
		public Player (string name, Characteristics ch, int bagsize, Dice dice, int x, int y) :base(name, ch, bagsize, dice)
		{
			X = x;
			Y = y;
		}
		
		public void Move(ConsoleKeyInfo c, Map m, out string message)
		{
			bool move = false;
			message = "";
			if ((c.Key == ConsoleKey.UpArrow) && (Y>0))
			{
				move = m.location[X,Y-1].Block.PerformAction(this, out message);
				if (move)
				{
					Y--;
				}				
			}
			else if ((c.Key == ConsoleKey.DownArrow) && (Y<(m.Heigth-1)))
			{
				move = m.location[X,Y+1].Block.PerformAction(this, out message);
				if (move)
				{
					Y++;
				}
			}
			else if ((c.Key == ConsoleKey.LeftArrow) && (X>0))
			{
				move = m.location[X-1,Y].Block.PerformAction(this, out message);
				if (move)
				{
					X--;
				}
			}
			else if ((c.Key == ConsoleKey.RightArrow) && (X<(m.Width-1)))
			{
				move = m.location[X+1,Y].Block.PerformAction(this, out message);
				if (move)
				{
					X++;
				}
			}
			else
			{
				message = "Nothing happened.";
			}
		}
		
		new public char Symbol()
		{
			return 'P';
		}
	}
}

