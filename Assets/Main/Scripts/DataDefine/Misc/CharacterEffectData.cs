using UnityEngine;
using System.Collections.Generic;
using GameFramework;

namespace Genesis.GameClient
{
    /// <summary>
    /// 角色特效数据。
    /// </summary>
    public class CharacterEffectData
    {
        public IList<KeyValuePair<string, string>> ModelShowDebut = new List<KeyValuePair<string, string>>();

        public IList<IList<KeyValuePair<string, string>>> ModelShowInteractions = new List<IList<KeyValuePair<string, string>>>();

        public IList<KeyValuePair<string, string>> ReceiveModelShowDebut = new List<KeyValuePair<string, string>>();

        public CharacterEffectData()
        {
            for (int i = 0; i < Constant.CharacterForShowInteractionCount; ++i)
            {
                ModelShowInteractions.Add(new List<KeyValuePair<string, string>>());
            }
        }

        public IList<KeyValuePair<string, string>> GetSenarioDataByAnimationAlias(string animationAlias)
        {
            switch (animationAlias)
            {
                case "ModelShowDebut":
                    return ModelShowDebut;
                case "ModelInteraction1":
                    return ModelShowInteractions[0];
                case "ModelInteraction2":
                    return ModelShowInteractions[1];
                case "ModelInteraction3":
                    return ModelShowInteractions[2];
                case "ReceiveModelShowDebut":
                    return ReceiveModelShowDebut;
                default:
                    Log.Warning("Animation alias '{0}' is not available in CharacterEffectData.", animationAlias);
                    return new List<KeyValuePair<string, string>>();
            }
        }
    }
}
