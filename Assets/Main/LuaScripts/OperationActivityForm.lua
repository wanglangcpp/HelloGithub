if OperationActivityForm ~= nil then return OperationActivityForm:New() end

local CreateObj = require("LuaFoundation.Common").CreateObj
local FsmModule = require("LuaFoundation.Fsm")
local Fsm = FsmModule.Fsm
local Time = require "UnityEngine.Time"
require "Ext"

local oaCommon = require("OperationActivity.OperationActivityCommon")
local TAB_KEY_PREFIX = oaCommon.TAB_KEY_PREFIX
local ClearTabButtonGrid = oaCommon.ClearTabButtonGrid
local states = require("OperationActivity.States")
local StateInitTabButtons = states.StateInitTabButtons
local StateInitTabContent = states.StateInitTabContent
local StateNormal = states.StateNormal

OperationActivityForm = {
    name = "OperationActivityForm",

    New = function(self, o)
        return CreateObj(self, o)
    end,

    OnOpen = function(self, args)
        self.displayData = args[0]
        self.rootGO = self.displayData.RootGO
        self.rootTransform = self.rootGO.transform
        self.csForm = self.displayData.Form
        self.toggleGroupBaseValue = self.displayData.ToggleGroupBaseValue
        self.tabBtnScrollView = self.rootTransform:Find("ActivityOperation/Bg/Page Btn List Bg/Page Btn List Scroll View").gameObject:GetComponent("UIScrollView")
        self.tabBtnGrid = self.tabBtnScrollView.gameObject.transform:Find("Page Btn List Grid").gameObject:GetComponent("UIGrid")
        self.tabContentParent = self.rootTransform:Find("ActivityOperation/Bg")

        self.fsm = Fsm:New { owner = self,
            states = {
                ["StateInitTabButtons"] = StateInitTabButtons:New(),
                ["StateInitTabContent"] = StateInitTabContent:New(),
                ["StateNormal"] = StateNormal:New(),
            }
        }

        self.fsm:Start("StateInitTabButtons")
    end,

    OnResume = function(self)
    end,

    Update = function(self)
        self.fsm:Update(Time.deltaTime, Time.unscaledDeltaTime)
    end,

    OnPause = function(self)
    end,

    OnClose = function(self)
        if self.fsm ~= nil then
            self.fsm:Shutdown()
        end

        ClearTabButtonGrid(self)

        self.displayData = nil
        print("[OperationActivityForm OnClose]")
    end,

    OnToggle = function(self, args)
        local go = args[0]
        local key = args[1]
        local toggleValue = args[2]

        if string.startsWith(key, TAB_KEY_PREFIX) then
            local subStrStart = string.len(TAB_KEY_PREFIX) + 1
            local indexStr = string.sub(key, subStrStart, subStrStart)
            local index = tonumber(indexStr)
            self.fsm.currentState:OnTab(self.fsm, index, toggleValue)
        end
    end,

    OnClickButton = function(self, args)
        local go = args[0]
        local key = args[1]
        self.fsm.currentState:OnClickButton(self.fsm, key)
    end,

    OnNetworkResponse = function(self, args)
        local responseDict = args[0]
        self.fsm.currentState:OnNetworkResponse(table.fromDict(responseDict))
    end,

    OnEvent_LoadAndInstantiateUIInstanceSuccess = function(self, args)
        local e = args[0]
        self.fsm.currentState:OnEvent_LoadAndInstantiateUIInstanceSuccess(e)
    end,

    OnEvent_LoadAndInstantiateUIInstanceFailure = function(self, args)
        local e = args[0]
        self.fsm.currentState:OnEvent_LoadAndInstantiateUIInstanceFailure(e)
    end,
}

return OperationActivityForm:New()
