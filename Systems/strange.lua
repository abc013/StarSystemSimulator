function init()
	AddObject(Mass_sun, 0.04, 'Sun A', ColorFromName('Yellow'), Vector(-Distance_au, 0, 0), Vector(0, -2 * math.pi * Distance_au, 0))
	AddObject(Mass_sun * 1.4, 0.05, 'Sun B', ColorFromName('Red'), Vector(Distance_au, 0, 0), Vector(0, 2 * math.pi * Distance_au, 0))
	AddObject(Mass_earth, 0.02, 'Planet A', ColorFromName('BlueViolet'), Vector(Distance_au, 0, 0), Vector(0, 2 * math.pi * Distance_au, 0))
	AddObject(Mass_earth * 0.1, 0.005, 'Planet B', ColorFromName('CornflowerBlue'), Vector(Distance_au, 2 * Distance_au, 0), Vector(Distance_au * math.pi, 0, 0))
	AddObject(Mass_earth, 0.02, 'Planet B', ColorFromName('BlueViolet'), Vector(0, 0, 0), Vector(0.3 * Distance_au * math.pi, 0, 0))
	AddObject(Mass_moon, 0.005, 'Moon', ColorFromName('DarkGray'), Vector(0, Distance_earth_radius + Distance_moon_to_earth, 0), Vector(2 * math.pi * math.pi * (Distance_earth_radius + Distance_moon_to_earth), 0, 0))
end