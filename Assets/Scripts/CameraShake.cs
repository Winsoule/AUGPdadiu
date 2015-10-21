using UnityEngine;
using System.Collections;
using InControl;

public class CameraShake : MonoBehaviour {

    Quaternion orgRot;
    float shakeAmount = 0;
    InputDevice controller;
    UnitManager manager;
    Transform player = null;

	// Use this for initialization
	void Start () {
        orgRot = transform.rotation;
        controller = InputManager.ActiveDevice;
        GameObject temp = GameObject.Find("GameManager");
        if(temp != null)
            manager = temp.GetComponent<UnitManager>();
        if (manager != null)
            player = manager.player;
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.rotation = orgRot;
        if (shakeAmount > float.Epsilon)
        {
            transform.Rotate(Vector3.up, Random.Range(0, shakeAmount));
            transform.Rotate(Vector3.right, Random.Range(0, shakeAmount));
            shakeAmount -= Time.deltaTime + shakeAmount*Time.deltaTime;
            controller.Vibrate(shakeAmount * 10000);
        }
	}

    public void Shake(float newshake)
    {
        if(newshake > shakeAmount)
            shakeAmount = newshake;
    }

    public void PlayerSpecificShake(float newShake, Vector3 shakeOrigin)
    {
        if(player != null)
        {
            shakeAmount = newShake * Mathf.Clamp(20 - Vector3.Distance(shakeOrigin, player.position), 0, 20) / 20;
        }
    }
}
