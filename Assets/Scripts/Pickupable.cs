using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class Pickupable : MonoBehaviour
{
    public enum Hand
    {
        NONE, LEFT, RIGHT
    }

    public enum PickupState {
        LOOSE, INHAND, FLYING, BOUNCED
    }
    public InputActionAsset actions;
    private InputAction leftAction, rightAction, pickUpAction;

    public Transform playerTransform, leftHandTransform, rightHandTransform, cameraTransform;
    private Rigidbody rb;
    private Collider coll;

    public float pickUpRange, forwardDropForce, verticalDropForce;
    public static GameObject leftHand, rightHand;
    public float bounceForce = 10f;

    private Hand equippedIn = Hand.NONE;
    private PickupState pickupState = PickupState.LOOSE;
    void Awake()
    {
        InputActionMap actionMap = actions.FindActionMap("Weapon");
        leftAction = actionMap.FindAction("Left Action");
        rightAction = actionMap.FindAction("Right Action");
        pickUpAction = actionMap.FindAction("Pick Up");

        leftAction.performed += TryDropLeft;
        rightAction.performed += TryDropRight;
        pickUpAction.performed += TryPickUp;

        leftAction.Enable();
        rightAction.Enable();
        pickUpAction.Enable();

        this.rb = GetComponent<Rigidbody>();
        this.coll = GetComponent<Collider>();

        this.leftHandTransform = GameObject.Find("Main Camera/Left Hand").transform;
        this.rightHandTransform = GameObject.Find("Main Camera/Right Hand").transform;

    }

    private static Boolean hasSpace()
    {
        return leftHand == null || rightHand == null;
    }

    void PickUp()
    {
        // print("Being picked up!");
        pickupState = PickupState.INHAND;

        if (leftHand == null)
        {
            leftHand = gameObject;
            equippedIn = Hand.LEFT;
            transform.SetParent(leftHandTransform);
        }
        else
        {
            rightHand = gameObject;
            equippedIn = Hand.RIGHT;
            transform.SetParent(rightHandTransform);
        }

        rb.isKinematic = true;
        coll.isTrigger = true;

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        // transform.localScale = Vector3.one;

        // gunscript.enabled = true;
    }
    void TryPickUp(InputAction.CallbackContext context)
    {
        Vector3 distanceToPlayer = playerTransform.position - transform.position;
        if (hasSpace() && distanceToPlayer.magnitude < pickUpRange && equippedIn == Hand.NONE)
        {
            PickUp();
        }

    }
    void Drop()
    {
        print("Being dropped!");
        if (equippedIn == Hand.LEFT)
        {
            leftHand = null;
        }
        else if (equippedIn == Hand.RIGHT)
        {
            rightHand = null;
        }
        equippedIn = Hand.NONE;
        rb.isKinematic = false;
        coll.isTrigger = false;

        transform.SetParent(null);
        transform.position = cameraTransform.position;

        pickupState = PickupState.FLYING;
        rb.velocity = GetComponent<Rigidbody>().velocity;
        rb.AddForce(cameraTransform.forward * forwardDropForce, ForceMode.Impulse);
        rb.AddForce(cameraTransform.up * verticalDropForce, ForceMode.Impulse);

    }

    void OnTriggerEnter(Collider other)
    {
        if (equippedIn == Hand.NONE)
        {
            if (other.CompareTag("Enemy") && pickupState == PickupState.FLYING)
            {
                Vector3 bounceDir = new Vector3(-rb.velocity.x, 0, -rb.velocity.z).normalized;
                bounceDir = Quaternion.AngleAxis(Random.Range(-45, 45), Vector3.up) * bounceDir;
                bounceDir.y = 5;
                bounceDir = bounceDir.normalized * bounceForce;
                rb.velocity = bounceDir;
                pickupState = PickupState.BOUNCED;
            }
            else if (other.CompareTag("Player") && pickupState == PickupState.BOUNCED)
            {
                if (hasSpace())
                {
                    PickUp();
                }
            }
        }
    }

    // void OnCollisionEnter(Collision collision)
    // {   
    //     print("Collided with " + collision.gameObject.tag);
    //     if (collision.gameObject.tag == "Player")
    //     {
    //         if (hasSpace() && isFlying)
    //         {
    //             PickUp();
    //         }
    //     }
    // }

    void TryDropLeft(InputAction.CallbackContext context)
    {
        if (this.equippedIn == Hand.LEFT) Drop();
    }
    void TryDropRight(InputAction.CallbackContext context)
    {
        if (this.equippedIn == Hand.RIGHT) Drop();
    }

}
