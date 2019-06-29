using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// <see cref="OpenFunctionDialog"/> 显示数据。
    /// </summary>
    public class OpenFunctionsData : UIFormBaseUserData
    {
        public NGUIForm lobby;
        public List<DROpenFunction> itemList;
    }
}
