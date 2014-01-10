
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
map:CreateMapField(11,11)
map.PlayerX = 0
map.PlayerY = 5
--Write to XML
map:ToXml("dungeon2")
