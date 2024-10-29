using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;
    private Dictionary<GameObject, ObjectPool> dicPools = new Dictionary<GameObject, ObjectPool>();  // dùng GameObject thay cho string làm Key cho đỡ rác

    private void Awake()
    {
        Instance = this;
    }
    
    public GameObject GetFromPool(GameObject obj)
    {
        if (dicPools.ContainsKey(obj) == false)
        {
            dicPools.Add(obj, new ObjectPool(obj));
        }

        Debug.Log("pool " + obj.name +": " + dicPools[obj].GetNumber());
        return dicPools[obj].GetFromPool();
    }
}
