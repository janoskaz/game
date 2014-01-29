using System;
using System.Collections;
using System.Collections.Generic;

namespace ExtensionMethods
{
    public static class MyExtensions
    {
        
		public static string ToUpperFirstLetter(this string source)
		{
		    if (string.IsNullOrEmpty(source))
		        return string.Empty;
		    // convert to char array of the string
		    char[] letters = source.ToCharArray();
		    // upper case the first char
		    letters[0] = char.ToUpper(letters[0]);
		    // return the array made of the new char array
		    return new string(letters);
		}
		
		public static string ListThroughCommands(this List<string> cmds)
		{
			// define variables to help
			string s = "";
			int whichFromEnd = 0;
			int whichToDisplay;
			bool run = true;
			int stringPosition = 0;
			
			while (run)
			{
				// get user input
				ConsoleKeyInfo keyhit = Console.ReadKey();
				switch (keyhit.Key)
				{
				case ConsoleKey.Escape: // return "close", which will end program
					s = "close";
					run = false;
					break;					
				case ConsoleKey.Backspace: // backspace character
					if (s.Length > 0 & stringPosition>0)
					{
						s = s.Remove(stringPosition-1,1);
						stringPosition--;
						char[] charlist = s.ToCharArray();
						for (int i=stringPosition; i<charlist.Length; i++)
							Console.Write(charlist[i]);
						Console.Write(" ");
						Console.CursorLeft = stringPosition;
					}
					break;
				case ConsoleKey.Delete: // delete character
					if (s.Length>stringPosition)
					{
						s = s.Remove(stringPosition,1);
						char[] charlist = s.ToCharArray();
						for (int i=stringPosition; i<charlist.Length; i++)
							Console.Write(charlist[i]);
						Console.Write(" ");
						Console.CursorLeft = stringPosition;
					}
					break;
				case ConsoleKey.Enter: // return s
					if (s.Trim().Length>0)
						run = false;
					stringPosition = 0;
					break;
				case ConsoleKey.End: // go to end
					stringPosition = s.Length;
					Console.CursorLeft = stringPosition;
					break;
				case ConsoleKey.Home: // go to beginning
					stringPosition = 0;
					Console.CursorLeft = stringPosition;
					break;
				case ConsoleKey.RightArrow: // go one character right
					if (s.Length>(stringPosition))
					{
						stringPosition++;
						Console.CursorLeft = stringPosition;
					}						
					break;
				case ConsoleKey.LeftArrow: // go one character left
					if (stringPosition>0)
					{
						stringPosition--;
						Console.CursorLeft = stringPosition;
					}						
					break;
				case ConsoleKey.UpArrow: // List through commands
					whichFromEnd += 1;
					if (whichFromEnd > cmds.Count)
						whichFromEnd = cmds.Count;
					whichToDisplay = cmds.Count-whichFromEnd;
					if (whichToDisplay >= 0 & cmds.Count > 0)
					{
						Console.CursorLeft = 0;
						int w = Console.WindowWidth;
						for (int i=1; i<w; i++)
							Console.Write(" ");
						Console.CursorLeft = 0;
						Console.Write(cmds[whichToDisplay]);
						s = cmds[whichToDisplay];
					}
					stringPosition = s.Length;
					break;
				case ConsoleKey.DownArrow: // List through commands
					whichFromEnd += -1;
					if (whichFromEnd <=0)
						whichFromEnd = 1;
					whichToDisplay = cmds.Count-whichFromEnd;
					if (whichToDisplay < cmds.Count & cmds.Count > 0)
					{
						Console.CursorLeft = 0;
						int w = Console.WindowWidth;
						for (int i=1; i<w; i++)
							Console.Write(" ");
						Console.CursorLeft = 0;
						Console.Write(cmds[whichToDisplay]);
						s = cmds[whichToDisplay];
					}
					stringPosition = s.Length;
					break;
				default:
					int asciicode = (int)keyhit.KeyChar;
					if (asciicode<255) //better condition
					{
						s = s.Insert(stringPosition, keyhit.KeyChar.ToString());
						stringPosition++;
						char[] charlist = s.ToCharArray();
						for (int i=stringPosition; i<charlist.Length; i++)
							Console.Write(charlist[i]);
						Console.CursorLeft = stringPosition;
					}						
					break;
				}
			
			}
			return s;
		}

    }   
	
	public class LimitedQueue<T> : Queue
	{
	    private int limit = -1;
	
	    public int Limit
	    {
	        get { return limit; }
	        set { limit = value; }
	    }
	
	    public LimitedQueue(int limit)
	        : base(limit)
	    {
	        this.Limit = limit;
	    }
	
	    public void Enqueue(T item)
	    {
	        if (this.Count >= this.Limit)
	        {
	            this.Dequeue();
	        }
	        base.Enqueue(item);
	    }
	}
	
	
}
