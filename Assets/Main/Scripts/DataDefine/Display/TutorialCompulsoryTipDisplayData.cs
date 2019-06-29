using System;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class TutorialCompulsoryTipDisplayData : TutorialNormalTipDisplayData
    {
        public bool MaskVisible = true;
        public bool PauseGame = true;
        public string[] WidgetPathsToInteract;
        public bool HideOnResume = true;
    }
}
