using System;

namespace Game
{
	public class Chest :Item
	{
		public Inventory Content {get; set;}
		
		public Chest (string name, Inventory content) :base(name)
		{
			Content = content;
		}
		
		public override char Symbol()
		{
			return 'X';
		}
		
		public override bool PerformAction (Player p, out string msg)
		{
			msg = "";
			
			bool lootChest = true;
			
			LimitedQueue<string> messageBoard = new LimitedQueue<string>(10);
			
			while (lootChest)
			{
				Console.Clear();
				Console.WriteLine("CONTENT OF CHEST");
				Console.Write(this.Content.ToString());
				Console.WriteLine();
				Console.WriteLine("YOUR INVENTORY");
				Console.Write(p.bag.ToString());
				Console.WriteLine();
				
				Console.WriteLine("To equip item, write 'pick #of_equipment'\n" +
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
					lootChest = false;
					break;
				}
				case "pick":
				{
					isNum = int.TryParse(words[1], out n);
					bool canEquip = this.Content.bag.Count >= n;
					if (canEquip && isNum)
					{
						p.PickItem(this.Content.bag[n-1]);
						messageBoard.Enqueue(this.Content.bag[n-1].Name);
						this.Content.Remove(this.Content.bag[n-1].Name);
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
					bool canDrop = p.bag.bag.Count >= n;
					if (canDrop && isNum)
					{
						this.Content.Add(p.bag.GetItem(p.bag.bag[n-1].Name));
						messageBoard.Enqueue(p.bag.bag[n-1].Name);
						p.DropItem(p.bag.bag[n-1].Name);
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
			return true;
		}
	}
}

