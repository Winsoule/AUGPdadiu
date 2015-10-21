using UnityEngine;
using System.Collections;

public class HackHackBladeScript : MonoBehaviour {

    enum State { seeking, deployed, returning, waiting}

    State state = State.waiting;

    public Transform scaler;
    Transform hand;
    Vector3 target;
    Rigidbody body;
    bool thrown = false;
    bool reachedTarget = false;
    float deployCounter = 0;
    float returnCounter = 0;

	// Use this for initialization
	void Start ()
    {
        body = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(hand != null && hand.parent == null)
        {
            enabled = false;
            return;
        }
        switch (state)
        {
            case State.waiting:
                break;
            case State.seeking:
                deployCounter = Mathf.Clamp(deployCounter + Time.deltaTime * 10, 0, 5);
                transform.Rotate(deployCounter * 100 * Vector3.up * Time.deltaTime);
                scaler.localScale = Vector3.one * deployCounter * 0.5f;
                if (Vector3.Distance(target, transform.position) < 1)
                {
                    body.velocity = Vector3.zero;
                    state = State.deployed;
                }
                break;
            case State.deployed:
                deployCounter = Mathf.Clamp(deployCounter + Time.deltaTime * 20, 0, 5);
                scaler.localScale = Vector3.one * deployCounter * 0.5f;
                transform.Rotate(deployCounter * 100 * Vector3.up * Time.deltaTime);
                returnCounter += Time.deltaTime;
                if (returnCounter > 2)
                {
                    returnCounter = 0;
                    state = State.returning;
                }
                break;
            case State.returning:
                deployCounter = Mathf.Clamp(deployCounter - Time.deltaTime * 20, 0, 5);
                scaler.localScale = Vector3.one * deployCounter * 0.5f;
                body.velocity = (hand.position - transform.position).normalized * 20;
                if (Vector3.Distance(hand.position, transform.position) < 1)
                {
                    scaler.localScale = Vector3.zero;
                    body.velocity = Vector3.zero;
                    body.isKinematic = true;
                    transform.position = hand.position;
                    transform.parent = hand.parent;
                    hand.root.GetComponent<HackHackShoot>().BladeReturned(transform);
                    state = State.waiting;
                }
                break;
        }
	}

    public void Throw(Vector3 newTarget, Transform owner)
    {
        deployCounter = 0;
        hand = owner;
        target = newTarget;
        body.isKinematic = false;
        body.velocity = (target - transform.position).normalized * 20;
        transform.rotation = Quaternion.LookRotation(target - transform.position, Vector3.up);
        state = State.seeking;
    }

    void OnTriggerStay (Collider col)
    {
        if (state != State.waiting)
        {
            Transform tempTrans = col.transform.root;
            if (tempTrans == null)
                tempTrans = col.transform;
            if (tempTrans == hand.root)
                return;
            Health health = tempTrans.GetComponent<Health>();
            if (health != null)
                health.health -= Time.deltaTime;
        }
    }
}
