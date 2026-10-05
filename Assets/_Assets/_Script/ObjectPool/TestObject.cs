using UnityEngine;

public class TestObject : MonoBehaviour
{
    public Renderer renderer;
    public MaterialPropertyBlock materialPropertyBlock;
    public Vector3 position;
    void Awake()
    {
        renderer = GetComponent<Renderer>();
        materialPropertyBlock = new MaterialPropertyBlock();
    }
    private void OnEnable()
    {
        //transform.position = position;
    }
    private void Update()
    {
        //gameObject.SetActive(false);
    }
    public TestObject PlaceBead(Vector3 placePosition, Color beadColor)
    {
        GameObject placedObject = MyPoolManager.Instance.GetFromPool(gameObject, placePosition);
        TestObject placedBead = placedObject.GetComponent<TestObject>();
        if (placedBead != null)
            placedBead.ChangeColor(beadColor);

        return placedBead;
    }
    public void ChangeColor(Color color)
    {
        materialPropertyBlock.SetColor("_BaseColor", color);
        renderer.SetPropertyBlock(materialPropertyBlock);
    }
}       
public enum Col 
{
    white,
    orange,
    magenta,
    lightBlue,
    yellow,
    lime,
    pink,
    lightGray, 
    gray,
    cyan,
    purple, 
    blue,
    brown,
    green,
    red,
    black 

}
