using GameFramework;
using System;

namespace Genesis.GameClient
{
    public static class InstanceLogicFactory
    {
        public static BaseInstanceLogic Create(InstanceLogicType logicType)
        {
            if (logicType == InstanceLogicType.NonInstance)
            {
                return new NonInstanceLogic();
            }

            if (logicType == InstanceLogicType.ForResource)
            {
                return new InstanceForResourceLogic();
            }

            Type type = Type.GetType(string.Format("Genesis.GameClient.{0}InstanceLogic", logicType.ToString()));

            if (type == null)
            {
                Log.Error("InstanceLogicFactory doesn't recognize the instance logic type '{0}'.", logicType);
                return null;
            }

            return Activator.CreateInstance(type) as BaseInstanceLogic;
        }
    }
}
