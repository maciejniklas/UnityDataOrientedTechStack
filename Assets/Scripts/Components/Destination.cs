using Unity.Entities;
using Unity.Mathematics;

namespace TestDOTS.Components
{
    [GenerateAuthoringComponent]
    public struct Destination : IComponentData
    {
        public float3 Value;
    }
}