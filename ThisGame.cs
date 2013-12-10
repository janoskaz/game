using System;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace Game
{
	public class ThisGame
	{
		public ThisGame ()
		{
		}
		
		public void RunGame()
		{
			Map dungeon = LoadMap("map.dat");

			Dice dice6 = new Dice(6);
			
			Player p = InitializePlayer(dice6);
			
// test - creating XML
//create dices
					
// key to the doors, small sword and chest toput them to
Item key = new Item("Rusty key");
Characteristics ch2 = new Characteristics(0, 5, 0, 2);	
Weapon smallsword = new Weapon("Small sword", ch2, new List<string> {"weapon"}, dice6);
Inventory chestInventory = new Inventory(10);
chestInventory.Add(key);
chestInventory.Add(smallsword);
			
Chest chest = new Chest("Small wooden chest", chestInventory);

dungeon.AddLocation(new Location(1,1,chest));
			
// add goblin
Characteristics ch_goblin = new Characteristics(15, 5, 1, 0);
Being goblin = new Being("Goblin", ch_goblin, ch_goblin, 10, dice6);
Characteristics chSword = new Characteristics(0, 5, 0, 2);		
Weapon smallsword2 = new Weapon("Small sword", chSword, new List<string> {"weapon"}, dice6);
Characteristics chArmor = new Characteristics(0, 0, 2, -2);
Equipment armor = new Equipment("Leather armor", chArmor, new List<string> {"body"});
goblin.PickItem(smallsword2);
goblin.PickItem(armor);
goblin.EquipItem(armor);
goblin.EquipItem(smallsword2);
	
dungeon.AddLocation(new Location(11,2, goblin));
			
//dungeon.ToXml("testmap.xml");
//p.SaveAsXml();
p = this.LoadFromXml("Odin");
			//
			
			// main loop - running the program
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
				Console.WriteLine("Save current player - press S");
				Console.WriteLine();
				Console.WriteLine(message);
				
				ConsoleKeyInfo c = Console.ReadKey();
				if (c.Key == ConsoleKey.Escape)
					end = true;
				else if (c.Key == ConsoleKey.Enter)
					p.ManageInventory();
				else if (c.Key == ConsoleKey.S)
				{
					p.Save(out message);
					continue;
				}					
					
				p.Move(c, dungeon, out message);
				
				if (!p.Alive())
				{
					Console.WriteLine("Your dead! press any key");
					end = true;
					Console.ReadKey();
				}
					
			}
			dungeon.ToXml("testmap.xml");
			
		}
		
		public Map LoadMap(string filename)
		{
			Map newmap = new Map();
			
			string startupPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName,"files/"+filename);
			
			using (StreamReader sr = new StreamReader(startupPath))
			{
				string s;
				while ((s = sr.ReadLine()) != null)
				{
					try
					{
						string[] splitted = s.Split(';');
						string klass = splitted[0];
						int x = int.Parse (splitted[1]);
						int y = int.Parse (splitted[2]);
						switch(klass)
						{
						case "Map":
						{
							newmap.CreateMapField(x,y);
							break;
						}
						case "Wall":
						{
							newmap.AddLocation( new Location( x,y, new Wall() ) );
							break;
						}
						case "Door":
						{
							string msg = splitted[3];
							string keyname = splitted[4];
							bool locked = splitted[5] == "true";
							newmap.AddLocation( new Location( x,y, new Door(msg, keyname, locked)  ) );
							break;
						}
								
						}
					}
					catch
					{
						Console.WriteLine("Configuration file is incorrect.");
					}
					
				}
			}
			return newmap;
		}
		
		/// <summary>
		/// Loads the player from a file.
		/// </summary>
		/// <returns>
		/// The player.
		/// </returns>
		/// <param name='playername'>
		/// Playername.
		/// </param>
		/// <param name='dice'>
		/// Dice.
		/// </param>
		public Player LoadPlayer(string playername, Dice dice)
		{
			// set path to the file with player
			string startupPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName,"files/"+playername.ToLower() + ".plr");
			
			// read all lines to an array
			string[] lines = File.ReadAllLines(startupPath);
			
			// read first line with player name and position
			string[] line1 = lines[0].Split(';');
			string name = line1[1];
			int x = int.Parse(line1[2]);
			int y = int.Parse(line1[3]);
			// read second line with his characteristics
			string[] line2 = lines[1].Split(';');
			Characteristics ch = new Characteristics( int.Parse(line2[1]), int.Parse(line2[2]), int.Parse(line2[3]), int.Parse(line2[4]));
			// read third line with his characteristics
			string[] line3 = lines[2].Split(';');
			Characteristics currentCh = new Characteristics(
				ch.hitpoints += int.Parse(line3[1]),
				ch.attack += int.Parse(line3[2]),
				ch.defence += int.Parse(line3[3]),
				ch.speed += int.Parse(line3[4]));
			// read fourth line with bagsize
			string[] line4 = lines[3].Split(';');
			int bagsize = int.Parse(line4[1]);
			
			// create player
			Player newplayer = new Player(name, ch, ch,bagsize , dice, x, y);
			
			// read line after line and 
			for (int n = 4; n<lines.Length; n++)
			{
				string s = lines[n];
				string[] splitted = s.Split(';');
				string klass = splitted[1];
				switch(klass)
				{
					case "Game.Item":
					{
					Item i = new Item(splitted[2]);
					if(splitted[0] == "Bag")
						newplayer.PickItem(i);
					break;
					}
					case "Game.Weapon":
					{
					List<string> bodyparts = new List<string>();
					if(splitted[3]=="True")
						bodyparts.Add("head");
					if(splitted[4]=="True")
						bodyparts.Add("body");
					if(splitted[5]=="True")
						bodyparts.Add("legs");
					if(splitted[6]=="True")
						bodyparts.Add("boots");
					if(splitted[7]=="True")
						bodyparts.Add("weapon");
					if(splitted[8]=="True")
						bodyparts.Add("shield");
								
					Characteristics ch2 = new Characteristics(int.Parse(splitted[9]), int.Parse(splitted[10]), int.Parse(splitted[11]), int.Parse(splitted[12]));
							
					Weapon w = new Weapon(splitted[2], ch2, bodyparts, dice);
								
					if(splitted[0] == "Bag")
						newplayer.PickItem(w);
					else
					{
						newplayer.PickItem(w);
						newplayer.EquipItem(w);
					}							
					break;
				}
				case "Game.Equipment":
				{
					List<string> bodyparts = new List<string>();
					if(splitted[3]=="true")
						bodyparts.Add("head");
					if(splitted[4]=="true")
						bodyparts.Add("body");
					if(splitted[5]=="true")
						bodyparts.Add("legs");
					if(splitted[6]=="true")
						bodyparts.Add("boots");
					if(splitted[7]=="true")
						bodyparts.Add("weapon");
					if(splitted[8]=="true")
						bodyparts.Add("shield");
								
					Characteristics ch2 = new Characteristics(int.Parse(splitted[9]), int.Parse(splitted[10]), int.Parse(splitted[11]), int.Parse(splitted[12]));
								
					Equipment e = new Weapon(splitted[2], ch2, bodyparts, dice);
					
					if(splitted[0] == "Bag")
						newplayer.PickItem(e);
					else
					{
						newplayer.PickItem(e);
						newplayer.EquipItem(e);
					}							
					break;
					}
					default:
					{
						break;
					}
				}
			newplayer.SetCurrentCharacteriscs(currentCh);
			}
			return newplayer;
		
	}
		
		public Player InitializePlayer(Dice dice)
		{
			string startupPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName,"files");
			
			List<string> players = new List<string>();
			
			// initialize empty player, which will be overriden
			Player p = new Player("name", new Characteristics(0,0,0,0), new Characteristics(0,0,0,0), 10, dice, 0, 0);
			// list of all players ctored in a database
			DirectoryInfo dir = new DirectoryInfo(startupPath);
			foreach (FileInfo file in dir.GetFiles())
			{
				string filename = file.Name;
				if (filename.EndsWith(".plr"))
				{
					string newname = char.ToUpper(filename[0]) + filename.Substring(1); // correct name with first capital letter
					players.Add (newname.Replace(".plr",""));
				}
			}
			
			// ask player, if he want to chose from existing players, or create a new one
			if (players.Count == 0)
			{
				p = CreateNewPlayer(dice);
				return p;
			}
			
			bool ask = true;
			
			while (ask)
			{
				Console.WriteLine("Do you wish to create a new player? [y/n]");
				char answer = Console.ReadKey().KeyChar;
				Console.WriteLine();
				switch(answer)
				{
				case 'y':
				{
					p = CreateNewPlayer(dice);
					ask = false;
					break;
				}
				case 'n':
				{
					p = PickCharacter(players, dice);
					ask = false;
					break;
				}
				default:
				{
					break;
				}
				}				
				
			}
			return p;
					
		}
		
		public Player CreateNewPlayer(Dice dice)
		{
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
			Player p = new Player(name, ch, ch, 100, dice, 3, 3);
			return p;
		}
		
		public Player PickCharacter(List<string> players, Dice dice)
		{
			// create empty player, which will be overriden
			Player p = new Player("name", new Characteristics(0,0,0,0), new Characteristics(0,0,0,0), 10, dice, 0, 0);
			bool chosen = false;
			while(!chosen)
			{
				Console.WriteLine("Which characters should I load?");
				Console.WriteLine();
				
				int i = 1;
				foreach (string name in players)
				{
					Console.WriteLine("{0}: {1}", i, name);
				}
				
				string n = Console.ReadLine();
				try
				{
					int nint = int.Parse(n)-1;
					p = LoadPlayer(players[nint], dice);
					chosen = true;
				}
				catch
				{
					Console.WriteLine("Your input was incorrect");
				}
			}
			return p;
		}
		
		public Player LoadFromXml(string playername)
		{
			string path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName,"files/"+playername + ".xml");
			
			XmlDocument doc = new XmlDocument();
			doc.Load(path);
			
			XmlNode root = doc.DocumentElement;
						
			// get attributes of player - name and positions
			string name = root.Attributes["name"].Value;
			int x = int.Parse(root.Attributes["x"].Value);
			int y = int.Parse(root.Attributes["y"].Value);
			Console.WriteLine("name: {0}; x: {1}; y: {2}", name, x, y);
			
			Characteristics ch = new Characteristics(0,0,0,0);
			Characteristics cch = new Characteristics(0,0,0,0);
			Inventory bag = new Inventory(0);
			Inventory equiped = new Inventory(0);
			List<string> b = new List<string>();
			
			Console.ReadKey();
			
			foreach (XmlNode node in root.ChildNodes)
			{
				string nodeName = node.Name;
				switch (nodeName)
				{
				case "Characteristics":
				{
					ch = LoadCharacteristicsFromXml((XmlElement)node);
					break;
				}
				case "CurrentCharacteristics":
				{
					cch = LoadCharacteristicsFromXml((XmlElement)node);
					break;
				}
				case "Bag":
				{
					bag = LoadInventoryFromXml((XmlElement)node);
					break;
				}
				case "Equiped":
				{
					equiped = LoadInventoryFromXml((XmlElement)node);
					break;
				}
				case "Body":
				{
					b = LoadBodyFromXML((XmlElement)node);
					break;
				}
					
				}
				
			}
			Player p = new Player(name, ch, cch, bag.maxsize, new Dice(6), x, y);
			p.bag = bag;
			p.equiped = equiped;
			p.SetBody(new Body(b));
			Console.WriteLine(p.ToString());
			Console.ReadKey();
			
			return p;
		}
		
		public Characteristics LoadCharacteristicsFromXml(XmlElement node)
		{
			int hp = int.Parse (node.GetAttribute("hitpoints"));
			int attack = int.Parse (node.GetAttribute("attack"));
			int defence = int.Parse (node.GetAttribute("defence"));
			int speed = int.Parse (node.GetAttribute("speed"));
			Characteristics ch = new Characteristics(hp, attack, defence, speed);
			return ch;
		}
		
		public List<string> LoadBodyFromXML(XmlElement node)
		{
			List<string> lst = new List<string>();
			if (bool.Parse (node.GetAttribute("body")))
				lst.Add("body");
			if (bool.Parse (node.GetAttribute("head")))
				lst.Add("head");
			if (bool.Parse (node.GetAttribute("legs")))
				lst.Add("legs");
			if (bool.Parse (node.GetAttribute("boots")))
				lst.Add("boots");
			if (bool.Parse (node.GetAttribute("weapon")))
				lst.Add("weapon");
			if (bool.Parse (node.GetAttribute("shield")))
				lst.Add("shield");
			return lst;
		}
		
		public Item LoadItemFromXml(XmlElement node)
		{
			string name = node.GetAttribute("name");
			return new Item(name);
		}
		
		public Equipment LoadEquipmentFromXml(XmlElement node)
		{
			string name = node.GetAttribute("name");
			Characteristics ch = LoadCharacteristicsFromXml((XmlElement)node.GetElementsByTagName("Characteristics")[0]);
			List<string> b = LoadBodyFromXML((XmlElement)node.GetElementsByTagName("Body")[0]);
			return new Equipment(name, ch, b);
		}
		
		public Weapon LoadWeaponFromXml(XmlElement node)
		{
			string name = node.GetAttribute("name");
			Characteristics ch = LoadCharacteristicsFromXml((XmlElement)node.GetElementsByTagName("Characteristics")[0]);
			List<string> b = LoadBodyFromXML((XmlElement)node.GetElementsByTagName("Body")[0]);
			return new Weapon(name, ch, b, new Dice(6));
		}
		
		public Inventory LoadInventoryFromXml(XmlElement node)
		{
			int bagsize = int.Parse(node.GetAttribute("maxsize"));
			Inventory inv = new Inventory(bagsize);
			foreach (XmlNode child in node.ChildNodes)
			{
				XmlElement childElement = (XmlElement)child;
				string name = childElement.Name;
				switch(name)
				{
				case "Item":
				{
					inv.Add(LoadItemFromXml(childElement));
					break;
				}
				case "Equipment":
				{
					inv.Add(LoadEquipmentFromXml(childElement));
					break;
				}
				case "Weapon":
				{
					inv.Add(LoadWeaponFromXml(childElement));
					break;
				}
				}
			}
			return inv;
		}
	}
}