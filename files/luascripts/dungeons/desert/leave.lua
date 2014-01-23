message = "leave"
keepscript = false
out = block

--save player, his configuration and current dungeon
ThisGame.SaveGame("desert")

--go to next level
dofile(path_to_files .. "luascripts/dungeons/directory.lua")
