using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Utility
{
    public static class Logger
    {
        private static bool _showLogsInEditor = true;
        private static bool _forceShowLogs;

        #region LIFECYCLE

        static Logger()
        {
#if !UNITY_EDITOR
            _showLogsInEditor = false;
#endif
        }

        #endregion

        #region PUBLIC METHODS

        public static void Log(LogType logType, string message, Object sender = null)
        {
            if (!_showLogsInEditor && !_forceShowLogs) return;
            switch (logType)
            {
                case LogType.Error:
                    Debug.LogError(message, sender);
                    break;
                case LogType.Assert:
                    break;
                case LogType.Warning:
                    Debug.LogWarning(message, sender);
                    break;
                case LogType.Log:
                    Debug.Log(message, sender);
                    break;
                case LogType.Exception:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logType), logType, "Log type not implemented.");
            }
        }

        #endregion
    }
}