
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
ThisGame = import_type "Game.ThisGame"

load_assembly "System"

String = import_type "System.String"

local map = Map()
map:CreateMapField(100,100)

for i=0,99 do
	for j=0,99 do
		rnd = math.random()
		if rnd<0.05 then
			rock = Impassable("rock")
			rock:SetSymbol('.')
			loc = Location(i,j,rock)
			--loc:UpdateSymbol()
		elseif rnd<0.08 and rnd>=0.05 then
			loc = Location(i,j,Impassable("cactus"))
		else
			loc = Location(i,j,BasicObject())
			loc:SetScript("desert_action.lua")
		end
		map:AddLocation(loc)
	end
end

map.PlayerX = 0
map.PlayerY = 5
--Write to XML
map:ToXml("desert")
