using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace ObjectPool
{
    public class ObjectPooler : MonoBehaviour
    {
        private Stack<PooledObject> pool;

        [SerializeField]
        private PooledObject pooledObjectPrefab;

        

        [SerializeField]
        private bool enableCreation;


        [SerializeField]
        private int count;

        private GameObject player;

        private void Start()
        {
            player = GameObject.Find("Player");
            pool = new Stack<PooledObject>();
            CreatePool(pooledObjectPrefab, count);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Get(new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f)), Quaternion.identity);
                Get(player.transform.position, player.transform.rotation);
            }
        }

        private void CreatePool(PooledObject pooledObject, int count)
        {
            for (int i = 0; i < count; i++)
            {
                PooledObject instance = Instantiate(pooledObjectPrefab);
                instance.gameObject.SetActive(false);
                instance.transform.parent = transform;
                instance.returnPool = this;
                pool.Push(instance);
            }
        }

        private PooledObject Get(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            if (pool.Count > 0)
            {
                PooledObject instance = pool.Pop();
                instance.gameObject.SetActive(true);
                instance.transform.position = position;
                instance.transform.rotation = rotation;
                instance.transform.parent = parent;
                return instance;
            }
            else if (enableCreation)
            {
                PooledObject createdInstance = Instantiate(pooledObjectPrefab);
                createdInstance.gameObject.SetActive(true);
                createdInstance.transform.position = position;
                createdInstance.transform.rotation = rotation;
                createdInstance.transform.parent = parent;
                createdInstance.returnPool = this;
                return createdInstance;
            }
            else
            {
                return null;
            }
        }

        public void ReturnToPool(PooledObject pooledObject)
        {
            pooledObject.gameObject.SetActive(false);
            pooledObject.transform.parent = transform;
            pool.Push(pooledObject);
        }
    }
}