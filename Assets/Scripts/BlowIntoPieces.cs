using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent (typeof(MeshMaker))]
public class BlowIntoPieces : MonoBehaviour {

    public List<Transform> piecesToBlowUp;
    public List<Transform> immediateDestroy;
    public GameObject explosion;
    public float shakeAmount = 0;

    CameraShake cameraShake;

	// Use this for initialization
	void Start () {
        cameraShake = Camera.main.GetComponent<CameraShake>();
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
                Rigidbody tempBody = t.gameObject.GetComponent<Rigidbody>();
                if(tempBody == null)
                    tempBody = t.gameObject.AddComponent<Rigidbody>();
                tempBody.angularDrag = 1f;
                tempBody.isKinematic = false;
                tempBody.useGravity = true;
                tempBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
                BoxCollider col = t.gameObject.AddComponent<BoxCollider>();
                col.size = Vector3.one * 0.2f;
                blowThisUp.Add(tempBody);
                t.parent = null;
                t.gameObject.AddComponent<DestroyAfter5Seconds>();
                t.gameObject.layer = 11;
            }
            foreach (Rigidbody r in blowThisUp)
            {
                r.AddForceAtPosition(((r.position + Vector3.up) - (transform.position + Random.onUnitSphere * 0.01f)).normalized * Random.Range(8, 12), r.transform.TransformPoint(Random.insideUnitSphere * 0.1f),  ForceMode.VelocityChange);
            }
            Instantiate(explosion, transform.position, Quaternion.LookRotation(transform.up));
        }
        if(immediateDestroy.Count > 0)
        {
            foreach(Transform t in immediateDestroy)
            {
                ParticleSystem p = t.GetComponent<ParticleSystem>();
                if (p != null)
                    p.gameObject.AddComponent<TurnOffAndDestroyParticle>();
                else
                    Destroy(t.gameObject);
            }
        }
        if(cameraShake != null)
            cameraShake.PlayerSpecificShake(shakeAmount, transform.position);
    }

    
}
