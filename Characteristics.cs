/* Basic class to store attributes of item/being, is a Dictionary
 * hitpoints, attack, defence, speed, mana, and manareg
 * mana and manareg are invisible (not printed in ToString() method)
 */
using System;
using System.Collections.Generic;

namespace Game
{
	public class Characteristics: Dictionary<string, int>
	{		
		/// <summary>
		/// Initializes a new instance of the <see cref="Game.Characteristics"/> class.
		/// </summary>
		/// <param name='hitpoints'>
		/// Hitpoints.
		/// </param>
		/// <param name='attack'>
		/// Attack.
		/// </param>
		/// <param name='defence'>
		/// Defence.
		/// </param>
		/// <param name='speed'>
		/// Speed.
		/// </param>
		/// <param name='mana'>
		/// Mana.
		/// </param>
		/// <param name='manareg'>
		/// Manareg.
		/// </param>
		public Characteristics (int hitpoints, int attack, int defence, int speed)
		{
			this.Add("hitpoints", hitpoints);
			this.Add("attack", attack);
			this.Add("defence", defence);
			this.Add("speed", speed);;
		}
		
		/// <summary>
		/// Gets the value of characteristic with given name.
		/// </summary>
		/// <returns>
		/// The value.
		/// </returns>
		/// <param name='s'>
		/// S.
		/// </param>
		/// <exception cref='ArgumentException'>
		/// Is thrown when an argument passed to a method is invalid.
		/// </exception>
		public int GetValue(string s)
		{
			if (this.ContainsKey(s))
				return this[s];
			else
				throw new ArgumentException("This characteristic is unknown.");
		}
		
		/// <summary>
		/// Sets the value of given characteristics.
		/// </summary>
		/// <param name='s'>
		/// S.
		/// </param>
		/// <param name='val'>
		/// Value.
		/// </param>
		public void SetValue(string s, int val)
		{
			if (this.ContainsKey(s))
				this[s] = val;
			else
				Console.WriteLine("Can not set value of {0}",s);
		}
		
		/// <summary>
		/// Update the specified hp, attack, def, speed, mana and manareg - adds specified values.
		/// </summary>
		/// <param name='hp'>
		/// Hp.
		/// </param>
		/// <param name='attack'>
		/// Attack.
		/// </param>
		/// <param name='def'>
		/// Def.
		/// </param>
		/// <param name='speed'>
		/// Speed.
		/// </param>
		/// <param name='mana'>
		/// Mana.
		/// </param>
		/// <param name='manareg'>
		/// Manareg.
		/// </param>
		public void Update(int hp, int attack, int def, int speed)
		{
			this["hitpoints"] += hp;
			this["attack"] += attack;
			this["defence"] += def;
			this["speed"] += speed;
		}
		
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Game.Characteristics"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current <see cref="Game.Characteristics"/>.
		/// </returns>
		public override string ToString ()
		{
			return string.Format ("[Characteristics:]\nHitpoints: {0}\nAttack: {1}\nDefence: {2}\n" +
				"Speed: {3}", this["hitpoints"], this["attack"], this["defence"],this["speed"]);
		}
		
	}
}

