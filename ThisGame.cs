using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Threading;
using System.Globalization;
using NLua;
using ExtensionMethods;

namespace Game
{
	public static class ThisGame
	{
		
		public static LimitedQueue<string> messageLog = new LimitedQueue<string>(6);
		
		public static Map dungeon;
		
		public static Player player;
		
		public static int mapHeight = 10;
		
		public static string filePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName,"files/");
		
		public static Lua lua = new Lua();
		
		public static int Visibility = 0;
		
		//public static string currentDungeon;

		public static void RunGame()
		{
			// first load lua scripts
			lua.DoFile(filePath + "luascripts/startup.lua");
			lua.DoFile(filePath + "luascripts/config.lua");
			
			// initialize player, which includes loading lua file, if player is loaded
			bool newgame = true;
			InitializePlayer(out newgame);
			
			//initialize map for player
			InitializeMap(newgame, "dungeon1");
			
			// set coordinates of a player
			player.X = dungeon.PlayerX;
			player.Y = dungeon.PlayerY;
			
			// pass variables to lua
			lua["player"] = player;
			lua["map"] = dungeon;
			lua["path_to_files"] = filePath;
			
			// main loop - running the program
			bool end = false;
			
			while(!end)
			{
				Console.Clear();
				
				// set visibility of the player
				double vis = (double)lua["config.visibility"];
				SetVisibility((int)vis);
				
				if (ThisGame.Visibility > 0)
					dungeon.CalculateVisibility(player, Visibility);
				dungeon.Draw(player);	
				
				int w = Console.WindowWidth;
				int h = mapHeight;
			
				int diffx = (int)Math.Ceiling((double)(w/2)) - player.X;
				int diffy = (int)Math.Ceiling((double)(h/2)) - player.Y;
				
				Console.CursorLeft = player.X + diffx;
				Console.CursorTop = player.Y + diffy;
				Console.Write(player.Symbol());
				
				Console.CursorTop = h+1;
				Console.CursorLeft = 0;
				Console.WriteLine("Movement - arrows");
				Console.WriteLine("End - press Escape");
				Console.WriteLine("Inventory - press I");
				Console.WriteLine("Interact with objects - press Enter");
				Console.WriteLine("Save current player - press S");
				Console.WriteLine();
				WriteMessages();
				
				ConsoleKeyInfo c = Console.ReadKey();
				if (c.Key == ConsoleKey.Escape)
					end = true;
				else if (c.Key == ConsoleKey.I)
					player.ManageInventory();
				else if (c.Key == ConsoleKey.Enter)
					dungeon.location[player.X,player.Y].VoluntaryAction(player);
				else if (c.Key == ConsoleKey.S)
				{
					SaveGame(player.currentDungeon);
				}					
					
				player.Move(c, dungeon);
				
				if (!player.Alive())
				{
					Console.WriteLine("You're dead! press any key");
					end = true;
					Console.ReadKey();
				}
					
			}
			
		}
		
		public static void LoadNextMap(string dungeonname)
		{
			InitializeMap(false, dungeonname);
			player.X = dungeon.PlayerX;
			player.Y = dungeon.PlayerY;
		}
		
		public static void SaveGame(string dungeonname)
		{
			player.SaveAsXml();
			dungeon.PlayerX = player.X;
			dungeon.PlayerY = player.Y;
			dungeon.ToXml("players/" + player.Name.ToLower() + "/" + dungeonname);
			ThisGame.SaveConfiguration();
		}
		
		public static void SaveConfiguration()
		{
			LuaTable config = lua.GetTable("config");
			
			string lines = "config = {}\n";
			Dictionary<object, object> tb = lua.GetTableDict(config);
		
			foreach(KeyValuePair<object, object> kv in tb)
			{
				lines += string.Format("config.{0} = {1}\n", kv.Key, kv.Value.ToString().ToLower());
			}
			config.Dispose();
			
			System.IO.StreamWriter file = new System.IO.StreamWriter(filePath + "players/" + player.Name.ToLower() + "/config.lua");
			file.WriteLine(lines);
			
			file.Close();
		}
		
		public static void SetVisibility(int vis)
		{
			Visibility = vis;
		}
		
		/*
		public static void SetCurrentDungeon(string newDungeon)
		{
			currentDungeon = newDungeon;
		}
		*/
		
		public static void WriteMessages()
		{
			foreach (string msg in messageLog)
				Console.WriteLine(msg);
		}
		
