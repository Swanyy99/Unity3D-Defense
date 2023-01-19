using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class PoolManager : SingleTon<PoolManager>
{
    private Dictionary<string, Stack<GameObject>> poolDic;

    [SerializeField]
    private List<Poolable> poolPrefab;

    private void Awake()
    {
        poolDic= new Dictionary<string, Stack<GameObject>>();
    }

    private void Start()
    {
        CreatePool();
    }

    public void CreatePool()
    {
        for (int i = 0; i < poolPrefab.Count; i++)
        {
            Stack<GameObject> stack = new Stack<GameObject>();
            for (int j = 0; j < poolPrefab[i].count; j++)
            {
                GameObject instance = Instantiate(poolPrefab[i].prefab);
                instance.SetActive(false);
                instance.gameObject.name = poolPrefab[i].prefab.name;
                instance.transform.parent = poolPrefab[i].container;
                stack.Push(instance);
            }

            poolDic.Add(poolPrefab[i].prefab.name, stack);
        }
    }

    public GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        Stack<GameObject> stack = poolDic[prefab.name];
        if (stack.Count > 0)
        {
            GameObject instance = stack.Pop();
            instance.transform.parent = parent;
            instance.SetActive(true);
            instance.transform.position = position;
            instance.transform.rotation = rotation;
            return instance;
        }

        Poolable poolable = poolPrefab.Find((x) => prefab.name == x.prefab.name);
        if (poolable.Creation)
        {
            GameObject instance = Instantiate(prefab);
            instance.transform.parent = parent;
            instance.SetActive(true);
            instance.gameObject.name = prefab.name;
            instance.transform.position = position;
            instance.transform.rotation = rotation;
            return instance;
        }

        else
        {
            return null;
        }

    }

    public void Release(GameObject instance)
    {
        Stack<GameObject> stack = poolDic[instance.name];
        instance.SetActive(false);
        stack.Push(instance);

        Poolable poolabe = poolPrefab.Find((x) => instance.name == x.container.name);
        instance.transform.parent = poolabe.container;
    }

    [Serializable]
    public struct Poolable
    {
        public GameObject prefab;
        public int count;
        public Transform container;
        public bool Creation;
    }
}
