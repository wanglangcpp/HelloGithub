-- string
function string.startsWith(str, start)
    return string.sub(str, 1, string.len(start)) == start
end

function string.endsWith(str, ending)
    return ending == '' or string.sub(str,-string.len(ending)) == ending
end

function string.nullOrEmpty(str)
    return not str or string.len(str) <= 0
end

function string.toBoolean(str)
    return str ~= nil and str:lower() == "true"
end
-- !string

-- Global
function printf(fmt, ...)
    print(string.format(fmt, ...))
end

function errorf(fmt, ...)
    error(string.format(fmt, ...))
end
-- !Global

local CreateObj = function(class, obj)
    obj = obj or {}
    setmetatable(obj, class)
    class.__index = class
    return obj
end

return {
    CreateObj = CreateObj,
}
