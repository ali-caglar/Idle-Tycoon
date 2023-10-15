using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Utility
{
    public interface IResetOnPlaymodeExit
    {
#if UNITY_EDITOR
        public void PlaymodeExitReset();
#endif
    }

#if UNITY_EDITOR
    [InitializeOnLoad]
    public static class AssetResetManager
    {
        static AssetResetManager()
        {
            EditorApplication.playModeStateChanged += HandleAssetReset;
        }

        private static void HandleAssetReset(PlayModeStateChange playModeStateChange)
        {
            if (playModeStateChange == PlayModeStateChange.ExitingPlayMode)
            {
                ResetAllIResetOnPlayModeExitAssets();
            }
        }

        private static void ResetAllIResetOnPlayModeExitAssets()
        {
            var resetAssets = Resources.FindObjectsOfTypeAll<Object>()
                .OfType<IResetOnPlaymodeExit>()
                .ToArray();

            if (resetAssets.Length == 0)
            {
                return;
            }

            foreach (var asset in resetAssets)
            {
                asset.PlaymodeExitReset();
            }

            Debug.Log($"Asset Reset Manager: Reset {resetAssets.Length} assets after exiting play mode.");
        }
    }
#endif
}