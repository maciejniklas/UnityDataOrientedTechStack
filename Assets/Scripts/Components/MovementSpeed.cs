using Unity.Entities;

namespace TestDOTS.Components
{
    [GenerateAuthoringComponent]
    public struct MovementSpeed : IComponentData
    {
        public float Value;
    }
}