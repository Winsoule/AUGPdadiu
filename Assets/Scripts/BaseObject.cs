using UnityEngine;
using System.Collections;

public class BaseObject : MonoBehaviour {

    public Transform hand;
    Vector3 target;
    Rigidbody body;

	// Use this for initialization
	void Start ()
    {
        body = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Throw(Vector3 newTarget, Transform owner)
    {
        hand = owner;
        target = newTarget;
        body.isKinematic = false;
        body.velocity = (target - transform.position).normalized * 30;
    }
}
