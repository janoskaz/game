using System;
using System.Xml;

namespace Game
{
	/// <summary>
	/// Door - has some message, description, symbol and name ok key, which opens it.
	/// </summary>
	public class Door: BasicObject
	{
		public string Description {get; private set;}
		
		private readonly string Keyname;
		
		public bool Locked {get; private set;}
		
		public char MapSymbol;
		
		public Door (string description, string key, bool locked=false)
		{
			Description = description;
			this.Keyname = key;
			Locked = locked;
			if (locked) // set symbol based on state of doors (locked/open)
			{
				this.MapSymbol = '/';
			}
			else
			{
				this.MapSymbol = '-';
			}
		}
		
		/// <summary>
		/// Opens the door with given key.
		/// </summary>
		/// <returns>
		/// The door.
		/// </returns>
		/// <param name='key'>
		/// If set to <c>true</c> key.
		/// </param>
		public void OpenDoor()
		{
			ThisGame.messageLog.Enqueue("Door has been opened");
			this.Locked = false;
		}
		
		public override char Symbol()
		{
			return this.MapSymbol;
		}
		
		public override bool CanMoveTo()
		{
			return !Locked;
		}
		
		/// <summary>
		/// Updates the symbol, depending on state.
		/// </summary>
		private void UpdateSymbol()
		{
			if (this.Locked)
			{
				this.MapSymbol = '/';
			}
			else
			{
				this.MapSymbol = '-';
			}
		}
		
		/// <summary>
		/// write the xml.
		/// </summary>
		/// <returns>
		/// The xml.
		/// </returns>
		/// <param name='doc'>
		/// Document.
		/// </param>
		/// <param name='slementName'>
		/// Slement name.
		/// </param>
		public override XmlElement ToXml (XmlDocument doc, string slementName)
		{
			XmlElement door = doc.CreateElement("Door");
			// append description
			XmlElement msg = doc.CreateElement("Message");
			msg.InnerXml = Description;
			door.AppendChild(msg);
			// append keyname
			XmlElement keyname = doc.CreateElement("Keyname");
			keyname.InnerXml = Keyname;
			door.AppendChild(keyname);
			// append locked
			door.SetAttribute("locked", Locked.ToString());
				
			return door;
		}
		
		/// <summary>
		/// Performs the action. If user has coorect key, opens the door, else - tells him he needs a key.
		/// </summary>
		/// <returns>
		/// The action.
		/// </returns>
		/// <param name='p'>
		/// If set to <c>true</c> p.
		/// </param>
		/// <param name='l'>
		/// If set to <c>true</c> l.
		/// </param>
		/// <param name='msg'>
		/// If set to <c>true</c> message.
		/// </param>
		/// <param name='l2'>
		/// If set to <c>true</c> l2.
		/// </param>
		public override IPlace AutomaticAction(Player p)
		{
			ThisGame.messageLog.Enqueue(this.Description);
			// does the user have a key to open the door?
			string keyname = this.Keyname;
			bool hasKey = p.HasKey(keyname);
			// open, if user has a key
			if (Locked)
			{
				if (hasKey)
				{
					this.OpenDoor();
					this.UpdateSymbol();
				}
				else
					ThisGame.messageLog.Enqueue("The doors is locked and you don't have the key.");
			}
			return this;
		}
		
	}
}

