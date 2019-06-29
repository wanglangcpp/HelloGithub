using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Genesis.GameClient.Editor
{
    internal class AudioPostprocessor : AssetPostprocessor
    {
        private void OnPreprocessAudio()
        {
            var audioImporter = assetImporter as AudioImporter;

            if (assetPath.Contains("Assets/Main/Music"))
            {
                var settings = audioImporter.defaultSampleSettings;
                settings.loadType = AudioClipLoadType.Streaming;
                audioImporter.defaultSampleSettings = settings;
            }
        }
    }
}
