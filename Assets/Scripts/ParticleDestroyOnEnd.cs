﻿using UnityEngine;
using System.Collections;

public class ParticleDestroyOnEnd : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Destroy(gameObject, GetComponent<ParticleSystem>().duration);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
