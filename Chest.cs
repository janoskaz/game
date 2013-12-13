using System;
using System.Xml;

namespace Game
{
	public class Chest :Item
	{
		protected string message;
		protected string description;
		
		public Inventory Content {get; set;}
		
		public Chest (string name, Inventory content) :base(name)
		{
			Content = content;
			description = "There is a chest in front of you.";
			message = "You have opened a chest\nCONTENT OF CHEST";
		}
		
		public override char Symbol()
		{
			return 'o';
		}
		
		public override bool PerformAction (Player p, Location l, out Location l2)
		{
			ThisGame.messageLog.Enqueue(description);
			l2 = l;
			
			return true;
		}
		
		public override void VoluntaryAction(Player p)
		{
			bool lootChest = true;
			
			LimitedQueue<string> messageBoard = new LimitedQueue<string>(10);
			
			while (lootChest)
			{
				Console.Clear();
				Console.WriteLine(this.message);
				Console.Write(this.Content.ToString());
				Console.WriteLine();
				Console.WriteLine("YOUR INVENTORY");
				Console.Write(p.bag.ToString());
				Console.WriteLine();
				
				Console.WriteLine("To pick item, write 'pick #of_equipment'\n" +
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
						n = int.Parse(words[1]);
						p.PickItem(this.Content.bag[n-1]);
						messageBoard.Enqueue(this.Content.bag[n-1].Name);
						this.Content.Remove(this.Content.bag[n-1]);
					}
					catch
					{
						messageBoard.Enqueue("Something wrong with your output");
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
						messageBoard.Enqueue("Something wrong with your output");
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
			XmlElement inv = this.Content.ToXml(doc, "Inventory");
			chest.AppendChild(inv);
			return chest;
		}
		
	}
}