using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class IgnoreRelativeVelocity : MonoBehaviour {

    //This script needs to be on the outermost object
    //AND there needs to be a RigidBody on that object
    //for it to work. 

    public bool ignoreMe = false;

    Rigidbody body;
    Vector3 lastVel = Vector3.zero;
    Vector3 lastAng = Vector3.zero;
    Vector3 currentVel = Vector3.zero;
    Vector3 currentAng = Vector3.zero;

    // Use this for initialization
    void Start ()
    {
        body = GetComponent<Rigidbody>();
        if (body == null)
        {
            Debug.Log("MISSING A RIGIDBODY!");
            enabled = false;
            return;
        }
        currentVel = body.velocity;
        currentAng = body.angularVelocity;
	}
	
	// Update is called once per frame
	void Update ()
    {
        lastVel = currentVel;
        lastAng = currentAng;
        currentVel = body.velocity;
        currentAng = body.angularVelocity;
	}

    void OnCollisionEnter(Collision col)
    {
        Transform obj;
        if (col.transform.root != null)
        {
            obj = col.transform.root; 
        }
        else
            obj = col.transform;
        IgnoreRelativeVelocity IRV = obj.GetComponent<IgnoreRelativeVelocity>();
        if (IRV != null)
        {
            if (IRV.ignoreMe)
            {
                body.velocity = lastVel;
                body.angularVelocity = lastAng;
            }
        }
    }

}
