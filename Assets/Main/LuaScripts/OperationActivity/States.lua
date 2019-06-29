local DailyLoginTabContent = require "OperationActivity.DailyLoginTabContent"
local oaCommon = require("OperationActivity.OperationActivityCommon")
local TAB_KEY_PREFIX = oaCommon.TAB_KEY_PREFIX
local FsmModule = require("LuaFoundation.Fsm")
local FsmState = FsmModule.FsmState
local gameClient = Genesis.GameClient
local gameEntry = gameClient.GameEntry
local uiUtility = gameClient.UIUtility
local luaUtility = gameClient.LuaUtility

local ClearTabButtonGrid = require("OperationActivity.OperationActivityCommon").ClearTabButtonGrid

local TabContentClasses = {
    [1] = DailyLoginTabContent,
}

local StateBase = FsmState:New {
    GetForm = function(self)
        return self.fsm.owner
    end,

    OnInit = function(self, fsm)
        FsmState.OnInit(self, fsm)
        self.fsm = fsm
    end,

    OnTab = function(self, fsm, index, enabled)
        if not enabled then return end

        local form = self:GetForm()
        form.tabs[index].button.toggle:Set(false, false)
        if form.tabs[form.currentTabIndex] then form.tabs[form.currentTabIndex].button.toggle:Set(true, false) end
    end,

    OnClickButton = function(self, fsm, key) end,
    OnEvent_LoadAndInstantiateUIInstanceSuccess = function(self, eventArgs) end,
    OnEvent_LoadAndInstantiateUIInstanceFailure = function(self, eventArgs) end,
    OnNetworkResponse = function(self, response) end,
}

local CreateTabButtons = function(form)
    form.currentTabIndex = 1
    form.tabs = {}
    local prefab = form.csForm:GetLinkedPrefab("TabButton")

    for i = 1, #(form.drs) do
        local go = NGUITools.AddChild(form.tabBtnGrid.gameObject, prefab)
        local trans = go.transform
        local tab = {}
        local button = {
            SetNameLabel = function(self, name)
                self.disabledLabel.text = name
                self.normalLabel.text = name
            end
        }

        button.go = go
        button.trans = trans
        button.index = i
        local stringKey = go:GetComponent("Genesis.GameClient.UIStringKey")
        stringKey.Key = TAB_KEY_PREFIX .. i
        local toggle = go:GetComponent("UIToggle")
        uiUtility.RefreshToggleGroup(toggle, form.toggleGroupBaseValue)
        form.csForm:SetToggleCallback(toggle)
        button.toggle = toggle
        button.disabledLabel = trans:Find("Btn Dis/Btn Text"):GetComponent("UILabel")
        button.normalLabel = trans:Find("Btn/Btn Text"):GetComponent("UILabel")
        button.reminderIcon = trans:Find("Decal Light/Remind Icon"):GetComponent("UISprite")
        button.icon = trans:Find("Decal Light/Icon"):GetComponent("UISprite")
        button.reminderIcon.gameObject:SetActive(false)
        button.toggle:Set(i == form.currentTabIndex, false)
        button:SetNameLabel(gameEntry.Localization:GetString(form.drs[i].ActivityName, ""))
        luaUtility.LoadUISpriteAsyncByIconId(button.icon, form.drs[i].ActivityIconId)
        tab.button = button
        tab.root = button.trans
        tab.dr = form.drs[i]
        form.tabs[i] = tab
    end
end

local StateInitTabButtons = StateBase:New {
    frameCountForDestroy = 1, frameCountForInit = 2,

    OnEnter = function(self, fsm)
        StateBase.OnEnter(self, fsm)

        local dt = gameEntry.DataTableProxy.OperationActivity
        local drs = dt:GetAllDataRows():ToTable()
        table.sort(drs, function(x, y) return x.Id < y.Id end)

        local form = self:GetForm()
        ClearTabButtonGrid(form)

        form.drs = drs
        self.frameCount = 0
    end,

    OnUpdate = function(self, fsm, deltaTime, unscaledDeltaTime)
        self.frameCount = self.frameCount + 1
        local form = self:GetForm()

        if self.frameCount == self.frameCountForDestroy then
            CreateTabButtons(form)
        elseif self.frameCount == self.frameCountForInit then
            form.tabBtnGrid:Reposition()
            form.tabBtnScrollView:ResetPosition()
            self:ChangeState(fsm, "StateInitTabContent")
        end
    end,
}

