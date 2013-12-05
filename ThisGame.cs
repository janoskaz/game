using System;
using System.IO;
using System.Collections.Generic;

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
			
			Player p = LoadPlayer("odin", dice6);
			
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
		
	}
}