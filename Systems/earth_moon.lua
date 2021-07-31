function init()
	AddObject(Mass_earth, 0.002, 'Earth', ColorFromName('BlueViolet'))
	AddObject(Mass_moon, 0.001, 'Moon', ColorFromName('DarkGray'), Vector(0, Distance_earth_radius + Distance_moon_to_earth, 0), Vector((365 / 29) * 2 * math.pi * (Distance_earth_radius + Distance_moon_to_earth), 0, 0))
end