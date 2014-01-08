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
		
		public bool Locked {get; private set;}
		
		public Door (string description, bool locked=true)
		{
			Description = description;
			Locked = locked;
			if (locked) // set symbol based on state of doors (locked/open)
			{
				this.symbol = '/';
			}
			else
			{
				this.symbol = '-';
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
			this.UpdateSymbol();
		}
		
		public override bool CanMoveTo()
		{
			return !Locked;
		}
		
		public override bool CanDropItemOnto()
		{
			return false;
		}
		
		/// <summary>
		/// Updates the symbol, depending on state.
		/// </summary>
		private void UpdateSymbol()
		{
			if (this.Locked)
			{
				this.symbol = '/';
			}
			else
			{
				this.symbol = '-';
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
			// append locked
			door.SetAttribute("locked", Locked.ToString());
			door.SetAttribute("symbol", symbol.ToString());
				
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
			// this wont happen, every door has a key
			return this;
		}
		
	}
}

