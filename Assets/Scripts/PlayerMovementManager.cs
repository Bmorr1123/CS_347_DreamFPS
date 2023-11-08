using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementManager : MonoBehaviour
{
    // Public Parameters
    public InputActionAsset playerInputActions;
    public GameObject cameraGO;
    public Transform orientation;
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump = true;
    bool sprinting = false;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    float startYScale;
    bool crouching;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    PlayerCam camControllerScript;
    InputAction playerMove;
    InputAction playerLook;
    InputAction playerJump;
    InputAction playerSprint;
    InputAction playerCrouch;
    Rigidbody rb;

    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;

    public MovementState mState;
    public enum MovementState
    {
        walking,
        crouching,
        sprinting,
        air
    }

    // Start is called before the first frame update
    void Awake()
    {
        playerMove = playerInputActions.FindActionMap("PlayerControl").FindAction("Movement");
        playerLook = playerInputActions.FindActionMap("PlayerControl").FindAction("Look");
        playerJump = playerInputActions.FindActionMap("PlayerControl").FindAction("Jump");
        playerSprint = playerInputActions.FindActionMap("PlayerControl").FindAction("Sprint");
        playerCrouch = playerInputActions.FindActionMap("PlayerControl").FindAction("Crouch");

        playerInputActions.FindActionMap("PlayerControl").FindAction("Jump").performed += Jump;
        playerInputActions.FindActionMap("PlayerControl").FindAction("Sprint").performed += StartSprinting;
        playerInputActions.FindActionMap("PlayerControl").FindAction("Crouch").performed += StartCrouching;
        playerInputActions.FindActionMap("PlayerControl").FindAction("Crouch").canceled += StopCrouching;

        camControllerScript = cameraGO.GetComponent<PlayerCam>();

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        startYScale = transform.localScale.y;
    }

    void FixedUpdate()
    {
        ProcessMove(playerMove.ReadValue<Vector2>());
        SpeedControl();
        ProcessLook(playerLook.ReadValue<Vector2>());
        MovementStateHandler();
    }

    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        rb.drag = grounded ? groundDrag : 0;
    }

    void ProcessMove(Vector2 input)
    {
        horizontalInput = input.x;
        verticalInput = input.y;

        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }
    
    void ProcessLook(Vector2 input)
    {
        camControllerScript.ProcessLookInput(input);
    }

    void StartSprinting(InputAction.CallbackContext context)
    {
        sprinting = true;
    }

    void StopSprinting()
    {
        sprinting = false;
    }

    void StartCrouching(InputAction.CallbackContext context)
    {
        crouching = true;
        transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
    }

    void StopCrouching(InputAction.CallbackContext context)
    {
        crouching = false;
        transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
    }

    void MovementStateHandler()
    {
        if (crouching)
        {
            mState = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }
        else if (grounded && sprinting)
        {
            mState = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }
        else if (grounded)
        {
            mState = MovementState.walking;
            moveSpeed = walkSpeed;
        }
        else
        {
            mState = MovementState.air;
        }

        if (sprinting)
        {
            if (rb.velocity.magnitude < 0.75 * walkSpeed)
            {
                StopSprinting();
            }
        }
    }

    void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    void Jump(InputAction.CallbackContext context)
    {
        if(!readyToJump || !grounded)
        {
            return;
        }

        readyToJump = false;

        rb.velocity = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        Invoke(nameof(ResetJump), jumpCooldown);
    }
    
    void ResetJump()
    {
        readyToJump = true;
    }

    void OnEnable()
    {
        playerInputActions.FindActionMap("PlayerControl").Enable();
    }
    void OnDisable()
    {
        playerInputActions.FindActionMap("PlayerControl").Disable();
    }
}
