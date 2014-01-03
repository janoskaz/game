---Create dungeon and save it as XML document

if not luanet then require 'luanet' end
local import_type, load_assembly = luanet.import_type, luanet.load_assembly

load_assembly "Game"
Map = import_type "Game.Map"
Location = import_type "Game.Location"
Player = import_type "Game.Player"
Being = import_type "Game.Being"
BasicObject = import_type "Game.BasicObject"
Wall = import_type "Game.Wall"
Door = import_type "Game.Door"
Chest = import_type "Game.Chest"
Item = import_type "Game.Item"
Weapon = import_type "Game.Weapon"
Inventory = import_type "Game.Inventory"
Characteristics = import_type "Game.Characteristics"

load_assembly "System"

String = import_type "System.String"

--Create map with given dimension
local map = Map()
map:CreateMapField(26,26)

---CREATE ALL WALLS
--Small rooms
for i=1,4 do --rooms on left side
	map:AddLocation(Location(i,0, Wall()))
	map:AddLocation(Location(i,3, Wall()))
	map:AddLocation(Location(i,6, Wall()))
end

for i=1,5 do
	map:AddLocation(Location(1,i, Wall()))
	map:AddLocation(Location(4,i, Wall()))
end

for i=8,11 do --rooms on right side
	map:AddLocation(Location(i,0, Wall()))
	map:AddLocation(Location(i,3, Wall()))
	map:AddLocation(Location(i,6, Wall()))
end

for i=1,5 do
	map:AddLocation(Location(8,i, Wall()))
	map:AddLocation(Location(11,i, Wall()))
end

--breakable walls
brokenWall = Location(4,1, Wall()) -- upper left room - entry room
brokenWall:SetScript("broken_wall.lua")
map:AddLocation(brokenWall)
wall = Location(8,1,Wall()) -- upper right room
wall:SetScript("breakable_wall.lua")
map:AddLocation(wall)
wall = Location(4,5,Wall()) -- lower left room
wall:SetScript("breakable_wall.lua")
map:AddLocation(wall)
wall = Location(8,4,Wall()) -- lower right room
wall:SetScript("breakable_wall.lua")
map:AddLocation(wall)

--big room north
map:AddLocation(Location(5,0, Wall()))
map:AddLocation(Location(6,0, Wall()))
map:AddLocation(Location(7,0, Wall()))
map:AddLocation(Location(4,7, Wall()))
map:AddLocation(Location(5,7, Wall()))
map:AddLocation(Location(7,7, Wall()))
map:AddLocation(Location(8,7, Wall()))

--hall to central chamber
for i=8,11 do
	map:AddLocation(Location(5,i, Wall()))
	map:AddLocation(Location(7,i, Wall()))
end

--central chamber
map:AddLocation(Location(3,11, Wall())) -- 1st line of walls
map:AddLocation(Location(4,11, Wall()))
map:AddLocation(Location(8,11, Wall()))
map:AddLocation(Location(9,11, Wall()))
map:AddLocation(Location(1,12, Wall())) -- 2nd line of walls
map:AddLocation(Location(2,12, Wall()))
map:AddLocation(Location(3,12, Wall()))
map:AddLocation(Location(9,12, Wall()))
map:AddLocation(Location(10,12, Wall()))
map:AddLocation(Location(11,12, Wall()))
map:AddLocation(Location(0,13, Wall())) -- 3rd line of walls
map:AddLocation(Location(1,13, Wall()))
map:AddLocation(Location(11,13, Wall()))
map:AddLocation(Location(12,13, Wall()))
map:AddLocation(Location(0,14, Wall())) -- 4th line of walls
map:AddLocation(Location(12,14, Wall()))
map:AddLocation(Location(0,15, Wall())) -- 5th line of walls
map:AddLocation(Location(0,16, Wall())) -- 6th line of walls
map:AddLocation(Location(12,16, Wall()))
map:AddLocation(Location(0,17, Wall())) -- 7th line of walls
map:AddLocation(Location(1,17, Wall()))
map:AddLocation(Location(11,17, Wall()))
map:AddLocation(Location(12,17, Wall()))
map:AddLocation(Location(1,18, Wall())) -- 8th line of walls
map:AddLocation(Location(2,18, Wall()))
map:AddLocation(Location(3,18, Wall()))
map:AddLocation(Location(9,18, Wall()))
map:AddLocation(Location(10,18, Wall()))
map:AddLocation(Location(11,18, Wall()))
map:AddLocation(Location(3,19, Wall())) -- 9th line of walls
map:AddLocation(Location(4,19, Wall()))
map:AddLocation(Location(5,19, Wall()))
map:AddLocation(Location(7,19, Wall()))
map:AddLocation(Location(8,19, Wall()))
map:AddLocation(Location(9,19, Wall()))
for i=20,22 do -- lines 10 to 12
	map:AddLocation(Location(3,i, Wall()))
	map:AddLocation(Location(9,i, Wall()))
