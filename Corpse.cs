using System;

namespace Game
{
	public class Corpse: Chest
	{
		
		public Corpse (string name, Inventory content) :base(name, content)
		{
			message = "Some might find it disgusting, but looting corpses is the only way to survive in this world\nTHE CORPSE HAS:";
		}
		
		public override char Symbol()
		{
			return 'X';
		}

	}
}

