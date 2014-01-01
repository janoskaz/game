using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml;
using NLua;

namespace Game
{
	public class Being: IPlace
	{
		// Everyone has to have a name
		public string Name {get; private set;}
		
		// Everyone has basic characteristics
		protected Characteristics Characteristics {get; private set;}
		
		// Everyone has current characteristics
		protected Characteristics CurrentCharacteristics {get; private set;}
		
		// everyone has a bag
		public Inventory bag;
		
		// everyone has things, which are currently equiped
		public Inventory equiped;
		
		// Everyone has a body
		public Body Body {get; private set;}
		
		protected char symbol;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Game.Being"/> class.
		/// </summary>
		/// <param name='name'>
		/// Name.
		/// </param>
		/// <param name='ch'>
		/// Ch.
		/// </param>
		/// <param name='bagsize'>
		/// Bagsize.
		/// </param>
		public Being (string name, Characteristics ch, Characteristics currentCh, int bagsize)
		{
			this.Name = name;
			this.Characteristics = ch;
			this.CurrentCharacteristics = currentCh;
			this.bag = new Inventory(bagsize);
			this.equiped = new Inventory(bagsize);
			this.Body = new Body( new string[0]);
			this.symbol = 'B';
		}
		
		/// <summary>
		/// Pick the item and put it into bag.
		/// </summary>
		/// <param name='item'>
		/// Item.
		/// </param>
		public void PickItem(Item item)
		{
			this.bag.Add(item);
		}
		
		public bool HasItem(string name)
		{
			bool hasIt = (this.bag.Inside(name) || this.equiped.Inside(name));
			return hasIt;
		}
		
		public bool HasKey(string keyname)
		{
			foreach (Item i in this.bag.bag)
				if (i.Name == keyname)
					return true;
			return false;
		}
		
		/// <summary>
		/// Drop the item from the bag.
		/// </summary>
		/// <param name='name'>
		/// Name.
		/// </param>
		public string DropItem(Item i)
		{
			string msg = this.bag.Remove(i);
			return msg;
		}
		
		public Item GetItem(string name)
		{
			Item item = new Item("Nothing");
			foreach (Item i in this.bag.bag)
			{
				if (i.Name == name)
				{
					item = i;
					break;
				}
			}
			return item;
		}
		
		/// <summary>
		/// Determines whether this instance can equip item with specified name.
		/// </summary>
		/// <returns>
		/// <c>true</c> if this instance can equip the specified name; otherwise, <c>false</c>.
		/// </returns>
		/// <param name='name'>
		/// If set to <c>true</c> name.
		/// </param>
		public bool CanEquip(Item i)
		{
			return this.Body.CanEquip(i);
		}
		
		/// <summary>
		/// Makes the space for equipment - strips equipment, which is in the way of current equipment.
		/// </summary>
		/// <param name='name'>
		/// Name.
		/// </param>
		public string MakeSpaceForEquipment(Item i)
		{
			string msg = "Making space for new items";
			bool isInside = this.bag.Inside(i.Name);
			if (isInside)
			{
				// list of covered body parts
				List<string> slots = new List<string>();
				foreach(var pair in ((Equipment)i).Body)
					if (pair.Value)
						slots.Add(pair.Key);
				// list of items on these body parts
				List<Equipment> toRemove = new List<Equipment>();
				foreach (Equipment e in this.equiped.bag)
				{
					foreach (var pair in e.Body)
					{
						if (pair.Value && (slots.Contains(pair.Key)))
							toRemove.Add(e);
					}
				}
				// strip these body parts
				foreach (Equipment e in toRemove)
				{
					msg += ("\n" + this.StripItem(e));
				}
			}
			return msg;
		}
		
