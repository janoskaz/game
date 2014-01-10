-- reset all variables,which are used in this level,excluding visibility
config={}
config.visibility=2

message = "leave"
keepscript = false
out = block

--save player, his configuration and current dungeon
ThisGame.SaveGame("dungeon1")

ThisGame.LoadNextMap("dungeon2")

--Console.Clear()

--tell_message("You leave the dungeon behind and put on a journey accross the City of Dead, large burial grounds, where all the ancestors remain (before someone robs their graves, leaving their remains for jackals to eat, but lets not be cynical.)")
--Console.WriteLine("The City of Death lies outside Akhbar, the Citi of Living, and although its not that far, the Sun is burning and you became exhausted and thirsty. There is Elin, the Big River just dozens of hundreds of dranech, which sounds far, but is actually quite close.\nDo you wish to go there and drink some water?")
--Console.WriteLine("\t1: Go to the river")
--Console.WriteLine("\t2: Ignore the river and continue to the city.")
--answer = get_answer(2)

