using TestDOTS.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace TestDOTS.Systems
{
    public class MoveToDestinationSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;

            Entities.ForEach((ref Translation translation, ref Rotation rotation, in Destination destination,
                in MovementSpeed speed) =>
            {
                if (math.all(translation.Value == destination.Value)) return;
                
                var distanceToDestination = destination.Value - translation.Value;
                var direction = math.normalize(distanceToDestination);

                rotation.Value = quaternion.LookRotation(direction, new float3(0, 1, 0));

                var displacement = direction * speed.Value * deltaTime;

                if (math.lengthsq(displacement) >= math.lengthsq(distanceToDestination))
                {
                    translation.Value = destination.Value;
                }
                else
                {
                    translation.Value += displacement;
                }
            }).ScheduleParallel();
        }
    }
}