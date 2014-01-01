---Interaction with broken wall

-- beetles
ch = Characteristics(5,1,2,0)
beetles = Being("Sacred beetles", ch, ch, 10)

-- only if the toch is lit
if (torch) then
	tell_message("In the light of the torch you can see the shity work you have done." 
	.. " If you push hard, you could move some of the stone block.")
	tell_message("You push and the wall breaks down, crumbling on the floor."
	.. " In horror, you watch dozens of huge sacred carnivore beetles rushing into the room, willing to eat you.")
	val = player:Fight(beetles)
	keepscript = false
	if val then
		tell_message("You squish the last beetle with your bare hand and exhale a breath full of relief. Killing sacred beetles is moraly problematic thing, you gain some cynicism, and mean people live longer, so you get also two more hitpoints.")
		player:PermanentlyUpdateCharacteristics(2,0,0,0)
		out = BasicObject()
	else
		out = beetles
	end
else
	message = "You can't go there - there's a wall!"
	keepscript = true
	out = Wall()
end