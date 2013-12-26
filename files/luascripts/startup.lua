---File with usefull functions etc
if not luanet then require 'luanet' end
import_type, load_assembly = luanet.import_type, luanet.load_assembly

--Load classes / game
load_assembly "Game"

Map = import_type "Game.Map"
Location = import_type "Game.Location"

BasicObject = import_type "Game.BasicObject"
Wall = import_type "Game.Wall"

Player = import_type "Game.Player"
Being = import_type "Game.Being"

Corpse = import_type "Game.Corpse"

Item = import_type "Game.Item"

Inventory = import_type "Game.Inventory"

Characteristics = import_type "Game.Characteristics"

--Load classes / system
load_assembly "System"
Console = import_type "System.Console"

--Sleep function
function sleep(n)
	os.execute("sleep " .. tonumber(string.format("%.2f",n)))
end

--Say function
function say(who, what)
	Console.WriteLine(who .. "> " .. what .. "\n")
	sleep(1)
end

--Message function
function tell_message(what)
	Console.WriteLine(what .. "\n")
	sleep(2)
end

