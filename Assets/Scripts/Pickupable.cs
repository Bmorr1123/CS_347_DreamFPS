using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector3 = UnityEngine.Vector3;

public class Pickupable : MonoBehaviour
{

    public InputActionAsset actions;
    private InputAction leftAction, dropAction;

    public Transform playerTransform, gunContainerTransform, cameraTransform;
    public Rigidbody rb;
    public Collider coll;

    public float pickUpRange, forwardDropForce, verticalDropForce;
    public static Boolean leftFull;

    private Boolean equipped;
    void Awake()
    {
        print(actions);
        InputActionMap actionMap = actions.FindActionMap("Weapon");
        leftAction = actionMap.FindAction("Left Action");
        // rightAction = actionMap.FindAction("Right Action");

        leftAction.performed += TryEither;
        // rightAction.performed += OnRightAction;

        leftAction.Enable();
        // rightAction.Enable();

        // leftAction.performed += TryPickUp;
        // dropAction.performed += TryDrop;

        this.rb = GetComponent<Rigidbody>();
        this.coll = GetComponent<Collider>();
    }

    void TryEither(InputAction.CallbackContext context) {
        if(this.equipped) {
            TryDrop(context);
        } else {
            TryPickUp(context);
        }
    }

    void PickUp()
    {   
        print("Being picked up!");
        leftFull = true;
        equipped = true;
        rb.isKinematic = true;
        coll.isTrigger = true;

        transform.SetParent(gunContainerTransform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        // transform.localScale = Vector3.one;

        // gunscript.enabled = true;
    }
    void TryPickUp(InputAction.CallbackContext context)
    {
        Vector3 distanceToPlayer = playerTransform.position - transform.position;
        if (!leftFull && distanceToPlayer.magnitude < pickUpRange && !equipped)
        {
            PickUp();
        }

    }
    void Drop()
    {
        print("Being dropped!");
        leftFull = false;
        equipped = false;
        rb.isKinematic = false;
        coll.isTrigger = false;

        transform.SetParent(null);

        rb.velocity = GetComponent<Rigidbody>().velocity;
        rb.AddForce(cameraTransform.forward * forwardDropForce, ForceMode.Impulse);
        rb.AddForce(cameraTransform.up * verticalDropForce, ForceMode.Impulse);// test

    }
    void TryDrop(InputAction.CallbackContext context)
    {
        if (this.equipped) Drop();
    }

}
