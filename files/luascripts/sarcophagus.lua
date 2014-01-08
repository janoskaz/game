tell_message("You are looking down to sarcophagus. Ornaments on its surface show Khasekhemre, pharaohs chief accountant, standing on backs of 8 servants, who carry him accross Humpf, the swamp of Doom, which lies between the land of living and the land of dead. You see 8 Gods with animal heads, watching over him to safely arrive.")
tell_message("\"All right\", you tell to yourself, that explains all of this.\nNow what will you do?")

times_opened = 0
keepscript = true
out = block

::options::
Console.Write("\t1: Open the sarcophagus.\n")
Console.WriteLine("\t2: Leave it alone.")

answer = get_answer(2)
if (answer == "1") then
	times_opened = times_opened + 1
	if (times_opened == 2) then
	--better message
		tell_message("You open the sarcophagus for the second time and it's still full of paper and ink. Annoyed, you pick randomly one of the pieces of papyrus and start reading. It's a story of hare and turtle, and although you did not learn how to run, you learned a lot about of running. You gain one point of speed.")
		player:PermanentlyUpdateCharacteristics(0,0,0,1)
		keepscript = false
	else
		tell_message("You carefully move the lid of the sarcophagus. Inside is laying mummy of Hrasekhmere with the things, which he loved the most on this world - papers and ink. Not a single piece of gold or anything like it. That's a dissapointment. One would expect some treasure, of mighty artefact, after all that troubles you had to go through to get here. Well, maybe NEXT TIME.")
		goto options
	end
else
	tell_message("You leave the sarcophagus alone.")
end

explored_sarcophagus = true
message = "You explored the sarcophagus"