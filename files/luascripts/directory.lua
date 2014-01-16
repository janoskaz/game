Console.WriteLine("Where will you go?")
Console.WriteLine("\t1:Go to pub")
Console.WriteLine("\t2:Go to temple district")
Console.WriteLine("\t3:Go to desert")
get_answer(3)

if (answer=='3') then
	ThisGame.LoadNextMap("desert")
end
