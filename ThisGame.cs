using System;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Threading;
using NLua;

namespace Game
{
	public static class ThisGame
	{
		
		public static LimitedQueue<string> messageLog = new LimitedQueue<string>(6);
		
		public static Map dungeon;
		
		public static int mapHeight = 10;
		
		public static string filePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName,"files/");
		
		public static Lua lua = new Lua();
		
		public static int Visibility = 0;
	
		public static void RunGame()
		{
			// first load lua scripts
			lua.DoFile(filePath + "luascripts/startup.lua");
			lua.DoFile(filePath + "luascripts/config.lua");
			
			// initialize player, which includes loading lua file, if player is loaded
			Player p = InitializePlayer();
			lua["player"] = p;
			
			//initialize map for player
			dungeon = InitializeMap(p);		
			
			// main loop - running the program
			bool end = false;
			
			while(!end)
			{
				Console.Clear();
				
				// set visibility of the player
				double vis = (double)lua["visibility"];
				SetVisibility((int)vis);
				
				if (ThisGame.Visibility > 0)
					dungeon.CalculateVisibility(p, Visibility);
				dungeon.Draw(p);	
				
				int w = Console.WindowWidth;
				int h = mapHeight;
			
				int diffx = (int)Math.Ceiling((double)(w/2)) - p.X;
				int diffy = (int)Math.Ceiling((double)(h/2)) - p.Y;
				
				Console.CursorLeft = p.X + diffx;
				Console.CursorTop = p.Y + diffy;
				Console.Write(p.Symbol());
				
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
					p.ManageInventory();
				else if (c.Key == ConsoleKey.Enter)
					dungeon.location[p.X,p.Y].VoluntaryAction(p);
				else if (c.Key == ConsoleKey.S)
				{
					p.SaveAsXml();
					dungeon.ToXml(p.Name.ToLower());
					ThisGame.SaveConfiguration(p);
					continue;
				}					
					
				p.Move(c, dungeon);
				
				if (!p.Alive())
				{
					Console.WriteLine("Your dead! press any key");
					end = true;
					Console.ReadKey();
				}
					
			}
			
		}
		
		public static void SaveConfiguration(Player p)
		{
			double vis = (double)lua["visibility"];
			bool torch = (bool)lua["torch"];
			string lines = String.Format("visibility = {0}\ntorch={1}", vis.ToString(), torch.ToString().ToLower());

			System.IO.StreamWriter file = new System.IO.StreamWriter(filePath + p.Name.ToLower() + ".lua");
			file.WriteLine(lines);
			
			file.Close();
		}
		
		public static void SetVisibility(int vis)
		{
			Visibility = vis;
		}
		
		public static void WriteMessages()
		{
			foreach (string msg in messageLog)
				Console.WriteLine(msg);
		}
		
		public static Map InitializeMap(Player p)
		{			
			string mapname = p.Name + "_map";
			
			try
			{
				return LoadMapFromXml(mapname);
			}
			catch
			{
				return LoadMapFromXml("dungeon1_map");
			}
		}
		
