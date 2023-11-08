using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCam : MonoBehaviour
{
    public InputActionAsset actions;

    public float sensX;
    public float sensY;

    public Transform orientation;

    private InputAction lookAction;
    float xRotation;
    float yRotation;
    // Start is called before the first frame update
    void Awake()
    {
        lookAction = actions.FindActionMap("PlayerControl").FindAction("Look");
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ProcessLookInput(Vector2 lookVector)
    {
        float mouseX = lookVector.x * Time.deltaTime * sensX;
        float mouseY = lookVector.y * Time.deltaTime * sensY;

        yRotation += mouseX;        

        xRotation -= mouseY;        
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
