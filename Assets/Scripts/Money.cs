using UnityEngine;
using System.Collections;

public class Money : MonoBehaviour {

    Rigidbody body;
    UnitManager manager;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody>();
        manager = GameObject.Find("GameManager").GetComponent<UnitManager>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.Rotate(Vector3.up, Time.fixedDeltaTime * 45);
        if (manager.player != null && Vector3.Distance(transform.position, manager.player.position) < 4)
        {
            body.velocity += (manager.player.position - transform.position).normalized * Time.fixedDeltaTime * 50;
        }
	}

    void OnCollisionEnter(Collision col)
    {
        if(manager.player != null && col.transform == manager.player)
        {
            manager.money += 1;
            Destroy(gameObject);
        }
    }
}
