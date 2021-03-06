
if not luanet then require 'luanet' end
local import_type, load_assembly = luanet.import_type, luanet.load_assembly

load_assembly "Game"
Map = import_type "Game.Map"
Location = import_type "Game.Location"
Player = import_type "Game.Player"
Being = import_type "Game.Being"
BasicObject = import_type "Game.BasicObject"
Impassable = import_type "Game.Impassable"
Door = import_type "Game.Door"
Chest = import_type "Game.Chest"
Item = import_type "Game.Item"
Weapon = import_type "Game.Weapon"
Inventory = import_type "Game.Inventory"
Characteristics = import_type "Game.Characteristics"
ThisGame = import_type "Game.ThisGame"

load_assembly "System"

String = import_type "System.String"

desertsize = 100
local map = Map()
map:CreateMapField(desertsize,desertsize)

for i=0,(desertsize-1) do
	for j=0,(desertsize-1) do
		rnd = math.random()
		if rnd<0.03 then
			rock = Impassable("rock")
			rock:SetSymbol('.')
			loc = Location(i,j,rock)
			--loc:UpdateSymbol()
		elseif rnd<0.04 and rnd>=0.02 then
			loc = Location(i,j,Impassable("cactus"))
		else
			loc = Location(i,j,BasicObject())
			loc:SetScript("desert_action.lua")
		end
		map:AddLocation(loc)
	end
end

--leave
leave = BasicObject()
leave:SetSymbol('X')
loc = Location(0,5,leave)
loc:SetScript("leave.lua")
loc:UpdateSymbol()
map:AddLocation(loc)

map.PlayerX = 1
map.PlayerY = 5
--Write to XML
map:ToXml("dungeons/desert")
