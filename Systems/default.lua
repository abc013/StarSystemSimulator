setInit(function()
	AddObject(Mass_sun, 0.3, 'Sun', ColorFromRGB(1, 1, 1))
	
	AddObject(Mass_earth, 0.02, 'Earth', ColorFromName('BlueViolet'), Vector(Distance_au, 0, 0), Vector(0, Distance_au * math.pi * 2, 0))
	AddObject(Mass_earth, 0.001, 'Moon', ColorFromName('DarkGray'), Vector(Distance_au, Distance_earth_radius + Distance_moon_to_earth, 0), Vector((365 / 29) * 2 * math.pi * (Distance_earth_radius + Distance_moon_to_earth), Distance_au * math.pi * 2, 0))
	AddObject(Mass_mars, 0.01, 'Mars', ColorFromName('IndianRed'), Vector(Distance_mars, 0, 0), Vector(0, Distance_mars * math.pi * 2 / Time_mars_revolution, 0))
	AddObject(Mass_jupiter, 0.1, 'Jupiter', ColorFromName('Green'), Vector(Distance_jupiter, 0, 0), Vector(0, Distance_jupiter * math.pi * 2 / Time_jupiter_revolution, 0))
	AddObject(Mass_saturn, 0.07, 'Saturn', ColorFromName('Brown'), Vector(Distance_saturn, 0, 0), Vector(0, Distance_saturn * math.pi * 2 / Time_saturn_revolution, 0))
	
	SetViewZoom(-16)
end)

setTick(function()
	--RotateView(0.02, 0.04)
end)