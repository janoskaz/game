using Game;
using System;
using System.Collections.Generic;

class Program
{
	
	public static void Main()
	{
		//create dices
		Dice dice6 = new Dice(6);
		
		/*
		// create empty inventory
		Inventory bag = new Inventory(10);
				
		// create big sword and put it into bag
		Characteristics ch1 = new Characteristics(0, 10, 0, 0);		
		Weapon bigsword = new Weapon("Big sword", ch1, new List<string> {"weapon","shield"}, dice6);
		bag.Add(bigsword);
		
		// create small sword and put it into bag
		Characteristics ch2 = new Characteristics(0, 5, 0, 2);		
		Weapon smallsword = new Weapon("Small sword", ch2, new List<string> {"weapon"}, dice6);
		bag.Add(smallsword);
		
		// create armor and put it into bag
		Characteristics ch3 = new Characteristics(0, 0, 5, -2);
		Equipment armor = new Equipment("Leather armor", ch3, new List<string> {"body"});
		bag.Add(armor);
		
		// create ring and put it into bag
		Characteristics ch4 = new Characteristics(10, 0, 0, 0);
		Equipment ring = new Equipment("Ring", ch4, new List<string>());
		bag.Add(ring);
		
		// create orc
		Characteristics ch_orc = new Characteristics(50, 10, 3, -1);
		Being orc = new Being("Orc", ch_orc, 10, dice6);
		
		// create goblin
		Characteristics ch_goblin = new Characteristics(40, 8, 5, 1);
		Being goblin = new Being("Goblin", ch_goblin, 10, dice6);
		
		
		//orc.PickItem(bigsword);
		//orc.PickItem(ring);		
		//orc.EquipItem("Big sword");
		//orc.EquipItem("Ring");
		
		goblin.PickItem(smallsword);
		goblin.PickItem(armor);
		goblin.PickItem(bigsword);
		goblin.PickItem(ring);
		goblin.EquipItem("Leather armor");
		goblin.EquipItem("Small sword");
		goblin.EquipItem("Big sword");
		goblin.EquipItem("Big sword", true);
		
		Console.WriteLine();
		Console.WriteLine("##############################");
		Console.WriteLine(orc);
		Console.WriteLine("##############################");
		Console.WriteLine(goblin);
		
		// create simple map
		Map newMap = new Map(10,5);
		// create new world
		World middleEarth = new World(newMap);
		
		middleEarth.Fight(orc, goblin);	
		
		goblin.ManageInventory();
		*/
		
		// create world
		Map dungeon = new Map(15,5);
		
		// some vizualization
		Console.BackgroundColor = ConsoleColor.White;
		Console.ForegroundColor = ConsoleColor.Black;
		Console.Clear();	
		
		// create walls
		// counting from 0!
		for (int i = 0; i<10; i++){
			dungeon.AddLocation(new Location(i,0, new Wall()));
			dungeon.AddLocation(new Location(i,4, new Wall()));
		}		
		for (int i = 0; i<5; i++){
			dungeon.AddLocation(new Location(0,i,new Wall()));
			dungeon.AddLocation(new Location(9,i,new Wall()));
		}
		
		Door door = new Door("In front of you are solid wooden doors. You will need a key to open them.", "Rusty key", true);
		dungeon.AddLocation(new Location(9,3,door));
		
		Item key = new Item("Rusty key");
		Characteristics ch2 = new Characteristics(0, 5, 0, 2);	
		Weapon smallsword = new Weapon("Small sword", ch2, new List<string> {"weapon"}, dice6);
		Inventory chestInventory = new Inventory(10);
		chestInventory.Add(key);
		chestInventory.Add(smallsword);
		
		Chest chest = new Chest("Small wooden chest", chestInventory);
		
		dungeon.AddLocation(new Location(1,1,chest));
		
		dungeon.AddLocation(new Location(11,2,new Wall()));
		dungeon.AddLocation(new Location(11,4,new Wall()));
		dungeon.AddLocation(new Location(10,2,new Wall()));
		dungeon.AddLocation(new Location(10,4,new Wall()));
		
		
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
		
		
		bool end = false;
		
		string message = "";
		while(!end)
		{
			Console.Clear();
			dungeon.Draw();			
			
			Console.CursorLeft = p.X;
			Console.CursorTop = p.Y;
			Console.Write(p.Symbol());
			
			Console.CursorTop = dungeon.Heigth + 5;
			Console.CursorLeft = 0;
			Console.WriteLine("Movement - arrows");
			Console.WriteLine("End - escape");
			Console.WriteLine("Inventory - enter");
			Console.WriteLine();
			Console.WriteLine(message);
			
			ConsoleKeyInfo c = Console.ReadKey();
			if (c.Key == ConsoleKey.Escape)
				end = true;
			else if (c.Key == ConsoleKey.Enter)
				p.ManageInventory();
			p.Move(c, dungeon, out message);
			
		}		
		
	}
	
}
