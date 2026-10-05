using UnityEngine;
using System.Collections.Generic;

public class MyPool
{
    private Stack<GameObject> stack = new Stack<GameObject>();
    private GameObject baseObj;
    private GameObject tmp;

    private ReturnToMyPool returnPool;
    public MyPool(GameObject baseObj)
    {
        this.baseObj = baseObj;
    }

    public GameObject Get(Vector3 position)
    {
        if (stack.Count > 0)
        {
            tmp = stack.Pop();
            tmp.transform.position = position;
            tmp.SetActive(true);
            return tmp;
        }
        tmp = GameObject.Instantiate(baseObj);
        tmp.transform.position = position;
        returnPool = tmp.AddComponent<ReturnToMyPool>();
        returnPool.pool = this;
        return tmp;
    }

    public void AddToPool(GameObject obj)
    {
        stack.Push(obj);
    }
}