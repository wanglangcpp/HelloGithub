using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Genesis.GameClient.Editor
{
    public class TimeLineActionFactoryRefresher
    {
        private const string FileAssetPath = "Main/Scripts/TimeLine/Entity/EntityTimeLineActionFactory.cs";
        private const string ClassTemplateFileAssetPath = "Main/Editor/EntityTimeLineActionFactoryTemplate.txt";
        private static readonly string FilePath = Utility.Path.GetCombinePath(Application.dataPath, FileAssetPath);
        private static readonly string ClassTemplateFilePath = Utility.Path.GetCombinePath(Application.dataPath, ClassTemplateFileAssetPath);

        private const string DictItemFormat = "{0}{{ \"{1}\", {2} }},";
        private const string CreateActionMethodNameFormat = "Create{0}";
        private const string CreateDataMethodNameFormat = "Create{0}Data";

        private int indentCount = 0;

        private string Indentation
        {
            get
            {
                return new string(' ', indentCount * 4);
            }
        }

        private const string ActionCreatorMethodTemplate =
@"{0}private static TimeLineAction<Entity> {1}(TimeLineActionData timeLineActionData)
{0}{{
{0}    return new {2}(timeLineActionData);
{0}}}";

        private const string DataCreatorMethodTemplate =
@"{0}private static TimeLineActionData {1}()
{0}{{
{0}    return new {2}Data();
{0}}}";

        public void Run()
        {
            var actionCreatorDictItemsSB = new StringBuilder();
            var actionCreatorsSB = new StringBuilder();
            var dataCreatorDictItemsSB = new StringBuilder();
            var dataCreatorsSB = new StringBuilder();

            var actionClasses = Assembly.Load("Assembly-CSharp").GetTypes().Where(t => !t.IsAbstract && t.IsClass && t.Name.StartsWith("Entity") && t.IsSubclassOf(typeof(TimeLineAction<Entity>))).ToList();
            actionClasses.Sort((x, y) => { return x.Name.CompareTo(y.Name); });

            int index = 0;
            foreach (var actionClass in actionClasses)
            {
                var actionClassName = actionClass.Name;
                var actionCreatorMethodName = string.Format(CreateActionMethodNameFormat, actionClassName);
                var dataCreatorMethodName = string.Format(CreateDataMethodNameFormat, actionClassName);

                indentCount = 3;
                actionCreatorDictItemsSB.AppendFormat(DictItemFormat, Indentation, actionClassName, actionCreatorMethodName);
                dataCreatorDictItemsSB.AppendFormat(DictItemFormat, Indentation, actionClassName, dataCreatorMethodName);

                indentCount = 2;
                actionCreatorsSB.AppendFormat(ActionCreatorMethodTemplate, Indentation, actionCreatorMethodName, actionClassName);
                dataCreatorsSB.AppendFormat(DataCreatorMethodTemplate, Indentation, dataCreatorMethodName, actionClassName);

                if (index < actionClasses.Count - 1)
                {
                    actionCreatorDictItemsSB.AppendLine();
                    dataCreatorDictItemsSB.AppendLine();
                    actionCreatorsSB.AppendLine();
                    dataCreatorsSB.AppendLine();
                    actionCreatorsSB.AppendLine();
                    dataCreatorsSB.AppendLine();
                }

                ++index;
            }

            var template = File.ReadAllText(ClassTemplateFilePath, Encoding.UTF8);
            var newFileContent = template.Replace("$ActionCreatorsDict", actionCreatorDictItemsSB.ToString())
                .Replace("$DataCreatorsDict", dataCreatorDictItemsSB.ToString())
                .Replace("$ActionCreators", actionCreatorsSB.ToString())
                .Replace("$DataCreators", dataCreatorsSB.ToString());
            newFileContent = Regex.Replace(newFileContent, @"(?<!\r)\n", "\r\n");
            File.WriteAllText(FilePath, newFileContent, Encoding.UTF8);
            AssetDatabase.ImportAsset(Utility.Path.GetCombinePath("Assets", FileAssetPath));
        }
    }
}
