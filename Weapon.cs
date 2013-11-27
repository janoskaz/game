/* Weapon inherits from Equipment and is extended by havind a dice
 */
using System;
using System.Collections.Generic;

namespace Game
{
	public class Weapon: Equipment
	{
		// Weapon has a dice, which tells, how big the attack is
		public readonly Dice dice;
		
		public Weapon (string name, Characteristics ch, List<string> lst, Dice dice) :base(name, ch, lst)
		{
			this.dice = dice;
		}
		
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Game.Weapon"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current <see cref="Game.Weapon"/>.
		/// </returns>
		public override string ToString ()
		{
			return string.Format ("[Item:]\nName: {0}\n{1}\n[Equiped in slots:] {2}\nAttack roll with with {3}-sided dice", 
			                      this.Name, this.Characteristics.ToString(), this.Body.ToString(), this.dice.NrFacets);
			
		}
	}
}

