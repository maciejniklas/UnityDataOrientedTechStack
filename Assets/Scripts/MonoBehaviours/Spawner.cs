using System;
using System.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TestDOTS.MonoBehaviours
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private AssetReference personPrefab;
        [SerializeField] private int gridSize;
        [SerializeField] private int spread;

        private BlobAssetStore _blob;

        private void Awake()
        {
            _blob = new BlobAssetStore();
        }

        private IEnumerator Start()
        {
            var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, _blob);
            var prefabLoadAsyncOperation = personPrefab.LoadAssetAsync<GameObject>();

            yield return new WaitUntil(() => prefabLoadAsyncOperation.IsDone);

            if (prefabLoadAsyncOperation.Status == AsyncOperationStatus.Succeeded)
            {
                var prefabGameObject = prefabLoadAsyncOperation.Result;

                var entity = GameObjectConversionUtility.ConvertGameObjectHierarchy(prefabGameObject, settings);
                
                var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                entityManager.Instantiate(entity);
            }
            else
            {
                Debug.LogError($"Error during prefab loading! (GUID: {personPrefab.AssetGUID})");
            }
        }

        private void OnDestroy()
        {
            _blob.Dispose();
        }
    }
}