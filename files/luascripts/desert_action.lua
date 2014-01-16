config.visibility = 10000

all_explored = map:AllExplored()
keepscript = true
out = block

if (all_explored) then
	tell_message("You explored the whole desert! You haven't found your son, or fame, fortune... in fact, you did not find anything - not even an inner peace.\n But your body got stronger, and you gain two hitpoints and one points of strength")
	player:PermanentlyUpdateCharacteristics(2,1,0,0)
end
