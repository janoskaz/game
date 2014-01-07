message = "This door is huge, made from stone and covered with elaborate carwings, resembling different kinds of animals."

out = block
keepscript = true

-- create table with bool values for every amulet
amulets = {player:HasItem("Amulet with owl"), player:HasItem("Amulet with fish"), player:HasItem("Amulet with jackal"), 
player:HasItem("Amulet with cat"), player:HasItem("Amulet with camel"), player:HasItem("Amulet with hippo"), 
player:HasItem("Amulet with octopus"), player:HasItem("Amulet with crocodile")}

--function tocheck, if player has all the amulets, or any of the amulets
has_amulets = function(tb, all)
	for i,v in ipairs(tb) do
		if ((not v) and all) then return false end
		if (v and (not all)) then return true end
	end
	return true
end

--function to create annoying hourglass
hourglass = function(pos)
	Console.Clear()
	local pos = {{1,1},{1,2},{1,3},{1,4},{1,5},{1,6},{1,7},{1,8},{1,9},
				{2,2},{2,8},{3,3},{3,7},{4,4},{4,6},{5,5},{6,4},{6,6},{7,3},{7,7},{8,2},{8,8},
				{9,1},{9,2},{9,3},{9,4},{9,5},{9,6},{9,7},{9,8},{9,9}}
	local sand = {{2,3},{2,4},{2,5},{2,6},{2,7},{3,4},{3,5},{3,6},{4,5}}
	local transition = {{2,5,8,5},{2,4,8,6},{2,6,8,4},{2,3,8,7},{2,7,8,3},{3,5,7,5},{3,4,7,6},{3,6,7,4},{4,5,6,5}}

	local w = math.floor(Console.WindowWidth/2)
	local h = 4
	for i,v in ipairs(pos) do
		Console.CursorTop = h+v[1]
		Console.CursorLeft = w - 5 + v[2]
		Console.Write("X")
	end
	for i,v in ipairs(sand) do
		Console.CursorTop = h+v[1]
		Console.CursorLeft = w - 5 + v[2]
		Console.Write(".")
	end
	for i,v in ipairs(transition) do
		Console.CursorTop = h+v[1]
		Console.CursorLeft = w - 5 + v[2]
		Console.Write(" ")
		Console.CursorTop = h+v[3]
		Console.CursorLeft = w - 5 + v[4]
		Console.Write(".")
		Console.CursorTop = 0
		Console.CursorLeft = 0
		sleep(1)
	end
end

if (has_amulets(amulets, true)) then --does the player has all of the amulets?
	tell_message("You place amulets inside carvings and suddenly you hear a sound of sand piling up somewhere.")
	hourglass()
	message = "The door opens and you enter the burial chamber."
	keepscript = false
	block:OpenDoor()
	out = block
elseif (has_amulets(amulets, false)) then --does he has at least some amulets?
	message = message .. " Some of the amulets that you have seem to be fitting inside the carvings, but you will need more."
else
	message = message .. " You can not open the door."
end