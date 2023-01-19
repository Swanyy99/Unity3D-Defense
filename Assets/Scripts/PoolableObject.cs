using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace ObjectPool
{
    public class PoolableObject : MonoBehaviour
    {
        [SerializeField]
        private float returnTime;

        public void Return()
        {
            PoolManager.Instance.Release(gameObject);
        }

        public IEnumerator DelayToReturn()
        {
            yield return new WaitForSeconds(returnTime);
            PoolManager.Instance.Release(gameObject);
        }

        private bool Type(string name)
        {
            return gameObject.tag.Equals(name);
        }
    }
}
