local TabContent = require("OperationActivity.TabContent")
local gameClient = Genesis.GameClient
local gameEntry = gameClient.GameEntry
local lobbyLogic = gameEntry.LobbyLogic
local luaUtility = gameClient.LuaUtility
local generalItemUtility = gameClient.GeneralItemUtility

local EFFECT_KEY_PREFIX = "Day"
local CLAIM_BUTTON_KEY = "ClaimButton"

-- This tab will be used for monthly log in now, and the logic will be implemeneted via C#. The code is only commented for future reference.

local DailyLoginTabContent = TabContent:New {
    -- descLabelPath = "DailyLogin/Description Text Bg/Description Text",
    -- startEndTimeLabelPath = "DailyLogin/Description Text Bg/Description Text",
    -- localDayCount = 7,

    OnOpen = function(self)
        -- self.items = {}
        -- self.rootTrans = self.go.transform
        -- self.effectsController = self.go:GetComponent("UIEffectsController")
        -- self.effectsController:Resume()

        -- for i = 1, self.localDayCount do
        --     local item = {}
        --     if i < self.localDayCount then
        --         item.rootTrans = self.rootTrans:Find(string.format("DailyLogin/Table/Day %d Item", i - 1))
        --     else
        --         item.rootTrans = self.rootTrans:Find("DailyLogin/Day 6 Item")
        --     end

        --     item.icon = item.rootTrans:Find("Icon"):GetComponent("UISprite")
        --     item.activeStateObj = item.rootTrans:Find("Btn").gameObject
        --     item.titleLbl = item.rootTrans:Find("Btn/Title Name Text"):GetComponent("UILabel")
        --     item.titleLblDisabled = item.rootTrans:Find("Btn Dis/Title Name Text"):GetComponent("UILabel")
        --     item.nameLbl = item.rootTrans:Find("Btn/Item Name Text"):GetComponent("UILabel")
        --     item.nameLblDisabled = item.rootTrans:Find("Btn Dis/Item Name Text"):GetComponent("UILabel")
        --     item.goodsView = item.rootTrans:Find("Icon"):GetComponent("GoodsView")
        --     item.claimedMark = item.rootTrans:Find("Icon/Icon Already"):GetComponent("UISprite")

        --     self.items[i] = item
        -- end

        -- self.claimingBtn = self.rootTrans:Find("DailyLogin/Receive Btn"):GetComponent("UIButton")
        -- self.claimingBtn.transform:Find("Btn Text"):GetComponent("UILabel").text = gameEntry.Localization:GetString("UI_BUTTON_RECEIVEREWARDS", "")
        -- self.form.csForm:SetButtonCallback(self.claimingBtn)

        -- lobbyLogic:OperationActivityRequest(table.toStringDict({["AId"] = self.activityId, ["Op"] = "GetData"}))
    end,

    OnClose = function(self)
        -- self:ClearViewItems()
        -- self.effectsController:Pause()
        -- self.claimingBtn.onClick:Clear()
    end,

    OnClickButton = function(self, key)
        -- if key ~= CLAIM_BUTTON_KEY then return end
        -- lobbyLogic:OperationActivityRequest(table.toStringDict({["AId"] = self.activityId, ["Op"] = "ClaimReward"}))
    end,

    OnNetworkResponse = function(self, response)
        -- if tonumber(response["AId"]) ~= self.activityId then return end

        -- print("[DailyLoginTabContent OnNetworkResponse]")

        -- if response["Op"] == "GetData" then
        --     self.hasClaimed = response["HasClaimed"]:toBoolean()
        --     self.claimedCount = tonumber(response["ClaimedCnt"])
        --     self.dayCount = tonumber(response["DayCnt"])
        --     self.rewards = {}
        --     for i = 1, self.dayCount do
        --         self.rewards[i] = {
        --             itemId = tonumber(response[string.format("Reward.%d.ItemId", i - 1)]),
        --             itemCount = tonumber(response[string.format("Reward.%d.ItemCnt", i - 1)]),
        --         }
        --     end
        -- elseif response["Op"] == "ClaimReward" then
        --     self.hasClaimed = response["HasClaimed"]:toBoolean()
        --     self.claimedCount = tonumber(response["ClaimedCnt"])
        -- end

        -- self.claimingBtn.isEnabled = not self.hasClaimed
        -- for i = 1, #(self.items) do
        --     local item = self.items[i]
        --     if i <= self.dayCount then
        --         local itemId = self.rewards[i].itemId
        --         luaUtility.LoadUISpriteAsyncByIconId(item.icon, generalItemUtility.GetGeneralItemIconId(itemId))
        --         item.titleLbl.text = gameEntry.Localization:GetString("UI_TEXT_OPERATION_DAY_NUMBER", i)
        --         item.titleLblDisabled.text = item.titleLbl.text
        --         local itemNameKey = generalItemUtility.GetGeneralItemName(itemId)

        --         if not string.nullOrEmpty(itemNameKey) then
        --             local itemName = gameEntry.Localization:GetString(itemNameKey, "")
        --             item.nameLbl.text = itemName
        --             item.nameLblDisabled.text = itemName
        --         end

        --         local itemCount = self.rewards[i].itemCount
        --         item.goodsView:InitGoodsView(itemId, itemCount)

        --         item.claimedMark.gameObject:SetActive(i <= self.claimedCount or i == self.claimedCount + 1 and self.hasClaimed)

        --         local effectKey = EFFECT_KEY_PREFIX .. tostring(i - 1)
        --         self.effectsController:DestroyEffect(effectKey)

        --         if i == self.claimedCount + 1 and not self.hasClaimed then
        --             self.effectsController:ShowEffect(effectKey)
        --         end
        --     else
        --         self.ClearViewItem(i)
        --     end
        -- end
    end,

    -- ClearViewItem = function(self, i)
    --     local item = self.items[i]
    --     item.titleLbl.text = ""
    --     item.titleLblDisabled.text = ""
    --     item.nameLbl.text = ""
    --     item.nameLblDisabled.text = ""
    --     self.effectsController:DestroyEffect(EFFECT_KEY_PREFIX .. tostring(i - 1))
    -- end,

    -- ClearViewItems = function(self)
    --     if not self.items then return end

    --     for i = 1, #(self.items) do
    --         self:ClearViewItem(i)
    --     end
    -- end,
}

return DailyLoginTabContent
