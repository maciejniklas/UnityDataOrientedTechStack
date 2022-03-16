using TestDOTS.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace TestDOTS.Systems
{
    public class SetRandomDestinationSystem : SystemBase
    {
        private RandomSystem _randomSystem;

        protected override void OnCreate()
        {
            _randomSystem = World.GetExistingSystem<RandomSystem>();
        }

        protected override void OnUpdate()
        {
            var randomArray = _randomSystem.RandomArray;

            Entities.WithNativeDisableParallelForRestriction(randomArray).ForEach(
                (int nativeThreadIndex, ref Destination destination, in Translation translation) =>
                {
                    var distance = math.abs(math.length(destination.Value - translation.Value));

                    if (distance < float.Epsilon)
                    {
                        var random = randomArray[nativeThreadIndex];
                        
                        destination.Value.x = random.NextFloat(0, 500);
                        destination.Value.z = random.NextFloat(0, 500);

                        randomArray[nativeThreadIndex] = random;
                    }
                }).ScheduleParallel();
        }
    }
}