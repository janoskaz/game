tell_message("In front of you is statue of Qebehsutep, goddes of deceased's intestines. She looks like a fat woman with owl's head.\nThe statue carved out of stone, is about 5 dranech high you estimate its weight to about 13 huppeth, which is just the right size to use it as a ram.")
Console.WriteLine("What will you do?")

keepscript = true
block:SetSymbol("S")
out = block

::conversation::
Console.WriteLine("\t1: Stare at the statue for a little longer.")
Console.WriteLine("\t2: Pray to Qebehsutep.")
Console.WriteLine("\t3: Pray to Amon.")
Console.WriteLine("\t4: Take the statue.")
Console.WriteLine("\t5: Leave.")

answer = get_answer(5)

if (answer == "1") then
	Console.WriteLine("1: Stare at the statue for a little longer.")
	tell_message("You stare at the statue.")
	goto conversation
elseif (answer == "2") then
	Console.WriteLine("2: Pray to Qebehsutep.")
	if (pray) then 
		tell_message("You have already tried that") 
	else
		tell_message("\"All hail to the mighty Qebehsutep, the queen of all the entrails - livers, stomachs, kidneys and kidney stones. I praise your heavenly presence in this temple.\"")
		tell_message("Your prayers have been answered and in case you get mummified one day, you know that your guts will safely get to the heavens.")
		pray = true
	end
	goto conversation
elseif (answer == "3") then
	Console.WriteLine("3: Pray to Amon.")
	tell_message("What are you, mad?") 
	goto conversation
elseif (answer == "4") then
	Console.WriteLine("4: Take the statue.")
	if (has_statue) then 
		tell_message("You have already done that.") 
		goto conversation
	else
		tell_message("You contemplate for a while, if something bad will happen to you. Then you take the statue and nothing happens.")
		player:PickItem(block)
		out = BasicObject()
		keepscript = false
		has_statue = true
	end
	goto conversation
elseif (answer == "5") then
	tell_message("5: Leave.")
end