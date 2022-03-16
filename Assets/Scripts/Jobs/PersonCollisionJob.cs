using TestDOTS.Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Rendering;

namespace TestDOTS.Jobs
{
    public struct PersonCollisionJob : ITriggerEventsJob
    {
        [ReadOnly] public ComponentDataFromEntity<PersonTag> PersonGroup;
        public ComponentDataFromEntity<URPMaterialPropertyBaseColor> ColorGroup;
        public float TimeStamp;
        

        public void Execute(TriggerEvent triggerEvent)
        {
            var isPersonEntityA = PersonGroup.HasComponent(triggerEvent.EntityA);
            var isPersonEntityB = PersonGroup.HasComponent(triggerEvent.EntityB);

            if (!isPersonEntityA || !isPersonEntityB) return;

            var random = new Random((uint)((1 + TimeStamp) + triggerEvent.BodyIndexA * triggerEvent.BodyIndexB));

            ChangeMaterialColor(ref random, triggerEvent.EntityA);
            ChangeMaterialColor(ref random, triggerEvent.EntityB);
        }

        private void ChangeMaterialColor(ref Random random, Entity entity)
        {
            if (!ColorGroup.HasComponent(entity)) return;
            
            var colorComponent = ColorGroup[entity];
            colorComponent.Value = random.NextFloat4(0, 1);
            ColorGroup[entity] = colorComponent;
        }
    }
}