using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    public InputActionAsset actions;
    private InputAction leftAction, rightAction;
    private Rigidbody weaponBody;

    public Vector3 launchAngle = new Vector3(0, 0.25f, 1).normalized;
    public float launchForce = 0f;

    void Awake()
    {
        InputActionMap actionMap = actions.FindActionMap("Weapon");
        leftAction = actionMap.FindAction("Left Action");
        rightAction = actionMap.FindAction("Right Action");

        leftAction.performed += OnLeftAction;
        rightAction.performed += OnRightAction;

        leftAction.Enable();
        rightAction.Enable();

        print(leftAction);
        print(rightAction);

        weaponBody = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {     
        // if (leftAction.) {
        //     weaponBody.AddForce(Vector3.forward * 10);
        // }
    }

    private void OnLeftAction(InputAction.CallbackContext context)
    {
        print("Left Action!!!");
        this.weaponBody.AddForce(this.launchAngle * launchForce);
    }

    private void OnRightAction(InputAction.CallbackContext context)
    {
        print("Right Action!!!");
    }

}
