using UnityEditor;

namespace Genesis.GameClient.Editor
{
    public static class CodeGenerator
    {
        [MenuItem("Game Framework/Generate Code/Event", priority = 4000)]
        public static void GenerateEvent()
        {
            EditorWindow.GetWindow<EventGeneratorEditorWindow>(true).Reset();
        }

        [MenuItem("Game Framework/Generate Code/Data Row", priority = 4001)]
        public static void GenerateDataRow()
        {
            EditorWindow.GetWindow<DataRowGeneratorEditorWindow>(true).Reset();
        }

        //[MenuItem("Game Framework/Generate Code/Time Line Action", priority = 4002)]
        //public static void GenerateTimeLineAction()
        //{

        //}

        [MenuItem("Game Framework/Generate Code/Refresh Time Line Action Factory", priority = 4002)]
        public static void RefreshTimeLineActionFactory()
        {
            new TimeLineActionFactoryRefresher().Run();
        }
    }
}
