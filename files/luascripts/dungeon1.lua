---Create dungeon and save it as XML document

if not luanet then require 'luanet' end
local import_type, load_assembly = luanet.import_type, luanet.load_assembly

load_assembly "Game"
Map = import_type "Game.Map"
Location = import_type "Game.Location"
Player = import_type "Game.Player"
Being = import_type "Game.Being"
Wall = import_type "Game.Wall"
Corpse = import_type "Game.Corpse"
Item = import_type "Game.Item"
Inventory = import_type "Game.Inventory"
Characteristics = import_type "Game.Characteristics"

--Create map with given dimension
local map = Map()
map:CreateMapField(4,4)

-- Add walls on the perimeter
for i=0,3 do
	map:AddLocation(Location(i,0, Wall()))
	map:AddLocation(Location(i,3, Wall()))
end

for i=1,3 do
	map:AddLocation(Location(0,i, Wall()))
	map:AddLocation(Location(3,i, Wall()))
end

--Add corpse in corner
torch = Item("Torch")
torch:SetScript("torch.lua")
inv = Inventory(10)
inv:Add(torch)
deadman = Corpse("Dead body", inv)
deadman:SetDescription("You have stumbled upon a corpse. Literary - you have stumbled and fallen down.")
--TODO: previous line is not working
map:AddLocation(Location(1,1, deadman))

--Add broken wall
brokenWall = Location(3,1, Wall())
brokenWall:SetScript("broken_wall.lua")
map:AddLocation(brokenWall)

--Write to XML
map:ToXml("dungeon1")