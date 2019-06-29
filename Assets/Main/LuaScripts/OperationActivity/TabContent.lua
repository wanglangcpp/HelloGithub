local CreateObj = require("LuaFoundation.Common").CreateObj

local TabContent = {
    New = function(self, o) return CreateObj(self, o) end,
    OnOpen = function(self) end,
    OnClose = function(self) end,
    OnClickButton = function(self, key) end,
    OnNetworkResponse = function(self, response) end,
}

return TabContent
