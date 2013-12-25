---File with usefull functions etc

if not luanet then require 'luanet' end
luanet.load_assembly "System"
Console = luanet.import_type "System.Console"
local import_type, load_assembly = luanet.import_type, luanet.load_assembly

--Sleep function
function sleep(n)
  os.execute("sleep " .. tonumber(n))
end

--Load classes
Being = import_type("Game.Being")
Player = import_type("Game.Player")
BasicObject = import_type("Game.BasicObject")
Corpse = import_type("Game.Corpse")
