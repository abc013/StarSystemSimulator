function init()
	AddObject(Mass_sun * 1.4, 0.5, 'Distractor', ColorFromName('WhiteSmoke'), Vector(75 * Distance_au, -50 * Distance_au, 0), Vector(-4 * Distance_au, 3 * Distance_au, 0))
	AddObject(Mass_sun, 0.4, 'Sun', ColorFromName('Yellow'))

	local mass = 0.005 * Mass_moon
	local fullAngle = 2 * math.pi

	for i = 0, 300, 1
	do
		local angle = math.random() * fullAngle

		local dist = Distance_jupiter + (math.random() - 0.5) * Distance_au
		local x = math.sin(angle) * dist
		local y = math.cos(angle) * dist
		local z = (math.random() - 0.5) * 0.2 * dist

		local revolution = (fullAngle * dist) / (Time_jupiter_revolution * dist / Distance_jupiter)
		local xVel = math.sin(angle + fullAngle / 4) * revolution
		local yVel = math.cos(angle + fullAngle / 4) * revolution

		AddObject(mass, math.random() * 0.03 + 0.02, 'Rock ' .. i .. ', orbit A', ColorFromName('DarkGray'), Vector(x, y, z), Vector(xVel, yVel, 0))
	end

	for i = 0, 300, 1
	do
		local angle = math.random() * fullAngle

		local dist = Distance_jupiter + (math.random() + 0.5) * Distance_au
		local x = (math.random() - 0.5) * 0.2 * dist
		local y = math.sin(angle) * dist
		local z = math.cos(angle) * dist

		local revolution = (fullAngle * dist) / (Time_jupiter_revolution * dist / Distance_jupiter)
		local yVel = math.sin(angle + fullAngle / 4) * revolution
		local zVel = math.cos(angle + fullAngle / 4) * revolution

		AddObject(mass, math.random() * 0.03 + 0.02, 'Rock ' .. i .. ', orbit B', ColorFromName('DarkGray'), Vector(x, y, z), Vector(0, yVel, zVel))
	end

	SetViewZoom(-16)
end