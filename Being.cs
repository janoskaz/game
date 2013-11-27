using System;
using System.Collections;
using System.Collections.Generic;

namespace Game
{
	public class Being: IPlace
	{
		// Everyone has to have a name
		protected string Name {get; private set;}
		
		// Everyone has basic characteristics
		protected Characteristics Characteristics {get; private set;}
		
		// Everyone has current characteristics
		protected Characteristics CurrentCharacteristics {get; private set;}
		
		// everyone has a bag
		public Inventory bag;
		
		// everyone has things, which are currently equiped
		protected Inventory equiped;
		
		// Everyone has a body
		private Body body = new Body( new List<string>());
		
		// Default dice for defence rolls and barehanded attacks
		public readonly Dice dice;
		
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
		public Being (string name, Characteristics ch, int bagsize, Dice dice)
		{
			this.Name = name;
			this.Characteristics = ch;
			this.CurrentCharacteristics = ch;
			this.bag = new Inventory(bagsize);
			this.equiped = new Inventory(bagsize);
			this.dice = dice;
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
		
		public bool HasItem(string itemName)
		{
			bool hasIt = (this.bag.Inside(itemName) || this.equiped.Inside(itemName));
			return hasIt;
		}
		
		/// <summary>
		/// Drop the item from the bag.
		/// </summary>
		/// <param name='name'>
		/// Name.
		/// </param>
		public string DropItem(string name)
		{
			string msg = this.bag.Remove(name);
			return msg;
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
		public bool CanEquip(string name)
		{
			Item i = this.bag.GetItem(name);
			return this.body.CanEquip(i);
		}
		
		/// <summary>
		/// Makes the space for equipment - strips equipment, which is in the way of current equipment.
		/// </summary>
		/// <param name='name'>
		/// Name.
		/// </param>
		public string MakeSpaceForEquipment(string name)
		{
			string msg = "Making space for new items";
			bool isInside = this.bag.Inside(name);
			if (isInside)
			{
				Item i = this.bag.GetItem(name);
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
					msg += ("\n" + this.StripItem(e.Name));
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
		public string EquipItem(string name, bool force = false)
		{
			string msg = "Attempt to equip and item";
			// can the thing be equiped?
			if (this.CanEquip(name))
			{
				// Is the item already inside bag?
				bool isInside = this.bag.Inside(name);
				if (isInside)
				{
					Item i = this.bag.GetItem(name);
					// can the Item be put on body? (are there empty slots?)
					bool canEquip = this.body.CanEquip(i);
					if (canEquip)
					{
						msg += this.bag.Remove(name); // remove from the bag
						msg += this.equiped.Add(i); // equip
						this.body.UpdateBody((Equipment)i, true); // update body slots
						this.UpdateCharacteristics((Equipment)i, true); // update current characteristics
						msg = String.Format("{0} has been equiped", name);
					}
						
				}
				else
					msg = String.Format("Item is not inside the bag and can not be equiped");	
			}
			else
			{
				if (force)
				{
					this.MakeSpaceForEquipment(name);
					this.EquipItem(name, false);
				}
				else
					msg = String.Format("{0} can not be equiped", name);			
			}
			return msg;
		}
				
		/// <summary>
		/// Strips the item fromequiped and put it into bag.
		/// </summary>
		/// <param name='name'>
		/// Name.
		/// </param>
		public string StripItem(string name)
		{
			Item i = this.equiped.GetItem(name);
			bool spaceInBag = (this.bag.bag.Count < this.bag.maxsize);
			string msg;
			if (spaceInBag)
			{
				this.equiped.Remove(name);
				this.bag.Add(i);
				this.body.UpdateBody((Equipment)i, false);
				this.UpdateCharacteristics((Equipment)i, false);
				msg = String.Format("{0} has been stripped and put into bag",name);
			}
			else
			{
				msg = String.Format("{0} can not be stripped, because the bag is full", name);
			}
			return msg;
		}
		
		/// <summary>
		/// Updates the characteristics by enhancement of given item. If item is equiped, add bvalues, if item is stripped, substract values
		/// </summary>
		public void UpdateCharacteristics(Equipment i, bool equip)
		{	
			int multiplier = 1;
			if (!equip)
				multiplier = -1;
			
			int hitpoints = i.Characteristics["hitpoints"] * multiplier;
			int	attack = i.Characteristics["attack"] * multiplier;
			int	defence = i.Characteristics["defence"] * multiplier;
			int	speed = i.Characteristics["speed"] * multiplier;
						
			this.CurrentCharacteristics.Update(hitpoints, attack, defence, speed);
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
			int attack = this.CurrentCharacteristics.GetValue("attack");
			Dice d = this.dice; 
			string weapon = "Bare hands"; 
			// if a weapon is equiped, find out the name and dice of this weapon
			foreach (Item i in this.equiped.bag)
			{
				if (i.GetType() == typeof(Weapon))
					{
						d = ((Weapon)i).dice;
						weapon = i.Name;
						break;
					}			
			}
			// calculate actual attack roll and return it
			attack += d.Roll();
			Console.WriteLine("{0} attack with {1} and rolls {2}", this.Name, weapon, attack);
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
			b.Defend(attack);
		}
		
		/// <summary>
		/// Calculates the defence roll.
		/// </summary>
		/// <returns>
		/// The defence.
		/// </returns>
		public int CalculateDefence()
		{
			int defence = this.CurrentCharacteristics.GetValue("defence");
			defence += this.dice.Roll();
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
			int speed = this.CurrentCharacteristics.GetValue("speed");
			speed += this.dice.Roll();
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
				int hp = this.CurrentCharacteristics.GetValue("hitpoints");
				hp -= injury;
				if (hp<=0) // if died
				{
					hp = 0;
					msg += " and died";
				}
				this.CurrentCharacteristics.SetValue("hitpoints", hp);
			}
			else
				msg += " has defended himself";
			
			return msg;					
		}
		
		public bool Alive()
		{
			return (this.CurrentCharacteristics.GetValue("hitpoints") > 0);
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
			s += this.CurrentCharacteristics.ToString();
			s += ("\nCurrently equiped are:\n" + this.equiped.ToString());
			s += ("\nIn inventory are:\n" + this.bag.ToString());
			return s;
		}
		
		public void ManageInventory()
		{
			bool runInventory = true;
			
			LimitedQueue<string> messageBoard = new LimitedQueue<string>(15);
			
			while (runInventory)
			{
				Console.Clear();
				Console.Write(this.ToString());
				Console.WriteLine();
				Console.WriteLine("To strip item, write 'strip #of_equipment'\n" +
					"To equip item, wrire 'equip #of_equipment'\n" +
					"To drop item from inventory, write 'drop #of_equipment'\n" +
					"To go back to game, write 'close'\n");
				
				foreach (string s in messageBoard)
				{
					Console.WriteLine(s);
				}
				
				string response = Console.ReadLine();
				messageBoard.Enqueue(response);
				string[] words = response.Split(' ');
				int n;
				bool isNum;
				switch (words[0])
				{
				case "close":
				{
					runInventory = false;
					break;
				}
				case "strip":
				{
					isNum = int.TryParse(words[1], out n);
					bool canStrip = this.equiped.bag.Count >= n;
					if (canStrip && isNum)
					{
						messageBoard.Enqueue(this.StripItem(this.equiped.bag[n-1].Name));
					}
					else
					{
						messageBoard.Enqueue("Can not strip this item");
					}
					break;
				}
				case "equip":
				{
					isNum = int.TryParse(words[1], out n);
					bool canEquip = this.bag.bag.Count >= n;
					bool isEquipment = this.bag.bag[n-1].GetType() == typeof(Equipment);
					if (canEquip && isNum && isEquipment)
					{
						messageBoard.Enqueue(this.EquipItem(this.bag.bag[n-1].Name,true));
					}
					else
					{
						messageBoard.Enqueue("Can not equip this item");
					}
					break;
				}
				case "drop":
				{
					isNum = int.TryParse(words[1], out n);
					bool canDrop = this.bag.bag.Count >= n;
					if (canDrop && isNum)
					{
						messageBoard.Enqueue(this.DropItem(this.bag.bag[n-1].Name));
					}
					else
					{
						messageBoard.Enqueue("Can not drop this item");
					}
					break;
				}
				default:
				{
					messageBoard.Enqueue("Can not recognize the command");
					break;
				}
				}
			
			}
			
		}
			
		public char Symbol()
		{
			return 'A';
		}
		
		public bool PerformAction (Player p, out string msg)
		{
			msg = "";
			return true;
		}
		
	}
}

