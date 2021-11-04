using TestDOTS.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TestDOTS.MonoBehaviours
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private GameObject personPrefab;
        [SerializeField] [Min(10)] private int gridSize = 100;
        [SerializeField] [Min(1)] private int spread = 5;
        [SerializeField] private Vector2 speedRange = new Vector2(5, 7.5f);
        
        private BlobAssetStore _blob;

        private void Awake()
        {
            _blob = new BlobAssetStore();
        }

        private void Start()
        {
            var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, _blob);
            var entity = GameObjectConversionUtility.ConvertGameObjectHierarchy(personPrefab, settings);
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                
            for (var xIndex = 0; xIndex < gridSize; xIndex++)
            {
                for (var zIndex = 0; zIndex < gridSize; zIndex++)
                {
                    var instance = entityManager.Instantiate(entity);
                    var position = new float3(xIndex, 0, zIndex) * spread;
                    var speed = Random.Range(speedRange.x, speedRange.y);

                    entityManager.SetComponentData(instance, new Translation { Value = position });
                    entityManager.SetComponentData(instance, new MovementSpeed { Value = speed });
                    entityManager.SetComponentData(instance, new Destination { Value = position });
                }
            }
        }

        private void OnDestroy()
        {
            _blob.Dispose();
        }
    }
}