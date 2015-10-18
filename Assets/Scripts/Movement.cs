using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class Movement : MonoBehaviour
{
    public Transform upperBody;
    public Transform lowerBody;
    public Transform eyes;
    public Transform cameraTransform;
    public Transform gunhandler;

    [HideInInspector]
    public InputDevice controller;

    Rigidbody body;
    [HideInInspector]
    public Vector3 travelDirection = Vector3.zero;
    [HideInInspector]
    public bool gunHolstered = true;
    [HideInInspector]
    public bool canMove = true;
    [HideInInspector]
    public bool canLook = true;

    // Use this for initialization
    void Start ()
    {
        body = GetComponent<Rigidbody>();
        controller = InputManager.ActiveDevice;
        travelDirection = transform.forward;
        GameObject.Find("GameManager").GetComponent<UnitManager>().player = transform;
    }
	
	// Update is called once per frame
	void Update ()
    {
        controller = InputManager.ActiveDevice;
        if (canLook)
        {
            if (Mathf.Abs(controller.RightStickX) > 0.2f || Mathf.Abs(controller.RightStickY) > 0.2f)
            {
                HolsterGuns(false);
                Vector3 dir = Vector3.ProjectOnPlane(cameraTransform.TransformDirection(new Vector3(controller.RightStickX, 0, controller.RightStickY)), Vector3.up);
                gunhandler.rotation = Quaternion.Lerp(gunhandler.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 12);
                upperBody.rotation = Quaternion.Lerp(upperBody.rotation, Quaternion.LookRotation(gunhandler.forward), Time.deltaTime * 8);
                lowerBody.rotation = Quaternion.Lerp(lowerBody.rotation, upperBody.rotation, Time.deltaTime * 3);

                eyes.rotation = Quaternion.Lerp(eyes.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 12);
                if (eyes.localEulerAngles.y > 180)
                    eyes.localEulerAngles = new Vector3(0, Mathf.Clamp((eyes.localEulerAngles.y), 345, 360), 0);
                else
                    eyes.localEulerAngles = new Vector3(0, Mathf.Clamp((eyes.localEulerAngles.y), 0, 15), 0);
            }
            else
            {
                HolsterGuns(true);
                gunhandler.rotation = Quaternion.Lerp(gunhandler.rotation, Quaternion.LookRotation(travelDirection), Time.deltaTime * 3);
                upperBody.rotation = Quaternion.Lerp(upperBody.rotation, Quaternion.LookRotation(travelDirection), Time.deltaTime * 5);
                lowerBody.rotation = Quaternion.Lerp(lowerBody.rotation, upperBody.rotation, Time.deltaTime * 3);
            }

            if (controller.Action1.WasPressed)
                body.AddForce(Vector3.up * 15, ForceMode.VelocityChange);

            if (controller.Action2.WasPressed)
            {
                body.AddForce(Vector3.left * 3, ForceMode.VelocityChange);
            }
        }
    }

    void FixedUpdate()
    {
        
        if (canMove)
        {
            Vector3 dir = Vector3.ProjectOnPlane(cameraTransform.TransformDirection(new Vector3(controller.LeftStickX, 0, controller.LeftStickY)), Vector3.up).normalized;
            float speed = (gunHolstered ? 4 : 2);
            Vector3 force = dir * speed * controller.LeftStick.Vector.magnitude;
            if ((body.velocity + force).magnitude <= speed * 10)
                body.AddForce(force * 10, ForceMode.Acceleration);
            if (dir.magnitude > 0.2f) travelDirection = dir;
            else if (!gunHolstered) travelDirection = gunhandler.forward;
        }
        body.velocity -= body.velocity * 0.1f;
    }

    public void HolsterGuns(bool holster)
    {
        gunhandler.gameObject.SetActive(!holster);
        gunHolstered = holster;
    }
}
