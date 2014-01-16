-- reset all variables,which are used in this level,excluding visibility
config={}
config.visibility=2

message = "leave"
keepscript = false
out = block

--save player, his configuration and current dungeon
ThisGame.SaveGame("dungeon1")

Console.Clear()
tell_message("You leave the dungeon behind and put on a journey accross the City of Dead, large burial grounds, where all the ancestors remain (before someone robs their graves, leaving their remains for jackals to eat, but lets not be cynical.)")
tell_message("The City of Death lies outside Akhbar, the City of Living, and although its not that far, the Sun is burning and you became exhausted and thirsty.")
tell_message("When you finally get to the city gates, you can barely walk. You pass two guards, who look at you suspiciously but let you go, and go straight home.")
tell_message("Your home is standing in the poor neighborhood and it is basicaly just a shed with one big room. You don't even have doors, just some rugs hanging in the entrance. You walk through them, smile at your wife and shout: \"Surprise!\".")
tell_message("To your big surprise, she picks the closest blunt object and with genuine gust bashes your head. Unfortunately, that does not knock you unconsious, but you loose balance and fall down. Your head hits the floor and THAT knocks you unconsious.")

Console.WriteLine("...")
sleep(5)
tell_message("You wake up and once again, you have headache like hell.")
Console.WriteLine("\"You bastard!\" says familiar voice.")
Console.WriteLine("\t1:\"Honey...?\"")
Console.WriteLine("\t2:\"You bitch!\"")
Console.WriteLine("\t3:Pretend that you are asleep.")

answer = get_answer(3)

if (answer=="1") then
	Console.WriteLine("\t1:\"Honey...?\"")
	Console.WriteLine("\"Don't you dare.\"")
	goto revelation
	-- leave my mother out of it
elseif (answer=="2") then
	Console.WriteLine("\t2:\"You bitch\"")
	if (player:HasItem("Hammer")) then
		Console.WriteLine("Your wife freezes and her face turns red. She picks the closest object and to your bad luck, it's a hammer, which you for some strange reason took back from the tomb.")
	else
		Console.WriteLine("Your wife freezes and her face turns red. She picks the closest object and to your bad luck, it's a knife.")
	end
	Console.WriteLine("\t1:I bet you wouldn't...")
	Console.WriteLine("\t2:Oh shit. Baby, I am soooo sorry...")
	answer = get_answer(2)
	tell_message("Too soon you realize, that women should be treated better than what the Bible says. First strike hits you in the chest, second in the head, and the third you don't feel, because your dead.")
	player:Die()
	goto eof
else
	Console.WriteLine("\t3:Pretend that you are asleep.")
	Console.WriteLine("\"I know you are awake.\"")
	Console.WriteLine("\t1:Pretend that you are asleep.")
	Console.WriteLine("\t2:Okay")
	answer = get_answer(2)
	if (answer=="1") then
		Console.WriteLine("\t1:Pretend that you are asleep")
		Console.WriteLine("You are unbelievable. Why are you such a coward? And also, why the hell did I marry you?")
		Console.WriteLine("\t1:Pretend that you are asleep.")
		Console.WriteLine("\t2:Hey, I do not deserve this!")
		Console.WriteLine("\t3:If anyone, I would blame your mother")
		answer = get_answer(3)
		if (answer=="1") then
			Console.WriteLine("\t1:Pretend that you are asleep.")
			Console.WriteLine("\"I've had enough with you.\" Your wife picks your leg and pulls you from the bed.")
			goto revelation
		elseif (answer=="2") then
			Console.WriteLine("\t2:Hey, I do not deserve this!")
			Console.WriteLines("Oh really?")
			goto revelation
		else
			Console.WriteLine("\t3:If anyone, I would blame your mother")
			Console.WriteLine("Oh... Yes, I suppose that's true. Anyway...")
			goto revelation
		end
	else
		Console.WriteLine("\t2:Okay")
		goto revelation
	end
end

::revelation::
Console.WriteLine("\"Do you even know what you have done?\"")
tell_message("\t\"Actually...\"")
Console.WriteLine("\"You promised the soul of Hamidhapet, our only son, to Ahnutep, the Eater of the Dead in exchange for a fire!\"")
tell_message("\tYou vaguely remember that you indeed promised soul of your youngest sson to the god of afterlife, if the torch catches fire.")
Console.WriteLine("\"And just few hours back, he appeared on the doorstep, took Hamidhapet, told me this, and dissapeared!\"shouts your wife.")
Console.WriteLine("\t1:\"I did not know, that the gods are listening.\"")
Console.WriteLine("\t2:\"I'm sure he'll be fine.\"")
Console.WriteLine("\t3:\"Oh Gods, what have I done!\"")
get_answer(3)

if (answer=='1') then
	Console.WriteLine("\t1:\"I did not know, that the gods are listening.\"")
	tell_message("\"Well, you were wrong. Now get your things and go find him.\"")
elseif (answer=='2') then
	Console.WriteLine("\t2:\"I'm sure he'll be fine.\"")
	tell_message("\"The hell he will. You will go and bring him back\"")
else
	Console.WriteLine("\t3:\"Oh Gods, what have I done!\", you cry and start sobbing.")
	tell_message("\"Stop it. Crying won't bring him back. You will!\"")
end

tell_message("Your wife hands you small bag with some food, water and a knife. You want to kiss her goodbye, but she doesn't seem to be in the mood, so you just make an awkward smile and leave.")

dofile(path_to_files .. "luascripts/directory.lua")

::eof::


