setInit(function()
	AddObject(Mass_sun * 0.5, 0.1, 'Sun A', ColorFromName('Red'), Vector(-Distance_au, 0, 0), Vector(0, -2 * Distance_au, 0))
	AddObject(Mass_earth, 0.04, 'Planet', ColorFromName('CornflowerBlue'), Vector(0, 4 *Distance_au, 0), Vector(1.5 * math.pi * Distance_au, 0, 0))
end)