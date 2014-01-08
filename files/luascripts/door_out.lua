message = "This is simple stone plate, used to seal the entrance to the tomb."

has_showel = player:HasItem("Showel")
keepscript = true
out = block

if (has_showel) then
	tell_message("You could try to use the showel as a lever. Do you want to?")
	Console.Write("\t1: Yes.\n")
	Console.WriteLine("\t2: No.")
	answer = get_answer(2)
	if (answer == "1") then
		tell_message("You carefully place the showel to the bottom of the doors and use all your weight to move them. You hear the stone plate shifting on the floor, when suddenly the showel breaks into half. Luckily, the doors moved enough to allow you to scrape through the hole and leave this stale dungeon.")
		showel = player:GetItem("Showel")
		player:DropItem(showel)
		keepscript = false
		block:OpenDoor()
		out = block
	end
else
	message = message .. " Only if you had some lever to move it."
end