end
for i=22,24 do -- lines 12 to 14
	map:AddLocation(Location(4,i, Wall()))
	map:AddLocation(Location(8,i, Wall()))
end
map:AddLocation(Location(5,24, Wall()))
map:AddLocation(Location(7,24, Wall()))
map:AddLocation(Location(5,25, Wall())) -- line 15
map:AddLocation(Location(6,25, Wall()))
map:AddLocation(Location(7,25, Wall()))

--hall out
for i=12,25 do -- lines 22 to 24
	map:AddLocation(Location(i,14, Wall()))
	map:AddLocation(Location(i,16, Wall()))
end

--ENTRY ROOM - bonus functionality
--Add corpse in corner
inv = Inventory(10) -- create inventory
torch = Item("Torch") -- torch
torch:SetScript("torch.lua")
inv:Add(torch)
ss = String[1] -- hammer
ss[0] = "weapon"
hammer = Weapon("Hammer", Characteristics(0,2,0,0), ss, 6)
inv:Add(hammer)
amulet_owl = Item("Amulet with owl") -- amulet with owl
inv:Add(amulet_owl)
deadman1 = Chest("Dead body", inv) -- create dead body
deadman1:SetDescription("You have stumbled upon a corpse. Literary - you have stumbled and fallen down.")
--TODO: previous line is not working
map:AddLocation(Location(2,1, deadman1))

--REST OF THE SMALL ROOMS
inv = Inventory(10) -- dead men 1 in upper right room
amulet_fish = Item("Amulet with fish") -- amulet with fish
inv:Add(amulet_fish)
deadman2= Chest("Dead body", inv) -- create dead body
map:AddLocation(Location(10,1, deadman2))

inv = Inventory(10) -- dead men 2 in upper right room
amulet_jackal = Item("Amulet with jackal") -- amulet with jackal
inv:Add(amulet_jackal)
deadman3= Chest("Dead body", inv) -- create dead body
map:AddLocation(Location(10,2, deadman3))

inv = Inventory(10) -- dead men 1 in lower right room
amulet_cat = Item("Amulet with cat") -- amulet with cat
inv:Add(amulet_cat)
deadman4= Chest("Dead body", inv) -- create dead body
map:AddLocation(Location(10,4, deadman4))

inv = Inventory(10) -- dead men 2 in lower right room
amulet_camel = Item("Amulet with camel") -- amulet with camel
inv:Add(amulet_camel)
ss = String[2] -- showel
ss[0] = "weapon"
ss[1] = "shield"
showel = Weapon("Showel", Characteristics(0,3,1,0), ss, 8)
inv:Add(showel)
deadman5= Chest("Dead body", inv) -- create dead body
map:AddLocation(Location(10,5, deadman5))

inv = Inventory(10) -- dead men 1 in lower left room
amulet_hippo = Item("Amulet with hippo") -- amulet with hippo
inv:Add(amulet_hippo)
deadman5= Chest("Dead body", inv) -- create dead body
map:AddLocation(Location(2,4, deadman5))

--Mad men in lower left room
amulet_octopus = Item("Amulet with octopus") -- amulet with hawk
ch = Characteristics(3,3,1,0)
madman = Being("Mad men", ch, ch, 10)
madman:PickItem(amulet_octopus)
madman = Location(2,5, madman)
madman:SetScript("madman.lua")
map:AddLocation(madman)

--Statue in the main chamber
statue = Item("Statue of Qebehsutep")
statue:SetSymbol('S')
statue = Location(2,15,statue)
statue:SetScript("statue.lua")
statue:SetSymbol('S')
map:AddLocation(statue)

--Doors to burial chamber
doors = Location(6,19,Door("This door is huge, made from stone and covered with elaborate carwings, resembling different kinds of animals.","special key",true))
doors:SetScript("burial_chamber_door.lua")
doors:UpdateSymbol()
map:AddLocation(doors)


--Write to XML
map:ToXml("dungeon1")