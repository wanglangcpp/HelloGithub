using GameFramework;

namespace Genesis.GameClient
{
    public class EntityTimeLineEventUserData
    {
        public GameFrameworkAction<object> OnBreakTimeLineSuccess;

        public GameFrameworkAction<object> OnBreakTimeLineFailure;

        public object UserData;

        private static EntityTimeLineEventUserData s_DefaultInstance;

        public static EntityTimeLineEventUserData DefaultInstance
        {
            get
            {
                if (s_DefaultInstance == null)
                {
                    s_DefaultInstance = new EntityTimeLineEventUserData();
                }

                return s_DefaultInstance;
            }
        }
    }
}
