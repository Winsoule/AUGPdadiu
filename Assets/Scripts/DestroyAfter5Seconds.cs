using UnityEngine;
using System.Collections;

public class DestroyAfter5Seconds : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        Destroy(gameObject, 5);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
