using Game;
using System;
using System.Collections.Generic;

class Program
{
	
	public static void Main()
	{
		// some vizualization
		Console.BackgroundColor = ConsoleColor.White;
		Console.ForegroundColor = ConsoleColor.Black;		
		Console.Clear();
		
		// create world
		Map dungeon = new Map(15,5);
		
		//create dices
		Dice dice6 = new Dice(6);	
		
		// create walls
		for (int i = 0; i<10; i++){
			dungeon.AddLocation(new Location(i,0, new Wall()));
			dungeon.AddLocation(new Location(i,4, new Wall()));
		}		
		for (int i = 0; i<5; i++){
			dungeon.AddLocation(new Location(0,i,new Wall()));
			dungeon.AddLocation(new Location(9,i,new Wall()));
		}
		
		// doors and hallway behind them
		Door door = new Door("In front of you are solid wooden doors. You will need a key to open them.", "Rusty key", true);
		dungeon.AddLocation(new Location(9,2,door));
		
		dungeon.AddLocation(new Location(13,1,new Wall()));
		dungeon.AddLocation(new Location(13,2,new Wall()));
		dungeon.AddLocation(new Location(13,3,new Wall()));
		dungeon.AddLocation(new Location(12,1,new Wall()));
		dungeon.AddLocation(new Location(12,3,new Wall()));
		dungeon.AddLocation(new Location(11,1,new Wall()));
		dungeon.AddLocation(new Location(11,3,new Wall()));
		dungeon.AddLocation(new Location(10,1,new Wall()));
		dungeon.AddLocation(new Location(10,3,new Wall()));
		
		// key to the doors, small sword and chest toput them to
		Item key = new Item("Rusty key");
		Characteristics ch2 = new Characteristics(0, 5, 0, 2);	
		Weapon smallsword = new Weapon("Small sword", ch2, new List<string> {"weapon"}, dice6);
		Inventory chestInventory = new Inventory(10);
		chestInventory.Add(key);
		chestInventory.Add(smallsword);
		
		Chest chest = new Chest("Small wooden chest", chestInventory);
		
		dungeon.AddLocation(new Location(1,1,chest));
		
		// create goblin
		Characteristics ch_goblin = new Characteristics(15, 5, 1, 0);
		Being goblin = new Being("Goblin", ch_goblin, 10, dice6);
		Characteristics chSword = new Characteristics(0, 5, 0, 2);		
		Weapon smallsword2 = new Weapon("Small sword", chSword, new List<string> {"weapon"}, dice6);
		Characteristics chArmor = new Characteristics(0, 0, 2, -2);
		Equipment armor = new Equipment("Leather armor", chArmor, new List<string> {"body"});
		goblin.PickItem(smallsword2);
		goblin.PickItem(armor);
		goblin.EquipItem(armor);
		goblin.EquipItem(smallsword2);
		
		dungeon.AddLocation(new Location(11,2, goblin));
		
		// Create player
		Console.WriteLine("What is the name of your hero?");
		string name = Console.ReadLine().Trim();
		int choice = 0;
		string userInput;
		while ((choice!=1) && (choice!=2))
		{
			Console.WriteLine("What is body constitution of {0}", name);
			Console.WriteLine("Press 1 for strong and resilient");
			Console.WriteLine("Press 2 for agile and quick");
			userInput = Console.ReadLine();
			int.TryParse(userInput, out choice );
		}
		Characteristics ch;
		if (choice==1)
			ch = new Characteristics(50, 10, 3, -1);
		else
			ch = new Characteristics(35, 5, 7, 2);
		Player p = new Player(name, ch, 100, dice6, 3, 3);
		
		// main loop - runnig the program
		bool end = false;
		
		string message = "";
		while(!end)
		{
			Console.Clear();
			dungeon.CalculateVisibility(p);
			dungeon.Draw();			
			
			Console.CursorLeft = p.X;
			Console.CursorTop = p.Y;
			Console.Write(p.Symbol());
			
			Console.CursorTop = dungeon.Heigth + 5;
			Console.CursorLeft = 0;
			Console.WriteLine("Movement - arrows");
			Console.WriteLine("End - escape");
			Console.WriteLine("Inventory - Enter");
			Console.WriteLine();
			Console.WriteLine(message);
			
			ConsoleKeyInfo c = Console.ReadKey();
			if (c.Key == ConsoleKey.Escape)
				end = true;
			else if (c.Key == ConsoleKey.Enter)
				p.ManageInventory();
			p.Move(c, dungeon, out message);
			
			if (!p.Alive())
			{
				Console.WriteLine("Your dead! press any key");
				end = true;
				Console.ReadKey();
			}
				
		}		
		
	}
	
}
