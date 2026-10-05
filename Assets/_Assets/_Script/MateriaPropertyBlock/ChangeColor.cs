using UnityEngine;
using UnityEngine.InputSystem;

public class NewMonoBehaviourScript : MonoBehaviour
{
    private Renderer renderer;
    private MaterialPropertyBlock materialPropertyBlock;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        renderer = GetComponent<Renderer>();
        materialPropertyBlock = new MaterialPropertyBlock();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ChangeColor(Color color)
    {
        materialPropertyBlock.SetColor("_BaseColor", color);
        renderer.SetPropertyBlock(materialPropertyBlock);
    }
}
