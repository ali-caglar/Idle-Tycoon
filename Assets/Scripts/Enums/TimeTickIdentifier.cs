using UnityEngine;

namespace Enums
{
    public enum TimeTickIdentifier
    {
        [InspectorName("Custom")] Custom,
        [InspectorName("1 Second")] Second1,
        [InspectorName("2 Second")] Second2,
        [InspectorName("3 Second")] Second3,
        [InspectorName("5 Second")] Second5,
        [InspectorName("10 Second")] Second10,
    }
}