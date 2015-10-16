using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlowIntoPieces : MonoBehaviour {

    public List<Transform> piecesToBlowUp;
    public GameObject explosion;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void BlowUp()
    {
        List<Rigidbody> blowThisUp = new List<Rigidbody>();
        foreach (Transform t in piecesToBlowUp)
        {
            Rigidbody tempBody = t.gameObject.AddComponent<Rigidbody>();
            tempBody.angularDrag = 1f;
            BoxCollider col = t.gameObject.AddComponent<BoxCollider>();
            col.size = Vector3.one * 0.1f;
            blowThisUp.Add(tempBody);
            t.parent = null;
            t.gameObject.AddComponent<DestroyAfter5Seconds>();
        }
        foreach(Rigidbody r in blowThisUp)
        {
            r.AddForce(((r.position + Vector3.up) - (transform.position + Random.onUnitSphere*0.01f)).normalized * Random.Range(3, 5), ForceMode.VelocityChange);
        }
        Instantiate(explosion, transform.position, transform.rotation);
    }

    
}
