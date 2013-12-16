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
		
		public Player (string name, Characteristics ch, Characteristics currentCh, int bagsize, int x, int y) :base(name, ch, currentCh, bagsize)
		{
			X = x;
			Y = y;
		}
		
		public void Move(ConsoleKeyInfo c, Map m)
		{
			bool move = false;
			
			if ((c.Key == ConsoleKey.UpArrow) && (Y>0))
			{
				move = m.location[X,Y-1].CanMoveTo();
				m.location[X,Y-1].AutomaticAction(this);
				if (move)
					Y--;
			}
			else if ((c.Key == ConsoleKey.DownArrow) && (Y<(m.Heigth-1)))
			{
				move = m.location[X,Y+1].CanMoveTo();
				m.location[X,Y+1].AutomaticAction(this);
				if (move)
					Y++;
			}
			else if ((c.Key == ConsoleKey.LeftArrow) && (X>0))
			{
				move = m.location[X-1,Y].CanMoveTo();
				m.location[X-1,Y].AutomaticAction(this);
				if (move)
					X--;
			}
			else if ((c.Key == ConsoleKey.RightArrow) && (X<(m.Width-1)))
			{
				move = m.location[X+1,Y].CanMoveTo();
				m.location[X+1,Y].AutomaticAction(this);
				if (move)
					X++;
			}
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
					Thread.Sleep(600);
				}
				b1 = this;
				b2 = creature;
				Thread.Sleep(600);
			}
			return this.Alive();
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
			ThisGame.messageLog.Enqueue("Attepmt to save the player.");
			string path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName,"files/");
			
			XmlDocument doc = new XmlDocument();
			
			XmlDeclaration header = doc.CreateXmlDeclaration("1.0", "utf-8", null);
			doc.AppendChild(header);
			XmlElement root = this.ToXml(doc, "Player");
			
			doc.AppendChild(root);
			doc.Save( Path.Combine(path, this.Name.ToLower()+"_plr.xml") );
			ThisGame.messageLog.Enqueue("Save");
		}
		
	}
}

