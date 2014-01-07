using System;
using System.Collections.Generic;
using System.Xml;

namespace Game
{
	public class Inventory :IXml
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
		public string Remove(Item i)
		{
			string msg = ("Attempt to remove " + i.Name);
			bool removed = false;
			foreach(Item item in this.bag)
				if (item.Name == i.Name)
				{
					this.bag.Remove(item);
					removed = true;
					msg += "\nRemoved";
					break;
				}
			if (!removed)
				msg += ("\nItem is not in the Inventory and will not be removed");
			return msg;
		}
		
		public void RemoveAll()
		{
			this.bag = new List<Item>();
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
		public Item GetItem(string s)
		{
			foreach (Item i in this.bag)
				if (i.Name == s)
					return i;
			throw new ArgumentException("This item is not in the inventory.");
		}
		
		/// <summary>
		/// Is Item with given name inside inventory?.
		/// </summary>
		/// <param name='name'>
		/// If set to <c>true</c> name.
		/// </param>
		//
		public bool Inside(string name)
		{
			foreach (Item item in this.bag)
				if(item.Name == name)
					return true;
			return false;
		}
		
		
		public int Count()
		{
			return this.bag.Count;
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
		
		public XmlElement ToXml(XmlDocument doc, string elementName)
		{
			XmlElement inventory = doc.CreateElement(elementName);
			inventory.SetAttribute("maxsize", this.maxsize.ToString());
			foreach (IXml i in this.bag)
			{
				XmlElement xmli = i.ToXml(doc, i.GetType().ToString().Split('.')[1]);
				inventory.AppendChild(xmli);
			}
			return inventory;
		}
	}
}

