return {
    TAB_KEY_PREFIX = "Tab",

    ClearTabButtonGrid = function(form)
        local children = form.tabBtnGrid:GetChildList()

        if children.Count <= 0 then return end

        iter = children:GetEnumerator()
        while iter:MoveNext() do
            local trans = iter.Current
            if trans ~= nil then
                UnityEngine.GameObject.Destroy(trans.gameObject)
            end
        end
    end
}
