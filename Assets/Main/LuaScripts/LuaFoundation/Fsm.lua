local CreateObj = require("LuaFoundation.Common").CreateObj

local FsmState = {}

function FsmState:New(o)
    return CreateObj(self, o)
end

function FsmState:OnInit(fsm)
    -- Empty.
end

function FsmState:OnEnter(fsm)
    -- Empty.
end

function FsmState:OnLeave(fsm, isShutdown)
    -- Empty.
end

function FsmState:OnDestroy(fsm)
    -- Empty.
end

function FsmState:OnUpdate(fsm, deltaTime, unscaledDeltaTime)
    -- Empty
end

function FsmState:ChangeState(fsm, newStateKey)
    assert(newStateKey, "New state key is invalid.")

    assert(fsm.currentState ~= nil, "Current state is invalid.")
    fsm.currentState:OnLeave(fsm, false)

    local newState = fsm.states[newStateKey]
    assert(newState ~= nil, "New state is invalid.")
    fsm.currentState = newState
    fsm.newStateKey = newStateKey
    fsm.currentStateTime = 0
    newState:OnEnter(fsm)
end

local Fsm = {}

function Fsm:New(o)
    o = CreateObj(self, o)
    o.states = o.states or {}
    local states = o.states
    for k, v in pairs(states) do
        local state = states[k]
        state:OnInit(o)
    end

    o.currentState = nil
    o.currentStateKey = nil
    o.destroyed = false
    o.currentStateTime = 0
    return o
end

function Fsm:Start(stateKey)
    assert(type(stateKey) == "string", "State key is invalid.")
    assert(not self.destroyed, "Already destroyed.")
    assert(not self:IsRunning(), "Already running.")
    assert(not self.currentStateKey, "Current state key is not empty.")
    self.currentState = self.states[stateKey]    
    self.currentStateKey = stateKey
    self.currentStateTime = 0
    self.currentState:OnEnter(self)
end

function Fsm:Update(deltaTime, unscaledDeltaTime)
    --assert(self:IsRunning(), "Not running. Cannot update.")
    self.currentStateTime = self.currentStateTime + deltaTime
    self.currentState:OnUpdate(self, deltaTime, unscaledDeltaTime)
end

function Fsm:Shutdown()
    assert(not self.destroyed, "Already destroyed.")
    assert(self:IsRunning(), "Not running.")
    self.currentState:OnLeave(self, true)

    self.currentState = nil
    self.currentStateKey = nil
    self.currentStateTime = 0

    for k, v in pairs(self.states) do
        local state = self.states[k]
        state:OnDestroy(self)
    end

    self.destroyed = true
end

function Fsm:IsRunning()
    return self.currentState ~= nil
end

return { FsmState = FsmState, Fsm = Fsm }