local StateInitTabContent = StateBase:New {
    OnEnter = function(self, fsm)
        StateBase.OnEnter(self, fsm)
        -- print("[StateInitTabContent OnEnter]")
        self.shouldChangeToNormalState = false

        local form = self:GetForm()
        gameEntry.Waiting:StartWaiting(gameClient.WaitingType.Default, form.name)

        form.csForm:SubscribeEvent(gameClient.EventId.LoadAndInstantiateUIInstanceSuccess:ToInt());
        form.csForm:SubscribeEvent(gameClient.EventId.LoadAndInstantiateUIInstanceFailure:ToInt());

        local tab = form.tabs[form.currentTabIndex]

        self.prefabPath = tab.dr.ActivityUIPath
        if not self.prefabPath or self.prefabPath == "" then
            self.shouldChangeToNormalState = true
            return
        end

        gameEntry.UIFragment:LoadAndInstantiate(self.prefabPath, nil)
    end,

    OnLeave = function(self, fsm, isShutdown)
        self.prefabPath = nil
        local form = self:GetForm()
        gameEntry.Waiting:StopWaiting(gameClient.WaitingType.Default, form.name)
        form.csForm:UnsubscribeEvent(gameClient.EventId.LoadAndInstantiateUIInstanceSuccess:ToInt());
        form.csForm:UnsubscribeEvent(gameClient.EventId.LoadAndInstantiateUIInstanceFailure:ToInt());

        StateBase.OnLeave(self, fsm, isShutdown)
    end,

    OnUpdate = function(self, fsm, deltaTime, unscaledDeltaTime)
        StateBase.OnUpdate(self, fsm, deltaTime, unscaledDeltaTime)
        if self.shouldChangeToNormalState then self:ChangeState(fsm, "StateNormal") end
    end,

    OnEvent_LoadAndInstantiateUIInstanceSuccess = function(self, e)
        if not string.find(e.AssetName, self.prefabPath) then return end

        local go = e.GameObject
        if not go then
            error(string.format("Cannot find the game object from '%s'.", self.prefabPath))
            return
        end

        local trans = go.transform
        local cachedPos = trans.localPosition
        local cachedRot = trans.localRotation
        local cachedScale = trans.localScale

        local form = self:GetForm()
        trans:SetParent(form.tabContentParent)
        trans.localPosition = cachedPos
        trans.localRotation = cachedRot
        trans.localScale = cachedScale

        local tab = form.tabs[form.currentTabIndex]

        tab.contentGO = go
        local content = TabContentClasses[tab.dr.Id]:New { go = go, form = form, activityId = tab.dr.Id }
        -- content.descLabel = trans:Find(content.descLabelPath).gameObject:GetComponent("UILabel")
        -- content.descLabel.text = gameEntry.Localization:GetString(tab.dr.ActivityDesc, "")
        -- content.startEndTimeLabel = trans:Find(content.startEndTimeLabelPath).gameObject:GetComponent("UILabel")
        -- content.startEndTimeLabel.text = gameEntry.Localization:GetString("UI_TEXT_OPERATION_DATE", tab.dr.StartTime, tab.dr.EndTime)
        tab.content = content
        self.shouldChangeToNormalState = true
    end,

    OnEvent_LoadAndInstantiateUIInstanceFailure = function(self, e)
        if string.find(e.AssetName, self.prefabPath) then self.shouldChangeToNormalState = true end
    end,
}

local StateNormal = StateBase:New {
    OnEnter = function(self, fsm)
        StateBase.OnEnter(self, fsm)
        self.nextTabIndex = 1

        local form = self:GetForm()
        local tab = form.tabs[form.currentTabIndex]
        if not tab.content then return end
        local go = tab.contentGO
        go:SetActive(true)
        local panels = go:GetComponentsInChildren(System.Type.GetType("UIPanel"), true)
        local iter = panels:GetEnumerator()
        while iter:MoveNext() do
            local panel = iter.Current
            panel.depth = panel.depth + form.csForm.Depth - form.csForm.OriginalDepth
        end
        tab.content:OnOpen()
    end,

    OnLeave = function(self, fsm, isShutdown)
        local form = self:GetForm()
        local tab = form.tabs[form.currentTabIndex]
        if tab.content then
            tab.content:OnClose()

            local go = tab.contentGO
            local panels = go:GetComponentsInChildren(System.Type.GetType("UIPanel"), true)
            local iter = panels:GetEnumerator()
            while iter:MoveNext() do
                local panel = iter.Current
                panel.depth = panel.depth - form.csForm.Depth + form.csForm.OriginalDepth
            end
            go:SetActive(false)
        end

        form.currentTabIndex = self.nextTabIndex
        StateBase.OnLeave(self, fsm, isShutdown)
    end,

    OnTab = function(self, fsm, index, value)
        if not value then return end
        form = self:GetForm()
        if index == form.currentTabIndex then return end
        self.nextTabIndex = index
        self:ChangeState(fsm, "StateInitTabContent")
    end,

    OnClickButton = function(self, fsm, key)
        local form = self:GetForm()
        local tab = form.tabs[form.currentTabIndex]
        if tab.content then tab.content:OnClickButton(key) end
    end,

    OnNetworkResponse = function(self, response)
        local form = self:GetForm()
        local tab = form.tabs[form.currentTabIndex]
        if tab.content then tab.content:OnNetworkResponse(response) end
    end,
}

return {
    StateInitTabButtons = StateInitTabButtons,
    StateInitTabContent = StateInitTabContent,
    StateNormal = StateNormal
}