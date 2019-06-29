local StringDict = System.Collections.Generic.Dictionary_string_string

-- table
function table.toStringDict(tab)
    local ret = StringDict.New()
    for k, v in pairs(tab) do
        ret:Add(tostring(k), tostring(v))
    end
    return ret
end

function table.fromDict(dict)
    local ret = {}
    local iter = dict:GetEnumerator()
    while iter:MoveNext() do
        ret[iter.Current.Key] = iter.Current.Value
    end
    return ret
end
-- !table

-- Global
function printf(fmt, ...)
    print(string.format(fmt, ...))
end

function errorf(fmt, ...)
    error(string.format(fmt, ...))
end
-- !Global
