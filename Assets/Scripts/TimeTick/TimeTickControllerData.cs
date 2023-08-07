using Enums;

namespace TimeTick
{
    [System.Serializable]
    public struct TimeTickControllerData
    {
        public bool isAutomated;
        public float tickTimer;
        public float tickDuration;
        public TimeTickIdentifier timeIdentifier;
    }
}