		/// <summary>
		/// Equips the item - from the bag to equiped. Must have the item in the bag. Enables force equip
		/// </summary>
		/// <param name='name'>
		/// Name.
		/// </param>
		public string EquipItem(Item i, bool force = false)
		{
			string msg = "Attempt to equip and item";
			// can the thing be equiped?
			if (this.CanEquip(i))
			{
				// Is the item already inside bag?
				bool isInside = this.bag.Inside(i.Name);
				if (isInside)
				{
					// can the Item be put on body? (are there empty slots?)
					bool canEquip = this.Body.CanEquip(i);
					if (canEquip)
					{
						msg += this.bag.Remove(i); // remove from the bag
						msg += this.equiped.Add(i); // equip
						this.Body.UpdateBody((Equipment)i, true); // update body slots
						this.UpdateCharacteristics((Equipment)i, true); // update current characteristics
						msg = String.Format("{0} has been equiped", i.Name);
					}
						
				}
				else
					msg = String.Format("Item is not inside the bag and can not be equiped");	
			}
			else
			{
				if (force)
				{
					this.MakeSpaceForEquipment(i);
					this.EquipItem(i, false);
				}
				else
					msg = String.Format("{0} can not be equiped", i.Name);			
			}
			return msg;
		}
				
		/// <summary>
		/// Strips the item fromequiped and put it into bag.
		/// </summary>
		/// <param name='name'>
		/// Name.
		/// </param>
		public string StripItem(Item i)
		{
			bool spaceInBag = (this.bag.bag.Count < this.bag.maxsize);
			string msg;
			if (spaceInBag)
			{
				this.equiped.Remove(i);
				this.bag.Add(i);
				this.Body.UpdateBody((Equipment)i, false);
				this.UpdateCharacteristics((Equipment)i, false);
				msg = String.Format("{0} has been stripped and put into bag", i.Name);
			}
			else
			{
				msg = String.Format("{0} can not be stripped, because the bag is full", i.Name);
			}
			return msg;
		}
		
		/// <summary>
		/// Updates the characteristics by enhancement of given item. If item is equiped, add values, if item is stripped, substract values
		/// </summary>
		public void UpdateCharacteristics(Equipment i, bool equip)
		{	
			int multiplier = 1;
			if (!equip)
				multiplier = -1;
			
			int hitpoints = i.Characteristics.hitpoints * multiplier;
			int	attack = i.Characteristics.attack * multiplier;
			int	defence = i.Characteristics.defence * multiplier;
			int	speed = i.Characteristics.speed* multiplier;
						
			this.CurrentCharacteristics.Update(hitpoints, attack, defence, speed);
		}
		
		public void PermanentlyUpdateCharacteristics(int hp, int attack, int defence, int speed)
		{
			this.Characteristics.Update(hp, attack, defence, speed);
			this.CurrentCharacteristics.Update(hp, attack, defence, speed);
		}
		
		public void SetCurrentCharacteriscs(Characteristics ch)
		{
			this.CurrentCharacteristics = ch;
		}
		
		public void SetBody(Body b)
		{
			this.Body = b;
		}
		
		/// <summary>
		/// Calculates the attack roll.
		/// </summary>
		/// <returns>
		/// The attack.
		/// </returns>
		public int CalculateAttack()
		{
			// get attack value, set default dice and default weapon to bare hands
			int attack = this.CurrentCharacteristics.attack;
			// if a weapon is equiped, find out the name and dice of this weapon
			foreach (Item i in this.equiped.bag)
			{
				if (i.GetType() == typeof(Weapon))
					{
						string weapon = i.Name;
						attack += Dice.Roll(((Weapon)i).NrFacets);
						weapon = i.Name;
						Console.WriteLine("{0} attack with {1} and rolls {2}", this.Name, weapon, attack);
						return attack;
					}			
			}
			// if being does not have weapon, attack with bare hands and with dice 6
			attack += Dice.Roll(6);
			Console.WriteLine("{0} attack and rolls {1}", this.Name, attack);
			return attack;			
		}
		
		/// <summary>
		/// Attack another being.
		/// </summary>
		/// <param name='b'>
		/// B.
		/// </param>
		public void Attack(Being b)
		{
			int attack = this.CalculateAttack();
			Console.WriteLine(b.Defend(attack));
		}
		
