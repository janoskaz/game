dofile("/home/zbynek/Plocha/csharp/Game/Game/files/config.lua")
--TODO: relative paths

----
--interaction with goblin
---

--AutomaticAction
io.write("Goblin> Ha! You will die, puny human!")
io.write("\n\tWhat will you do?\n \t1: Attack Goblin\n\t2: Negotiate\n")

-- get answer from user
local answer
repeat
    answer=io.read()
	io.flush()
until answer=="1" or answer=="2"
--TODO: when user writes, he doesn't see the letters

--perform action based on answer
if (answer=="1") then
	val = player:Fight(goblin)
else
	io.write("You> What about we make a deal?\n")
	sleep(1)
	io.write("Goblin> What kind of deal?\n")
	sleep(1)
	io.write("You> What do you say about me, giving you all my gold?\n")
	sleep(1)
	io.write("Goblin> You shallow materialist! The real value is in the soul. But clearly not in yours... AAARGH?\n\n")
	sleep(2)
	val = player:Fight(goblin)
	
end

--if the fight ended with player being alive, return goblin, else - return corpse
if (val) then
	out = goblin:becameCorpse()
else
	out = goblin
end