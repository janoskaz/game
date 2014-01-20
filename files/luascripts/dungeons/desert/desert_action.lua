config.visibility = 6

keepscript = true
out = block

if (map:AllExplored() and not config.explored_desert) then
	tell_message("You explored the whole desert! You haven't found your son, or fame, fortune... in fact, you did not find anything - not even an inner peace.\n But your body got stronger, and you gain two hitpoints and one points of strength")
	sleep(2)
	player:PermanentlyUpdateCharacteristics(2,1,0,0)
	keepscript = false
	config.explored_desert = true
end
