using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class Movement : MonoBehaviour
{

    public Transform cameraTransform;
    public Transform gunhandler;
    InputDevice controller;

    // Use this for initialization
    void Start ()
    {
        controller = InputManager.ActiveDevice;
    }
	
	// Update is called once per frame
	void Update ()
    {
        controller = InputManager.ActiveDevice;
        if(Mathf.Abs(controller.RightStickX) > 0.2f || Mathf.Abs(controller.RightStickY) > 0.2f)
        {
            Vector3 dir = Vector3.ProjectOnPlane(cameraTransform.TransformDirection(new Vector3(controller.RightStickX, 0, controller.RightStickY)), Vector3.up);
            gunhandler.rotation = Quaternion.LookRotation(dir);
        }
	}

    void FixedUpdate()
    {
        Vector3 dir = Vector3.ProjectOnPlane(cameraTransform.TransformDirection(new Vector3(controller.LeftStickX, 0, controller.LeftStickY)), Vector3.up);
        transform.position += dir * Time.fixedDeltaTime * 4;
    }
}
