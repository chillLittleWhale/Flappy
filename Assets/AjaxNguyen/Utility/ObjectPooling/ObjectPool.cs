using System.Collections.Generic;
using UnityEngine;

namespace AjaxNguyen.Utility.ObjectPooling
{
    public class ObjectPool
    {
        private Stack<GameObject> stack = new Stack<GameObject>();
        private GameObject baseObj;
        private GameObject tmp;
        private ReturnToPool returnPool;

        public ObjectPool(GameObject baseObj)
        {
            this.baseObj = baseObj;
        }

        public GameObject GetFromPool()
        {
            if (stack.Count > 0)
            {
                tmp = stack.Pop();
                tmp.SetActive(true);
                return tmp;
            }

            tmp = GameObject.Instantiate(baseObj);
            returnPool = tmp.AddComponent<ReturnToPool>();
            returnPool.pool = this;
            return tmp;
        }

        public void AddToPool(GameObject obj)
        {
            stack.Push(obj);
        }

        public int GetNumber()
        {
            return stack.Count;
        }
    }

}
