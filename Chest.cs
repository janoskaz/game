using System;
using System.Xml;
using System.Collections.Generic;
using ExtensionMethods;

namespace Game
{
	public class Chest :Item //default setting is to represent corpse - symbol is x and can be removed after looting
	{
		private string message;
		private string description;
		private bool removeIfEmpty = true;
		
		public Inventory Content {get; set;}
		
		public Chest (string name, Inventory content) :base(name)
		{
			Content = content;
			description = String.Format("There is {0} laying on the ground", Name.ToLower());
			message = "Time to loot:\nCONTENT";
			symbol = 'x';
		}
		public bool IsEmpty()
		{
			return Content.Count() == 0;
		}
		
		public void SetDescription(string desc)
		{
			this.description = desc;
		}
		
		public void SetMessage(string msg)
		{
			this.message = msg;
		}
		
		public void SetRemoving(bool val)
		{
			removeIfEmpty = val;
		}
		
		public bool CanBeRemoved()
		{
			return removeIfEmpty;
		}
		
		public override bool CanMoveTo()
		{
			return true;
		}
		
		public override bool CanDropItemOnto()
		{
			return true;
		}
		
		public override IPlace DropItemOnto(Item i)
		{
			this.Content.Add(i);
			return this;
		}
		
		public override IPlace AutomaticAction (Player p)
		{
			ThisGame.messageLog.Enqueue(description);
			return this;
		}
		
		public override void VoluntaryAction(Player p)
		{
			bool lootChest = true;
			
			LimitedQueue<string> messageBoard = new LimitedQueue<string>(10);
			List<string> commands = new List<string>();
			
			while (lootChest)
			{
				Console.Clear();
				Console.WriteLine(this.message);
				Console.Write(this.Content.ToString());
				Console.WriteLine();
				Console.WriteLine("YOUR INVENTORY");
				Console.Write(p.bag.ToString());
				Console.WriteLine();
				
				Console.WriteLine("To pick item, write 'pick #of_equipment' or write 'pick all' to take everything\n" +
					"To drop item from inventory, write 'drop #of_equipment'\n" +
					"To go back to game, press Escape\n");
				
				foreach (string s in messageBoard)
				{
					Console.WriteLine(s);
				}
				
				string response = commands.ListThroughCommands();
				commands.Add(response);
				messageBoard.Enqueue(response);
				string[] words = response.Split(' ');
				int n;
				switch (words[0])
				{
				case "close":
				{
					lootChest = false;
					break;
				}
				case "pick":
				{
					try
					{
						if (words[1] == "all")
						{
							if (p.bag.maxsize - p.bag.Count() - this.Content.Count() < 0)
							{
								messageBoard.Enqueue("You don't have enough space to take all items in your bag.");
								break;
							}
							foreach (Item i in this.Content.bag)
							{
								p.PickItem(i);
							}
							this.Content.RemoveAll();
							messageBoard.Enqueue("All item has been taken");
						} else {
							n = int.Parse(words[1]);
							if (p.bag.maxsize - p.bag.Count() > 0)
							{
								p.PickItem(this.Content.bag[n-1]);
								messageBoard.Enqueue(this.Content.bag[n-1].Name);
								this.Content.Remove(this.Content.bag[n-1]);
							}
							else
								messageBoard.Enqueue("You don't have enough space to take all items in your bag.");							
						}
					}
					catch
					{
						messageBoard.Enqueue("Something wrong with your command");
					}
					break;
				}
				case "drop":
				{
					try
					{
						n = int.Parse(words[1]);
						this.Content.Add(p.bag.GetItem(p.bag.bag[n-1].Name));
						messageBoard.Enqueue(p.bag.bag[n-1].Name);
						p.DropItem(p.bag.bag[n-1]);
					}
					catch
					{
						messageBoard.Enqueue("Something wrong with your command");
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
		
		public override XmlElement ToXml (XmlDocument doc, string elementName)
		{
			XmlElement chest = doc.CreateElement("Chest");		
			chest.SetAttribute("name", this.Name);
			chest.SetAttribute("removeIfEmpty", CanBeRemoved().ToString());
			chest.SetAttribute("symbol", symbol.ToString());
			XmlElement inv = this.Content.ToXml(doc, "Inventory");
			chest.AppendChild(inv);
			return chest;
		}
		
	}
}