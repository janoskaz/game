--- Interaction with Mad Man

if (had_conversation_with_madman) then
	goto visited
end

--possible players sentences
s1 = "\t 1:\"What?\""
s2 = {"\t 2:\"No, it's actually thursday.\"", "\t2:\"Nothing better than good old fashioned stoning of an infidel, I totally agree.\"", "\t2:\"Did you know, that storks can sleep standing only on one leg?.\"", "\t2: \"You are clearly mad, Hamuneferi's hips are way too fat\"", "\t2:\"Who wouldn't?! That's the best way to prepare octopus.\""}
s3 = "\t 3:\"Anuh homtet rabbasete nih.\""
s4 = "\t 4:Attack him."

--possible mad mans sentences
random_words = {"halepnuhet", "anhk", "solme", "neser", "maha", "taudeh", "nun", "happe", "nefer", "jofer", "siri", "hin", "...Ahen", "ujme", "unhe", "ramtane"}
function mad_sentence_generator()
	rnd = math.random(math.floor(#random_words/2))
	mad_sentence = "\""
	for i=1,rnd do
		mad_sentence = mad_sentence .. " " .. random_words[math.random(#random_words)]
	end
	mad_sentence = mad_sentence .. ".\""
	return mad_sentence
end

tell_message("In the corner, you see small, shiverring silhouette of the man. His eyes are wide open and his mouth is drooling.")
Console.Write("The man points his shaking hand at you and asks: \"Ahk nun tahep unhe loto?\"")

function conversation(news)
	Console.WriteLine(s1)
	Console.WriteLine(news)
	Console.WriteLine(s3)
	Console.WriteLine(s4)
end

round = 1

--conversation with madman
::conversation::
had_conversation_with_madman = true
conversation(s2[round])
answer = get_answer(4)

if (answer == "1") then --neverending story
	say("You",s1)
	say("Mad man",mad_sentence_generator())
	goto conversation
elseif (answer == "2") then
	say("You",s2[round])
	if (round == 5) then --after five nonsensical sentecesm you get the amulet
		goto get_amulet
	end
	say("Mad man", mad_sentence_generator())
	round = round + 1
	goto conversation
elseif (answer == "3") then --dont make fun of mad people
	say("You",s3)
	Console.WriteLine("On mad mans face, bewildereness is replaced by anger. Somehow, you managed to gravely insult him. He charges at you, madly wawing his fists and screaming.")
	goto fight
elseif (answer == "4") then --words not swords
	Console.WriteLine(s4)
	goto fight
end

--fight the mad man
::fight::
val = player:Fight(block)
if (val) then
	out = block:BecameCorpse()
else
	out = block
end
tell_message("You have been bashing things for some time now, so it's time to get some reward... your attack skill has been slightly increased.")
player:PermanentlyUpdateCharacteristics(0,1,0,0) --increase attack
keepscript = false
message = "You defeated mad man!"
goto endoffile	

--outsmart mad man to give you the amulet
::get_amulet::
tell_message("You see a spark of understanding in mad mans eyes. He stretches his arm and gives you amulet with octopus.")
tell_message("By outsmarting mad man (frankly, you were just lucky), you got little more cunning, which gives you small advantage in future fights.")
amulet = block:GetItem("Amulet with octopus")
block:DropItem(amulet)
player:PickItem(amulet)
player:PermanentlyUpdateCharacteristics(0,0,1,0) --increase defence
keepscript = true
out = block
message = "Mad man is staring at you with his blank eyes. There is nothing you can do for him."
goto endoffile

--if you visited mad man and lived, dont do anything
::visited::
message = "Mad man is staring at you with his blank eyes. There is nothing you can do for him."
out = block
keepscript = true

::endoffile:: --just end
