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
			//Map dungeon = LoadMap("map.dat");
			Dice dice6 = new Dice(6);

			Player p = InitializePlayer(dice6);
						
			Map dungeon = InitializeMap(p, dice6);						
			
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
					p.SaveAsXml(out message);
					dungeon.ToXml(p.Name.ToLower());
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
			
		}
		
		public Map InitializeMap(Player p, Dice dice)
		{			
			string mapname = p.Name + "_map";
			
			try
			{
				return LoadMapFromXml(mapname, dice);
			}
			catch
			{
				return LoadMapFromXml("defaultmap", dice);
			}
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
				if (filename.EndsWith("_plr.xml"))
				{
					string newname = char.ToUpper(filename[0]) + filename.Substring(1); // correct name with first capital letter
					players.Add (newname.Replace("_plr.xml",""));
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
					i++;
				}
				
				string n = Console.ReadLine();
				try
				{
					int nint = int.Parse(n)-1;
					p = LoadPlayerFromXml(players[nint], dice);
					chosen = true;
				}
				catch
				{
					Console.WriteLine("Your input was incorrect");
				}
			}
			return p;
		}
		
		public Player LoadPlayerFromXml(string playername, Dice dice)
		{
			string path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName,"files/"+playername.ToLower() + "_plr.xml");
			
			XmlDocument doc = new XmlDocument();
			doc.Load(path);
			
			XmlNode root = doc.DocumentElement;
						
			// get attributes of player - name and positions
			string name = root.Attributes["name"].Value;
			int x = int.Parse(root.Attributes["x"].Value);
			int y = int.Parse(root.Attributes["y"].Value);
			
			Characteristics ch = new Characteristics(0,0,0,0);
			Characteristics cch = new Characteristics(0,0,0,0);
			Inventory bag = new Inventory(0);
			Inventory equiped = new Inventory(0);
			List<string> b = new List<string>();
			
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
					bag = LoadInventoryFromXml((XmlElement)node, dice);
					break;
				}
				case "Equiped":
				{
					equiped = LoadInventoryFromXml((XmlElement)node, dice);
					break;
				}
				case "Body":
				{
					b = LoadBodyFromXML((XmlElement)node);
					break;
				}
					
				}
				
			}
			Player p = new Player(name, ch, cch, bag.maxsize, dice, x, y);
			p.bag = bag;
			p.equiped = equiped;
			p.SetBody(new Body(b));
			
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
		
		public Weapon LoadWeaponFromXml(XmlElement node, Dice dice)
		{
			string name = node.GetAttribute("name");
			Characteristics ch = LoadCharacteristicsFromXml((XmlElement)node.GetElementsByTagName("Characteristics")[0]);
			List<string> b = LoadBodyFromXML((XmlElement)node.GetElementsByTagName("Body")[0]);
			return new Weapon(name, ch, b, dice);
		}
		
		public Inventory LoadInventoryFromXml(XmlElement node, Dice dice)
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
					inv.Add(LoadWeaponFromXml(childElement, dice));
					break;
				}
				}
			}
			return inv;
		}
		
		public Map LoadMapFromXml(string mapname, Dice dice)
		{
			string path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName,"files/"+mapname.ToLower() + ".xml");
			
			XmlDocument doc = new XmlDocument();
			doc.Load(path);
			
			XmlNode root = doc.DocumentElement;
						
			// get attributes of map - width and heigth
			int x = int.Parse(root.Attributes["width"].Value);
			int y = int.Parse(root.Attributes["heigth"].Value);
			
			Map newmap = new Map();
			newmap.CreateMapField(x,y);
			
			foreach (XmlNode node in root.ChildNodes)
			{
				Location l = LoadLocationFromXml((XmlElement)node, dice);
				newmap.AddLocation(l);
			}
			
			return newmap;
		}
		
		public Location LoadLocationFromXml(XmlElement node, Dice dice)
		{
			int x = int.Parse(node.Attributes["x"].Value);
			int y = int.Parse(node.Attributes["y"].Value);
			bool visible = bool.Parse(node.Attributes["visible"].Value);
			IPlace block = LoadBlockFromXml((XmlElement)node.GetElementsByTagName("block")[0], dice);
			Location l = new Location(x, y, block);
			l.Visible = visible;
			return l;
		}
		
		public IPlace LoadBlockFromXml(XmlElement node, Dice dice)
		{
			string type = node.Attributes["type"].Value;
			
			switch (type)
			{
			case "Game.BasicObject":
			{
				return new BasicObject();
			}
			case "Game.Wall":
			{
				return new Wall();
			}
			case "Game.Door":
			{
				return LoadDoorFromXml((XmlElement)node.GetElementsByTagName("Door")[0]);
			}
			case "Game.Chest":
			{
				return LoadChestFromXml((XmlElement)node.GetElementsByTagName("Chest")[0], dice);
			}
			case "Game.Corpse":
			{
				return LoadCorpseFromXml((XmlElement)node.GetElementsByTagName("Corpse")[0], dice);
			}
			case "Game.Being":
			{
				return LoadBeingFromXml((XmlElement)node.GetElementsByTagName("Being")[0], dice);
			}
			default:
			{
				return new BasicObject();
			}
			}
		}
		
		public Door LoadDoorFromXml(XmlElement node)
		{
			bool locked = bool.Parse(node.Attributes["locked"].Value);
			string msg = node.GetElementsByTagName("Message")[0].InnerText;
			string keyname = node.GetElementsByTagName("Keyname")[0].InnerText;
			return new Door(msg, keyname, locked);
		}
		
		public Chest LoadChestFromXml(XmlElement node, Dice dice)
		{
			string name = node.Attributes["name"].Value;
			XmlElement child = (XmlElement)node.GetElementsByTagName("Inventory")[0];
			Inventory inv = LoadInventoryFromXml(child, dice);
			return new Chest(name, inv);
		}
		
		public Corpse LoadCorpseFromXml(XmlElement node, Dice dice)
		{
			string name = node.Attributes["name"].Value;
			XmlElement child = (XmlElement)node.GetElementsByTagName("Inventory")[0];
			Inventory inv = LoadInventoryFromXml(child, dice);
			return new Corpse(name, inv);
		}
		
		public Being LoadBeingFromXml(XmlElement node, Dice dice)
		{
			// get name of being
			string name = node.Attributes["name"].Value;
			
			Characteristics ch = new Characteristics(0,0,0,0);
			Characteristics cch = new Characteristics(0,0,0,0);
			Inventory bag = new Inventory(0);
			Inventory equiped = new Inventory(0);
			List<string> b = new List<string>();
			
			Console.WriteLine(node.ChildNodes.Count);
			
			foreach (XmlNode subnode in node.ChildNodes)
			{
				string nodeName = subnode.Name;
				switch (nodeName)
				{
				case "Characteristics":
				{
					ch = LoadCharacteristicsFromXml((XmlElement)subnode);
					break;
				}
				case "CurrentCharacteristics":
				{
					cch = LoadCharacteristicsFromXml((XmlElement)subnode);
					break;
				}
				case "Bag":
				{
					bag = LoadInventoryFromXml((XmlElement)subnode, dice);
					break;
				}
				case "Equiped":
				{
					equiped = LoadInventoryFromXml((XmlElement)subnode, dice);
					break;
				}
				case "Body":
				{
					b = LoadBodyFromXML((XmlElement)subnode);
					break;
				}
					
				}
				
			}
			Being being = new Being(name, ch, cch, bag.maxsize, dice);
			being.bag = bag;
			being.equiped = equiped;
			being.SetBody(new Body(b));
			
			return being;
		}
	}
}