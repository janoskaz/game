message = "null"
out = block
keepscript = false

if (not config.explored_sarcophagus) then
	message = "You already feel the dry desert air from outside and rush towards the light in front of you, but your inner voice tells you: \"Isn't there maybe something interesting still inside?\""
	keepscript = true
end