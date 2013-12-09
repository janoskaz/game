/* Basic class to store attributes of item/being, is a Dictionary
 * hitpoints, attack, defence, speed, mana, and manareg
 * mana and manareg are invisible (not printed in ToString() method)
 */
using System;
using System.Collections.Generic;
using System.Xml;

namespace Game
{
	public class Characteristics :IXml
	{
		public int hitpoints;
		public int attack;
		public int defence;
		public int speed;
		
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
			this.hitpoints = hitpoints;
			this.attack = attack;
			this.defence = defence;
			this.speed = speed;
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
			this.hitpoints += hp;
			this.attack += attack;
			this.defence += def;
			this.speed += speed;
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
				"Speed: {3}", this.hitpoints, this.attack, this.defence, this.speed);
		}
		
		public XmlElement ToXml(XmlDocument doc, string elementName)
		{
			XmlElement ch = doc.CreateElement(elementName);
			// append hitpoints
			ch.SetAttribute("hitpoints", this.hitpoints.ToString());
			ch.SetAttribute("attack", this.attack.ToString());
			ch.SetAttribute("defence", this.defence.ToString());
			ch.SetAttribute("speed", this.speed.ToString());
			
			return ch;
		}
		
	}
}