		public static void InitializeMap(bool newgame, string dungeonName)
		{			
			string mapname = "players/" + player.Name.ToLower() + "/" + dungeonName;
			
			try
			{
				if (newgame)
				{
					dungeon = LoadMapFromXml("dungeons/" + dungeonName);
					//SetCurrentDungeon("dungeon1");
				}				
				else
					dungeon = LoadMapFromXml(mapname);
			}
			catch
			{
				//Console.WriteLine("Couldn't load map.");
				dungeon = LoadMapFromXml("dungeons/" + dungeonName);
			}
			
		}
		
		public static void InitializePlayer(out bool newgame)
		{
			string startupPath = filePath;
			newgame = true;
			
			List<string> players = new List<string>();
			
			// initialize empty player, which will be overriden
			Player p = new Player("name", new Characteristics(0,0,0,0), new Characteristics(0,0,0,0), 10, 0, 0);
			// list of all players ctored in a database
			DirectoryInfo dir = new DirectoryInfo(startupPath + "players/");
			foreach (DirectoryInfo dirinfo in dir.GetDirectories())
			{
				string dirname = dirinfo.Name.ToUpperFirstLetter();
				players.Add (dirname);
			}
			
			// ask player, if he want to chose from existing players, or create a new one
			if (players.Count == 0)
			{
				player = CreateNewPlayer();
				return;
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
					p = CreateNewPlayer();
					ask = false;
					break;
				}
				case 'n':
				{
					p = PickCharacter(players);
					newgame = false;
					ask = false;
					break;
				}
				default:
				{
					break;
				}
				}				
				
			}
			player = p;
		}
		
		public static Player CreateNewPlayer()
		{			
			// Pick name
			Console.WriteLine("What is your name?");
			string name = Console.ReadLine().Trim();
			// lets egyptize the name
			int choice = 0;
			string userInput = "";
			string[] names = {(name.ToLower() + "nefer").ToUpperFirstLetter(), (name.ToLower() + "hotep").ToUpperFirstLetter(), "Ptah" + name.ToLower() + "tep", "Nefe" + name.ToLower() + "bet", 
				"Ankh" + name.ToLower() + "amun", "Iset" + name.ToLower() + "rure", "Neb" + name.ToLower() + "kare"};
			while (choice < 1 || choice > 7)
			{
				Console.WriteLine("That sounds just bad. What about:");
				for (int i=0; i<names.Length; i++)
					Console.WriteLine("\t{0}: {1}", i+1, names[i]);
				userInput = Console.ReadLine();
                int.TryParse(userInput, out choice );
			}
			// create new name
			string newname = names[choice-1];
						
			Characteristics ch = new Characteristics(10,2,1,0);
			Characteristics ch2 = new Characteristics(10,2,1,0);
			Player p = new Player(newname, ch, ch2, 100, 2, 2);
			
			p.SetCurrentDungeon("dungeon1");
			
			Item amulet = new Item("Amulet with crocodile");
			p.PickItem(amulet);
			
			Console.Clear();
			Console.WriteLine("You wake up.");
			Console.WriteLine("Your head is spinning and you feel throbbing pain on the back of your head.");
			Console.WriteLine("There is pitch dark all around you, not a single ray of light. And who are you, anyway?");
			Console.ReadKey();
			Console.WriteLine("Oh yes, your name is {0} and you were building the tomb for Khasekhemre, pharaohs chief accountant.", newname);
			Console.WriteLine("And thats the last thing you remember");
			Console.ReadKey();
			Console.WriteLine("You should probably find out what happened.");
			Console.ReadKey();
			return p;
		}
		
		public static Player PickCharacter(List<string> players)
		{
			// create empty player, which will be overriden
			Player p = new Player("name", new Characteristics(0,0,0,0), new Characteristics(0,0,0,0), 10, 0, 0);
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
					p = LoadPlayerFromXml(players[nint]);
					chosen = true;
				}
				catch
				{
					Console.WriteLine("Your input was incorrect");
				}
			}
			return p;
		}
		
		public static Player LoadPlayerFromXml(string playername)
		{
			string path = filePath + "players/" + playername.ToLower() + "/player.xml";
			
			XmlDocument doc = new XmlDocument();
			doc.Load(path);
			
			XmlNode root = doc.DocumentElement;
						
			// get attributes of player - name and positions
			string name = root.Attributes["name"].Value;
			int x = int.Parse(root.Attributes["x"].Value);
			int y = int.Parse(root.Attributes["y"].Value);
			string dungeonName = root.Attributes["currentDungeon"].Value;
			
			Characteristics ch = new Characteristics(0,0,0,0);
			Characteristics cch = new Characteristics(0,0,0,0);
			Inventory bag = new Inventory(0);
			Inventory equiped = new Inventory(0);
			string[] b = new string[6];
			
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
			Player p = new Player(name, ch, cch, bag.maxsize, x, y);
			p.bag = bag;
			p.equiped = equiped;
			p.SetBody(new Body(b));
			p.SetCurrentDungeon(dungeonName);
			
			lua.DoFile(filePath + "players/" + p.Name.ToLower() + "/config.lua");
			
			return p;
		}
		
		public static Characteristics LoadCharacteristicsFromXml(XmlElement node)
		{
			int hp = int.Parse (node.GetAttribute("hitpoints"));
			int attack = int.Parse (node.GetAttribute("attack"));
			int defence = int.Parse (node.GetAttribute("defence"));
			int speed = int.Parse (node.GetAttribute("speed"));
			Characteristics ch = new Characteristics(hp, attack, defence, speed);
			return ch;
		}
		
		public static string[] LoadBodyFromXML(XmlElement node)
		{
			string[] lst = new String[6];
			if (bool.Parse (node.GetAttribute("body")))
				lst[0] = "body";
			if (bool.Parse (node.GetAttribute("head")))
				lst[1] = "head";
			if (bool.Parse (node.GetAttribute("legs")))
				lst[2] = "legs";
			if (bool.Parse (node.GetAttribute("boots")))
				lst[3] = "boots";
			if (bool.Parse (node.GetAttribute("weapon")))
				lst[4] = "weapon";
			if (bool.Parse (node.GetAttribute("shield")))
				lst[5] = "shield";
			return lst;
		}
		
		public static Item LoadItemFromXml(XmlElement node)
		{
			string name = node.GetAttribute("name");
			Item i = new Item(name);
			string script = node.GetAttribute("script");
			if(script != "")
				i.SetScript(script);
			try
			{
				string symbol = node.GetAttribute("symbol");
				i.SetSymbol(symbol);
			}
			catch
			{}
			return i;
		}
		
		public static Equipment LoadEquipmentFromXml(XmlElement node)
		{
			string name = node.GetAttribute("name");
			Characteristics ch = LoadCharacteristicsFromXml((XmlElement)node.GetElementsByTagName("Characteristics")[0]);
			string[] b = LoadBodyFromXML((XmlElement)node.GetElementsByTagName("Body")[0]);
			Equipment e = new Equipment(name, ch, b);
			string script = node.GetAttribute("script");
			if(script != "")
				e.SetScript(script);
			try
			{
				string symbol = node.GetAttribute("symbol");
				e.SetSymbol(symbol);
			}
			catch
			{}
			return e;
		}
		
		public static Weapon LoadWeaponFromXml(XmlElement node)
		{
			string name = node.GetAttribute("name");
			int nrFacets = int.Parse(node.GetAttribute("nrfacets"));
			Characteristics ch = LoadCharacteristicsFromXml((XmlElement)node.GetElementsByTagName("Characteristics")[0]);
			string[] b = LoadBodyFromXML((XmlElement)node.GetElementsByTagName("Body")[0]);
			Weapon w = new Weapon(name, ch, b, nrFacets);
			string script = node.GetAttribute("script");
			if(script != "")
				w.SetScript(script);
			try
			{
				string symbol = node.GetAttribute("symbol");
				w.SetSymbol(symbol);
			}
			catch
			{}
			return w;
		}
		
		public static Inventory LoadInventoryFromXml(XmlElement node)
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
		
		public static Map LoadMapFromXml(string mapname)
		{
			string path = filePath + mapname.ToLower() + ".xml";
			
			XmlDocument doc = new XmlDocument();
			doc.Load(path);
			
			XmlNode root = doc.DocumentElement;
						
			// get attributes of map - width and heigth
			int x = int.Parse(root.Attributes["width"].Value);
			int y = int.Parse(root.Attributes["heigth"].Value);
			int playerX = int.Parse(root.Attributes["playerX"].Value);
			int playerY = int.Parse(root.Attributes["playerY"].Value);
			
			Map newmap = new Map();
			newmap.CreateMapField(x,y);
			newmap.PlayerX = playerX;
			newmap.PlayerY = playerY;
			
			foreach (XmlNode node in root.ChildNodes)
			{
				Location l = LoadLocationFromXml((XmlElement)node);
				newmap.AddLocation(l);
				l.UpdateSymbol();
			}
			
			return newmap;
		}
		
		public static Location LoadLocationFromXml(XmlElement node)
		{
			int x = int.Parse(node.Attributes["x"].Value);
			int y = int.Parse(node.Attributes["y"].Value);
			bool visible = bool.Parse(node.Attributes["visible"].Value);
			string script;
			try
			{
				script = node.Attributes["script"].Value;
			}
			catch 
			{
				script = null;
			}
			IPlace block = LoadBlockFromXml((XmlElement)node.GetElementsByTagName("block")[0]);
			Location l = new Location(x, y, block);
			l.Script = script;
			l.Visible = visible;
			return l;
		}
		
		public static IPlace LoadBlockFromXml(XmlElement node)
		{
			string type = node.Attributes["type"].Value;
			
			switch (type)
			{
			case "Game.BasicObject":
			{
				return LoadBasicObjectFromXml((XmlElement)node.GetElementsByTagName("BasicObject")[0]);
			}
			case "Game.Impassable":
			{
				return LoadImpassableFromXml((XmlElement)node.GetElementsByTagName("Impassable")[0]);
			}
			case "Game.Door":
			{
				return LoadDoorFromXml((XmlElement)node.GetElementsByTagName("Door")[0]);
			}
			case "Game.Item":
			{
				return LoadItemFromXml((XmlElement)node.GetElementsByTagName("Item")[0]);	
			}
			case "Game.Chest":
			{
				return LoadChestFromXml((XmlElement)node.GetElementsByTagName("Chest")[0]);
			}
			case "Game.Being":
			{
				return LoadBeingFromXml((XmlElement)node.GetElementsByTagName("Being")[0]);
			}
			default:
			{
				return new BasicObject();
			}
			}
		}
		
		public static Door LoadDoorFromXml(XmlElement node)
		{
			bool locked = bool.Parse(node.Attributes["locked"].Value);
			string symbol = node.Attributes["symbol"].Value;
			string msg = node.GetElementsByTagName("Message")[0].InnerText;
			Door door = new Door(msg, locked);
			door.SetSymbol(symbol);
			return door;
		}
		
		public static BasicObject LoadBasicObjectFromXml(XmlElement node)
		{
			string symbol = node.Attributes["symbol"].Value;
			BasicObject bo = new BasicObject();
			bo.SetSymbol(symbol);
			return bo;
		}
		
		public static Impassable LoadImpassableFromXml(XmlElement node)
		{
			string symbol = node.Attributes["symbol"].Value;
			string name = node.Attributes["name"].Value;
			Impassable imp = new Impassable(name);
			imp.SetSymbol(symbol);
			return imp;
		}
			
		public static Chest LoadChestFromXml(XmlElement node)
		{
			string name = node.Attributes["name"].Value;
			bool removeIfEmpty = bool.Parse(node.Attributes["removeIfEmpty"].Value);
			string symbol = node.Attributes["symbol"].Value;
			XmlElement child = (XmlElement)node.GetElementsByTagName("Inventory")[0];
			Inventory inv = LoadInventoryFromXml(child);
			Chest chest = new Chest(name, inv);
			chest.SetSymbol(symbol);
			chest.SetRemoving(removeIfEmpty);
			return chest;
		}
		
		public static Being LoadBeingFromXml(XmlElement node)
		{
			// get name of being
			string name = node.Attributes["name"].Value;
			string symbol = node.Attributes["symbol"].Value;
			
			Characteristics ch = new Characteristics(0,0,0,0);
			Characteristics cch = new Characteristics(0,0,0,0);
			Inventory bag = new Inventory(0);
			Inventory equiped = new Inventory(0);
			string[] b = new string[6];
			
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
					bag = LoadInventoryFromXml((XmlElement)subnode);
					break;
				}
				case "Equiped":
				{
					equiped = LoadInventoryFromXml((XmlElement)subnode);
					break;
				}
				case "Body":
				{
					b = LoadBodyFromXML((XmlElement)subnode);
					break;
				}
					
				}
				
			}
			Being being = new Being(name, ch, cch, bag.maxsize);
			being.bag = bag;
			being.equiped = equiped;
			being.SetBody(new Body(b));
			being.SetSymbol(symbol);
			
			return being;
		}
	}
}