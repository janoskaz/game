using System;

namespace Game
{
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
			if (locked)
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
		public void OpenDoor(out string msg2)
		{
			msg2 = "\nDoor has been opened";
			this.Locked = false;
		}
		
		public override char Symbol()
		{
			return this.MapSymbol;
		}
		
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
		
		public override bool PerformAction(Player p, Location l, out string msg, out Location l2)
		{
			msg = this.Description;
			l2 = l;
			if (!this.Locked)
			{
				return true;
			}
			string keyname = this.Keyname;
			bool hasKey = p.HasItem(keyname);
			string msg2 = "";
			if (hasKey)
			{
				this.OpenDoor(out msg2);
				this.UpdateSymbol();
				msg += msg2;
				return true;
			}
			msg += "\nThe doors are locked and you don't have the key.";
			return false;
		}
		
	}
}

