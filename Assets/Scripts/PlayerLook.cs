using UnityEngine;
using System.Collections;
using InControl;

public class PlayerLook : MonoBehaviour {

    public Camera playerCamera;
    Vector3 orgCamOffset;
    InputDevice controller;

    // Use this for initialization
    void Start()
    {
        controller = InputManager.ActiveDevice;
        orgCamOffset = playerCamera.transform.position - transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        controller = InputManager.ActiveDevice;
        Vector3 aimDir = Vector3.ProjectOnPlane(playerCamera.transform.TransformDirection(new Vector3(controller.RightStickX, 0, controller.RightStickY)), Vector3.up);
        Vector3 walkDir = Vector3.ProjectOnPlane(playerCamera.transform.TransformDirection(new Vector3(controller.LeftStickX, 0, controller.LeftStickY)), Vector3.up);
        playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, transform.position + orgCamOffset + aimDir * 3, Time.fixedDeltaTime * 4);
    }
}
