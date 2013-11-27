using System;
using System.Collections.Generic;

namespace Game
{
	public class World
	{
		private List<Being> creatures = new List<Being>();
		
		private Map map;
		
		public World (Map map)
		{
			this.map = map;
		}
		
		public int WhoAttacks(Being creature1, Being creature2)
		{
			int init1 = creature1.CalculateInitiative();
			int init2 = creature2.CalculateInitiative();
			
			if (init1 > init2)
				return 1;
			if (init2>init1)
				return 2;
			return 0;					
		}
		
		public void Fight(Being creature1, Being creature2)
		{
			Being b1 = creature1;
			Being b2 = creature2;
			
			while(b1.Alive() && b2.Alive())
			{
				// decide, who will attack first
				int attacker = WhoAttacks(creature1, creature2);
				if (attacker==2)
				{
					b2 = creature1;
					b1 = creature2;
				}
				// perform attack
				b1.Attack(b2);
				if(b2.Alive() || attacker==0) // if enemy 2 is alive, of if the attacks are parallel
				{
					b2.Attack(b1);
				}
				b1 = creature1;
				b2 = creature2;
			}
		}
	}
}

