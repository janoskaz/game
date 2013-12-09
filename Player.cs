using System;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Xml;

namespace Game
{
	public class Player: Being
	{
		public int X {get; set;}
		public int Y {get; set;}
		public string Message {get; set;}
		
		public Player (string name, Characteristics ch, Characteristics currentCh, int bagsize, Dice dice, int x, int y) :base(name, ch, currentCh, bagsize, dice)
		{
			X = x;
			Y = y;
		}
		
		public void Move(ConsoleKeyInfo c, Map m, out string message)
		{
			bool move = false;
			message = "";
			Location l2 = m.location[X,Y];
			if ((c.Key == ConsoleKey.UpArrow) && (Y>0))
			{
				move = m.location[X,Y-1].Block.PerformAction(this, m.location[X,Y-1], out message, out l2);
				if (move)
					Y--;
				else
					l2 = m.location[X,Y];
			}
			else if ((c.Key == ConsoleKey.DownArrow) && (Y<(m.Heigth-1)))
			{
				move = m.location[X,Y+1].Block.PerformAction(this, m.location[X,Y+1], out message, out l2);
				if (move)
					Y++;
				else
					l2 = m.location[X,Y];
			}
			else if ((c.Key == ConsoleKey.LeftArrow) && (X>0))
			{
				move = m.location[X-1,Y].Block.PerformAction(this, m.location[X-1,Y], out message, out l2);
				if (move)
					X--;
				else
					l2 = m.location[X,Y];
			}
			else if ((c.Key == ConsoleKey.RightArrow) && (X<(m.Width-1)))
			{
				move = m.location[X+1,Y].Block.PerformAction(this, m.location[X+1,Y], out message, out l2);
				if (move)
					X++;
				else
					l2 = m.location[X,Y];
			}
			else
			{
				message = "Nothing happened.";
			}
			m.location[X,Y] = l2;
		}
		
		public int WhoAttacks(Being creature)
		{
			int init1 = this.CalculateInitiative();
			int init2 = creature.CalculateInitiative();
			
			if (init1 > init2)
				return 1;
			if (init2>init1)
				return 2;
			return 0;					
		}
		
		public bool Fight(Being creature)
		{
			Being b1 = this;
			Being b2 = creature;
			
			while(b1.Alive() && b2.Alive())
			{
				// decide, who will attack first
				int attacker = this.WhoAttacks(creature);
				if (attacker==2)
				{
					b2 = this;
					b1 = creature;
				}
				// perform attack
				b1.Attack(b2);
				if(b2.Alive() || attacker==0) // if enemy 2 is alive, of if the attacks are parallel
				{
					b2.Attack(b1);
				}
				b1 = this;
				b2 = creature;
				Thread.Sleep(500);
			}
			return this.Alive();
		}
		
		public void Save(out string message)
		{
			string startupPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName,"files/"+this.Name.ToLower()+".plr");
			
			using (StreamWriter sw = new StreamWriter(startupPath))
			{
				// write first line with name and coordinates
				string[] plr = {"Player" ,Name, X.ToString(), Y.ToString()};
				string plrJoin = String.Join(";", plr);
				sw.WriteLine(plrJoin);
				
				// second line with characteristics
				string[] characteristics = {"Characteristics" ,this.Characteristics.hitpoints.ToString(), 
					this.Characteristics.attack.ToString(), this.Characteristics.defence.ToString(), this.Characteristics.speed.ToString()};
				string charJoin = String.Join(";", characteristics);
				sw.WriteLine(charJoin);
				
				// third line with current characteristics
				string[] currentCharacteristics = {"CurrentCharacteristics" ,(this.CurrentCharacteristics.hitpoints - this.Characteristics.hitpoints).ToString(), 
					(this.CurrentCharacteristics.attack - this.Characteristics.attack).ToString(),
					(this.CurrentCharacteristics.defence - this.Characteristics.defence).ToString(),
					(this.CurrentCharacteristics.speed - this.Characteristics.speed).ToString()};
				string curCharJoin = String.Join(";", currentCharacteristics);
				sw.WriteLine(curCharJoin);
				
				// fourth line with the size of the bag
				sw.WriteLine("Bagsize;"+this.bag.maxsize);
				// add all items in the bag
				foreach (Item i in this.bag.bag)
				{
					switch(i.GetType().ToString())
					{
					case "Game.Item": // it item, write item
					{
						sw.WriteLine("Bag;Game.Item;"+i.Name);
						break;
					}
					default: // if equipment or weapon, saving object is the same
					{
						string output = "Bag;" + i.GetType().ToString() + ";" + i.Name; // bag + type of item
						foreach (var pair in ((Equipment)i).Body) // all body parts as binary numbers
							output += ";" + pair.Value.ToString();						
						output += ";"+((Equipment)i).Characteristics.hitpoints.ToString(); // add characteristics
						output += ";"+((Equipment)i).Characteristics.attack.ToString();
						output += ";"+((Equipment)i).Characteristics.defence.ToString();
						output += ";"+((Equipment)i).Characteristics.speed.ToString();
						sw.WriteLine(output);
						break;
					}
					}
				}
				// add all items currently equiped
				foreach (Item i in this.equiped.bag)
				{
					string output = "Equipment;" + i.GetType().ToString() + ";" + i.Name; // bag + type of item
					foreach (var part in ((Equipment)i).Body) // all body parts as binary numbers
						output += ";" + part.Value.ToString();						
					output += ";"+((Equipment)i).Characteristics.hitpoints.ToString(); // add characteristics
					output += ";"+((Equipment)i).Characteristics.attack.ToString();
					output += ";"+((Equipment)i).Characteristics.defence.ToString();
					output += ";"+((Equipment)i).Characteristics.speed.ToString();
					sw.WriteLine(output);
				}
				
			}
			message = "Saved";
		}
		
		new public char Symbol()
		{
			return 'P';
		}
		
		public override XmlElement ToXml(XmlDocument doc, string elementName)
		{
			XmlElement plr = doc.CreateElement(elementName);
			plr.SetAttribute("name", Name); // set name attribute
			plr.SetAttribute("x", this.X.ToString()); // set coordinates
			plr.SetAttribute("y", this.Y.ToString()); // set coordinates
			// add characteristics
			XmlElement ch = this.Characteristics.ToXml(doc, "Characteristics");
			plr.AppendChild(ch);
			// add Current Characteristics
			XmlElement cch = this.CurrentCharacteristics.ToXml(doc, "CurrentCharacteristics");
			plr.AppendChild(cch);
			// add bag
			XmlElement bag = this.bag.ToXml(doc, "Bag");
			plr.AppendChild(bag);
			// add equiped items
			XmlElement equiped = this.equiped.ToXml(doc, "Equiped");
			plr.AppendChild(equiped);
			// add body
			XmlElement body = this.Body.ToXml(doc, "Body");
			plr.AppendChild(body);
			
			return plr;
		}
		
		public void SaveAsXml()
		{
			string path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName,"files/");
			
			XmlDocument doc = new XmlDocument();
			
			XmlDeclaration header = doc.CreateXmlDeclaration("1.0", "utf-8", null);
			doc.AppendChild(header);
			XmlElement root = this.ToXml(doc, "Player");
			
			doc.AppendChild(root);
			doc.Save( Path.Combine(path, this.Name+".xml") );
		}
		
	}
}

