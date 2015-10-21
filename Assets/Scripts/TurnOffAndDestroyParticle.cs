using UnityEngine;
using System.Collections;

public class TurnOffAndDestroyParticle : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        ParticleSystem p = GetComponent<ParticleSystem>();
        p.enableEmission = false;
        Destroy(gameObject, p.startLifetime);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
