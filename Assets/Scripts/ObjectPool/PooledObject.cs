using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPool
{
    public class PooledObject : MonoBehaviour
    {
        [SerializeField]
        private float returnTime;

        public ObjectPooler returnPool { get; set; }

        private void OnEnable()
        {
            //StartCoroutine(DelayToReturn());
            //if (Type("Item")
        }


        private IEnumerator DelayToReturn()
        {
            yield return new WaitForSeconds(returnTime);
            returnPool.ReturnToPool(this);
        }

        public void Return()
        {
            returnPool.ReturnToPool(this);
        }


        private bool Type(string name)
        {
            return gameObject.tag.Equals(name);
        }
    }
}