		public static Player InitializePlayer()
		{
			string startupPath = filePath;
			
			List<string> players = new List<string>();
			
			// initialize empty player, which will be overriden
			Player p = new Player("name", new Characteristics(0,0,0,0), new Characteristics(0,0,0,0), 10, 0, 0);
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
				p = CreateNewPlayer();
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
					p = CreateNewPlayer();
					ask = false;
					break;
				}
				case 'n':
				{
					p = PickCharacter(players);
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
		
		public static Player CreateNewPlayer()
		{
			// Pick name
			Console.WriteLine("What is the name of your hero?");
			string name = Console.ReadLine().Trim();
			// lets egyptize the name
			int choice = 0;
			string userInput = "";
			while (choice < 1 || choice > 7)
			{
				Console.WriteLine("That sounds just bad. What about:");
				Console.WriteLine("\t1: {0}", name + "nefer");
				Console.WriteLine("\t2: {0}", name + "hotep");
				Console.WriteLine("\t3: {0}", "Ptah" + name.ToLower() + "tep");
				Console.WriteLine("\t4: {0}", "Nefe" + name.ToLower() + "bet");
				Console.WriteLine("\t5: {0}", "Ankh" + name.ToLower() + "amun");
				Console.WriteLine("\t6: {0}", "Iset" + name.ToLower() + "rure");
				Console.WriteLine("\t7: {0}", "Neb" + name.ToLower() + "kare");
				userInput = Console.ReadLine();
                int.TryParse(userInput, out choice );
			}
			// create new name
			string newname = "";
			switch (choice)
			{
			case 1:
				newname = name + "nefer";
				break;
			case 2:
				newname = name + "hotep";
				break;
			case 3:
				newname = "Ptah" + name.ToLower() + "tep";
				break;
			case 4:
				newname = "Nefe" + name.ToLower() + "bet";
				break;
			case 5:
				newname = "Ankh" + name.ToLower() + "amun";
				break;
			case 6:
				newname = "Iset" + name.ToLower() + "rure";
				break;
			case 7:
				newname = "Neb" + name.ToLower() + "kare";
				break;
			}
			
			Characteristics ch = new Characteristics(10,2,1,0);
			Player p = new Player(newname, ch, ch, 100, 2, 2);
			
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
			string path = filePath + playername.ToLower() + "_plr.xml";
			
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
			
			lua.DoFile(filePath + p.Name.ToLower() + ".lua");
			
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
		
		public static List<string> LoadBodyFromXML(XmlElement node)
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
		
		public static Item LoadItemFromXml(XmlElement node)
		{
			string name = node.GetAttribute("name");
			Item i = new Item(name);
			string script = node.GetAttribute("script");
			if(script != "")
				i.SetScript(script);
			return i;
		}
		
		public static Equipment LoadEquipmentFromXml(XmlElement node)
		{
			string name = node.GetAttribute("name");
			Characteristics ch = LoadCharacteristicsFromXml((XmlElement)node.GetElementsByTagName("Characteristics")[0]);
			List<string> b = LoadBodyFromXML((XmlElement)node.GetElementsByTagName("Body")[0]);
			Equipment e = new Equipment(name, ch, b);
			string script = node.GetAttribute("script");
			if(script != "")
				e.SetScript(script);
			return e;
		}
		
		public static Weapon LoadWeaponFromXml(XmlElement node)
		{
			string name = node.GetAttribute("name");
			int nrFacets = int.Parse(node.GetAttribute("nrfacets"));
			Characteristics ch = LoadCharacteristicsFromXml((XmlElement)node.GetElementsByTagName("Characteristics")[0]);
			List<string> b = LoadBodyFromXML((XmlElement)node.GetElementsByTagName("Body")[0]);
			Weapon w = new Weapon(name, ch, b, nrFacets);
			string script = node.GetAttribute("script");
			if(script != "")
				w.SetScript(script);
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
			
			Map newmap = new Map();
			newmap.CreateMapField(x,y);
			
			foreach (XmlNode node in root.ChildNodes)
			{
				Location l = LoadLocationFromXml((XmlElement)node);
				newmap.AddLocation(l);
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
				return LoadChestFromXml((XmlElement)node.GetElementsByTagName("Chest")[0]);
			}
			case "Game.Corpse":
			{
				return LoadCorpseFromXml((XmlElement)node.GetElementsByTagName("Corpse")[0]);
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
			string msg = node.GetElementsByTagName("Message")[0].InnerText;
			string keyname = node.GetElementsByTagName("Keyname")[0].InnerText;
			return new Door(msg, keyname, locked);
		}
		
		public static Chest LoadChestFromXml(XmlElement node)
		{
			string name = node.Attributes["name"].Value;
			XmlElement child = (XmlElement)node.GetElementsByTagName("Inventory")[0];
			Inventory inv = LoadInventoryFromXml(child);
			return new Chest(name, inv);
		}
		
		public static Corpse LoadCorpseFromXml(XmlElement node)
		{
			string name = node.Attributes["name"].Value;
			XmlElement child = (XmlElement)node.GetElementsByTagName("Inventory")[0];
			Inventory inv = LoadInventoryFromXml(child);
			return new Corpse(name, inv);
		}
		
		public static Being LoadBeingFromXml(XmlElement node)
		{
			// get name of being
			string name = node.Attributes["name"].Value;
			
			Characteristics ch = new Characteristics(0,0,0,0);
			Characteristics cch = new Characteristics(0,0,0,0);
			Inventory bag = new Inventory(0);
			Inventory equiped = new Inventory(0);
			List<string> b = new List<string>();
			
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
			
			return being;
		}
	}
}