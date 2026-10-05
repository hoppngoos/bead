using UnityEngine;
using UnityEngine.InputSystem;

public class TestChangeColor : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("left click!");
            Ray ray =Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            { 
                GameObject clickedObj = hit.collider.gameObject;
                Debug.Log("Clicked object: " + clickedObj.name);
                clickedObj.GetComponent<NewMonoBehaviourScript>().ChangeColor(Color.blue);
            }
        }
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Debug.Log("right click!");
            Ray ray =Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            { 
                GameObject clickedObj = hit.collider.gameObject;
                Debug.Log("Clicked object: " + clickedObj.name);
                clickedObj.GetComponent<NewMonoBehaviourScript>().ChangeColor(Color.red);
            }
        }
    }
}
