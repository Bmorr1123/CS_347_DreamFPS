using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class NoClipController : MonoBehaviour
{
    public Transform targetTransform;

    public InputActionAsset actions;
    private InputAction cameraX, cameraY;

    float sensitivity = 2f;
    float yRotationLimit = 88f;
    private Vector2 rotation = Vector2.zero;

    void Awake()
    {

        InputActionMap actionMap = actions.FindActionMap("Camera");
        cameraX = actionMap.FindAction("MouseX");
        cameraY = actionMap.FindAction("MouseY");
        cameraX.Enable();
        cameraY.Enable();

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2(cameraX.ReadValue<float>(), cameraY.ReadValue<float>());
        // print(rotation);

        rotation.x += input.x * sensitivity;
        rotation.y += input.y * sensitivity;
        rotation.y = Mathf.Clamp(rotation.y, -yRotationLimit, yRotationLimit);
        var xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);

        targetTransform.localRotation = xQuat * yQuat;
    }
}

