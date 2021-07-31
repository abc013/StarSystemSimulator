local sandbox = require('systems.sandbox')

init = function() end
tick = function() end

environment = { setInit = function(fn) init = fn end, setTick = function(fn) tick = fn end }

loadScript = function(content)
	sandbox.run(content, { env = environment })
end

registerGlobal = function(key, value)
	environment[key] = value
end