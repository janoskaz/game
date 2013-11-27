using System;
using System.Threading;

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
			Location l2 = m.location[X,Y];
			if ((c.Key == ConsoleKey.UpArrow) && (Y>0))
			{
				move = m.location[X,Y-1].Block.PerformAction(this, m.location[X,Y-1], out message, out l2);
				if (move)
					Y--;
				else
					l2 = m.location[X,Y];
			}
			else if ((c.Key == ConsoleKey.DownArrow) && (Y<(m.Heigth-1)))
			{
				move = m.location[X,Y+1].Block.PerformAction(this, m.location[X,Y+1], out message, out l2);
				if (move)
					Y++;
				else
					l2 = m.location[X,Y];
			}
			else if ((c.Key == ConsoleKey.LeftArrow) && (X>0))
			{
				move = m.location[X-1,Y].Block.PerformAction(this, m.location[X-1,Y], out message, out l2);
				if (move)
					X--;
				else
					l2 = m.location[X,Y];
			}
			else if ((c.Key == ConsoleKey.RightArrow) && (X<(m.Width-1)))
			{
				move = m.location[X+1,Y].Block.PerformAction(this, m.location[X+1,Y], out message, out l2);
				if (move)
					X++;
				else
					l2 = m.location[X,Y];
			}
			else
			{
				message = "Nothing happened.";
			}
			m.location[X,Y] = l2;
		}
		
		public int WhoAttacks(Being creature)
		{
			int init1 = this.CalculateInitiative();
			int init2 = creature.CalculateInitiative();
			
			if (init1 > init2)
				return 1;
			if (init2>init1)
				return 2;
			return 0;					
		}
		
		public bool Fight(Being creature)
		{
			Being b1 = this;
			Being b2 = creature;
			
			while(b1.Alive() && b2.Alive())
			{
				// decide, who will attack first
				int attacker = this.WhoAttacks(creature);
				if (attacker==2)
				{
					b2 = this;
					b1 = creature;
				}
				// perform attack
				b1.Attack(b2);
				if(b2.Alive() || attacker==0) // if enemy 2 is alive, of if the attacks are parallel
				{
					b2.Attack(b1);
				}
				b1 = this;
				b2 = creature;
				Thread.Sleep(500);
			}
			return this.Alive();
		}
		
		new public char Symbol()
		{
			return 'P';
		}
	}
}

