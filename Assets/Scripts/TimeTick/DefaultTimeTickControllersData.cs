using System.Collections.Generic;
using UnityEngine;

namespace TimeTick
{
    [CreateAssetMenu(fileName = "Default Time Tick Data", menuName = "Data/Time Tick/Default Data", order = 0)]
    public class DefaultTimeTickControllersData : ScriptableObject
    {
        [SerializeField] private List<TimeTickControllerData> defaultControllers;

        public List<TimeTickControllerData> DefaultControllers => defaultControllers;
    }
}