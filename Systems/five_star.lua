function init()
	AddObject(Mass_sun * 0.5, 0.1, 'Sun A', ColorFromName('Yellow'), Vector(-3 * Distance_au, 0, 0), Vector(0, -(2 * math.pi * Distance_au) / 3, 0))
	AddObject(Mass_sun * 0.5, 0.1, 'Sun B', ColorFromName('Red'), Vector(3 * Distance_au, 0, 0), Vector(0, (2 * math.pi * Distance_au) / 3, 0))
	AddObject(Mass_sun * 0.5, 0.1, 'Sun C', ColorFromName('Gold'), Vector(0, 3 * Distance_au, 0), Vector(-(2 * math.pi * Distance_au) / 3, 0, 0))
	AddObject(Mass_sun * 0.5, 0.1, 'Sun D', ColorFromName('OrangeRed'), Vector(0, -3 * Distance_au, 0), Vector((2 * math.pi * Distance_au) / 3, 0, 0))
	AddObject(Mass_sun * 0.5, 0.1, 'Sun E', ColorFromName('White'))
end