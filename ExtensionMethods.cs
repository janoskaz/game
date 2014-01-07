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
				case ConsoleKey.Backspace:
					if (s.Length > 0)
						s = s.Substring(0,s.Length-1);
					break;
				case ConsoleKey.Enter: // return s
					if (s.Trim().Length>0)
						run = false;
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
					break;
				case ConsoleKey.DownArrow:
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
					break;
				default:
					int asciicode = (int)keyhit.KeyChar;
					if (asciicode<255)
						s += keyhit.KeyChar.ToString();
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