		/// <summary>
		/// Calculates the defence roll.
		/// </summary>
		/// <returns>
		/// The defence.
		/// </returns>
		public int CalculateDefence()
		{
			int defence = this.CurrentCharacteristics.defence;
			defence += Dice.Roll(6); // defende with 6 faceted dice
			return defence;
		}
		
		/// <summary>
		/// Calculates the initiative.
		/// </summary>
		/// <returns>
		/// The initiative.
		/// </returns>
		public int CalculateInitiative()
		{
			int speed = this.CurrentCharacteristics.speed;
			speed += Dice.Roll(6); // initiative is rolled with 6 faceted dice
			return speed;
		}
		
		/// <summary>
		/// Defend the specified attack.
		/// </summary>
		/// <param name='attack'>
		/// Attack.
		/// </param>
		public string Defend(int attack)
		{
			// calculate defence roll, injury and set message
			int defence = this.CalculateDefence();
			int injury = attack - defence;
			string msg = this.Name;
			// if injured
			if (injury>0)
			{
				msg += " has been injured for " + injury + " hitpoints";
				int hp = this.CurrentCharacteristics.hitpoints;
				hp -= injury;
				if (hp<=0) // if died
				{
					hp = 0;
					msg += " and died";
				}
				this.CurrentCharacteristics.hitpoints = hp;
			}
			else
				msg += " has defended himself";
			
			return msg;					
		}
		
		public bool Alive()
		{
			return (this.CurrentCharacteristics.hitpoints > 0);
		}
		
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Game.Being"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current <see cref="Game.Being"/>.
		/// </returns>
		public override string ToString ()
		{
			string s = String.Format("Name: {0}\n", this.Name);
			s += ("Hitpoints:  " + this.CurrentCharacteristics.hitpoints.ToString() + "/" + this.Characteristics.hitpoints.ToString());
			s += ("\nAttack:     " + this.CurrentCharacteristics.attack.ToString());
			s += ("\nDefence:    " + this.CurrentCharacteristics.defence.ToString());
			s += ("\nSpeed:      " + this.CurrentCharacteristics.speed.ToString());
			s += ("\nCurrently equiped are:\n" + this.equiped.ToString());
			s += ("\nIn inventory are:\n" + this.bag.ToString());
			return s;
		}
		
		public Corpse BecameCorpse()
		{
			foreach (Equipment e in this.equiped.bag)
			{
				this.bag.Add(e);
			}
			Corpse thisCorpse = new Corpse(string.Format("Corpse of {0}", this.Name), this.bag);
			return thisCorpse;
		}
		
		public virtual char Symbol()
		{
			return symbol;
		}
		
		public virtual void SetSymbol(char x)
		{
			this.symbol = x;
		}
				
		public virtual bool CanMoveTo()
		{
			return false;
		}
		
		public virtual bool CanDropItemOnto()
		{
			return false;
		}
		
		public virtual IPlace DropItemOnto(Item i)
		{
			return this;
		}
		
		public IPlace AutomaticAction (Player p)
		{
			// there always will be a script to handle interaction of player and another being
			return this;
		}
		
		public virtual void VoluntaryAction(Player p)
		{}
		
		public virtual XmlElement ToXml(XmlDocument doc, string elementName)
		{
			XmlElement being = doc.CreateElement(elementName);
			being.SetAttribute("name", Name); // set name attribute
			// add characteristics
			XmlElement ch = this.Characteristics.ToXml(doc, "Characteristics");
			being.AppendChild(ch);
			// add Current Characteristics
			XmlElement cch = this.CurrentCharacteristics.ToXml(doc, "CurrentCharacteristics");
			being.AppendChild(cch);
			// add bag
			XmlElement bag = this.bag.ToXml(doc, "Bag");
			being.AppendChild(bag);
			// add equiped items
			XmlElement equiped = this.equiped.ToXml(doc, "Equiped");
			being.AppendChild(equiped);
			// add body
			XmlElement body = this.Body.ToXml(doc, "Body");
			being.AppendChild(body);
			
			return being;
		}
		
	}
}

