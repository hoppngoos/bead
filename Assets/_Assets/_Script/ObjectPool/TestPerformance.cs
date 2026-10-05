using UnityEngine;

public class TestPerformance : MonoBehaviour
{
    public TestObject testObject;
    public int amount = 2000;
    void Update()
    {
        for (int i = 0; i < amount; i++)
        {
            //MyPoolManager.Instance.GetFromPool(testObject.gameObject);
        }
    }
}



