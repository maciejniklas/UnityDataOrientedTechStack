using System;
using TestDOTS.Components;
using TestDOTS.Jobs;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Rendering;

namespace TestDOTS.Systems
{
    public class PersonCollisionSystem : SystemBase
    {
        private BuildPhysicsWorld _buildPhysicsWorld;
        private StepPhysicsWorld _stepPhysicsWorld;
        
        protected override void OnCreate()
        {
            _buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
            _stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
        }

        protected override void OnUpdate()
        {
            Dependency = new PersonCollisionJob
            {
                PersonGroup = GetComponentDataFromEntity<PersonTag>(true),
                ColorGroup = GetComponentDataFromEntity<URPMaterialPropertyBaseColor>(),
                TimeStamp = DateTimeOffset.Now.Millisecond
            }.Schedule(_stepPhysicsWorld.Simulation,
                ref _buildPhysicsWorld.PhysicsWorld, Dependency);
        }
    }
}