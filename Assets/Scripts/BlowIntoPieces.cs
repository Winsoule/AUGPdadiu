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
        if (piecesToBlowUp.Count > 0)
        {
            List<Rigidbody> blowThisUp = new List<Rigidbody>();
            foreach (Transform t in piecesToBlowUp)
            {
                Rigidbody tempBody = t.gameObject.AddComponent<Rigidbody>();
                tempBody.angularDrag = 1f;
                tempBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
                BoxCollider col = t.gameObject.AddComponent<BoxCollider>();
                col.size = Vector3.one * 0.2f;
                blowThisUp.Add(tempBody);
                t.parent = null;
                t.gameObject.AddComponent<DestroyAfter5Seconds>();
            }
            foreach (Rigidbody r in blowThisUp)
            {
                r.AddForceAtPosition(((r.position + Vector3.up) - (transform.position + Random.onUnitSphere * 0.01f)).normalized * Random.Range(8, 12), r.transform.TransformPoint(Random.insideUnitSphere * 0.1f),  ForceMode.VelocityChange);
            }
            Instantiate(explosion, transform.position, Quaternion.LookRotation(transform.up));
        }
    }

    
}
