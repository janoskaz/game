----
--interaction with goblin
---

--AutomaticAction
Console.WriteLine("Goblin> Ha! You will die, puny human!")
Console.WriteLine("\n\tWhat will you do?\n \t1: Attack Goblin\n\t2: Negotiate")

-- get answer from user
local answer
repeat
    answer=Console.ReadLine()
until answer=="1" or answer=="2"
--TODO: when user writes, he doesn't see the letters

--perform action based on answer
if (answer=="1") then
	val = player:Fight(block)
else
	Console.WriteLine("You> What about we make a deal?\n")
	sleep(1)
	Console.WriteLine("Goblin> What kind of deal?\n")
	sleep(1)
	Console.WriteLine("You> What do you say about me, giving you all my gold?\n")
	sleep(1)
	Console.WriteLine("Goblin> You shallow materialist! The real value is in the soul. But clearly not in yours... AAARGH?\n\n")
	sleep(2)
	val = player:Fight(block)
end

--if the fight ended with player being alive, return goblin, else - return corpse
keepscript = true
newscript = "null"
if (val) then
	out = block:becameCorpse()
	keepscript = false
else
	out = block
end