using System;
using System.Collections.Generic;

namespace Game
{
	public class Inventory
	{
		/// <summary>
		/// Number of items, which can be stored in an inventory.
		/// </summary>
		public readonly int maxsize;
		
		/// <summary>
		/// The inventory itself - list of items.
		/// </summary>
		public List<Item> bag;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Game.Inventory"/> class.
		/// </summary>
		/// <param name='maxsize'>
		/// Maxsize.
		/// </param>
		public Inventory (int maxsize)
		{
			this.maxsize = maxsize;
			this.bag = new List<Item>();
		}
		
		/// <summary>
		/// Add Item to inventory, if there is space.
		/// </summary>
		/// <param name='i'>
		/// I.
		/// </param>
		public string Add(Item i)
		{
			string msg = "Adding item to bag";
			if(this.bag.Count <= this.maxsize)
				this.bag.Add(i);
			else
				msg += "\nInventory is full";
			return msg;
		}
		
		/// <summary>
		/// Remove the specified Item from inventory, unless it is missing - in that case give warning message.
		/// </summary>
		/// <param name='i'>
		/// I.
		/// </param>
		public string Remove(string s)
		{
			string msg = ("Attempt to remove " + s);
			bool removed = false;
			foreach(Item i in this.bag)
				if (i.Name == s)
				{
					this.bag.Remove(i);
					removed = true;
					msg += "\nRemoved";
					break;
				}
			if (!removed)
				msg += ("\nItem is not in the Inventory and will not be removed");
			return msg;
		}
		
		/// <summary>
		/// Returns one item with given name from the Inventory.
		/// </summary>
		/// <returns>
		/// The item.
		/// </returns>
		/// <param name='name'>
		/// Name.
		/// </param>
		/// <exception cref='ArgumentException'>
		/// Is thrown when an argument passed to a method is invalid.
		/// </exception>
		public Item GetItem(string name)
		{
			foreach (Item i in this.bag)
				if (i.Name == name)
					return i;
			throw new ArgumentException("This item is not in the inventory.");
		}
		
		/// <summary>
		/// Is Item with given name inside inventory?.
		/// </summary>
		/// <param name='name'>
		/// If set to <c>true</c> name.
		/// </param>
		public bool Inside(string name)
		{
			foreach (Item i in this.bag)
				if(i.Name == name)
					return true;
			return false;
		}
		
		/// <summary>
		/// Write names of all items in inventory.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current <see cref="Game.Inventory"/>.
		/// </returns>
		public override string ToString ()
		{
			string s = "";
			int position = 1;
			foreach (Item i in this.bag)
			{
				s += (position + " " + i.Name + "\n");
				position++;
			}
			return s;
		}
	}
}

