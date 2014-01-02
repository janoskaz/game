---Interaction with torch

--Set the problem, give player some options
Console.WriteLine("This torch just can't seem to lit up. What will you do?")
::choice::
Console.WriteLine("\t1: Curse very loudly.")
Console.WriteLine("\t2: Try harder.")
Console.WriteLine("\t3: Promise the soul of your first born son to Ahnutep, the Eater of the Dead.")

--Inner dialog
answer = get_answer(3)

if (answer == "1") then --Cursing never helps
	Console.WriteLine("\t1: Curse very loudly.")
	Console.WriteLine("No matter how vivid and loud your swearing is, not a single spart appears")
	goto choice
elseif (answer == "2") then --This is not right wing propaganda
	Console.WriteLine("\t2: Try harder.")
	Console.WriteLine("After five minutes, you are all sweaty and tired, and... yes, still sitting in a pitch dark.")
	goto choice
elseif (answer == "3") then --Yes, lets, the Gods decide
	Console.WriteLine("\t3: Promise the soul of your first born son to Ahnutep, the Eater of the Dead.")
	tell_message("The torch imediately catches fire. That was easy.")
	message = "Finally, you can see. But you wish you weren't. You are in a small, hermetically sealed tomb with no exit. Perhaps there will be some way out, but so far, you can't see anything"
	newscript = "null"
	torch = true
	visibility = 2
end