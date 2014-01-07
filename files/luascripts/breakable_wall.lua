message = "This wall looks quite weak, you could try to break it."

out = block
keepscript = true

if (player:HasItem("Hammer")) then
	message = message .." The wall is too strong to be broken with a hammer, you will need something bigger."
end
if (player:HasItem("Statue of Qebehsutep")) then
	Console.WriteLine("You could try to use the statue of Qebehsutep as a ram.")
	Console.WriteLine("\t1: Do it.")
	Console.WriteLine("\t2: Don't do it.")
	answer = get_answer(2)
	if (answer == "1") then
		tell_message("You swing the statue several times before the wall breaks down. The dust is slowly settling and you can see small room with two bodies.")
		message = "You broke through the wall, using sacred item. Well done."
		out = BasicObject()
		keepscript = false
	end
end