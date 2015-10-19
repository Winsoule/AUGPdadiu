﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BlowIntoPieces))]
public class Health : MonoBehaviour {

    [HideInInspector]
    public float startHealth;
    public float health = 1;
    public bool isBoss;
    public Material portalMat;
    MeshMaker meshMaker;

    // Use this for initialization
    void Start () {
        RunAtStart();
        meshMaker = GetComponent<MeshMaker>();
    }

    protected void RunAtStart()
    {
        //startHealth = health;
    }
	
	// Update is called once per frame
	void Update () {
        if (health <= 0)
        {
            if (isBoss)
            {
                if(GameObject.Find("GameManager").GetComponent<MakeLevel>().bosses.Count == 1)
                {
                    GameObject go = meshMaker.Torus(portalMat);
                    go.name = "Portal";
                    go.transform.position = transform.position;
                    go.transform.rotation = Quaternion.Euler(50, 225, 0);
                    go.AddComponent<Portal>();
                    go.AddComponent<BoxCollider>();
                    go.GetComponent<BoxCollider>().isTrigger = true;
                }
                else
                {
                    GameObject.Find("GameManager").GetComponent<MakeLevel>().bosses.Remove(gameObject);
                }
            }
            GetComponent<BlowIntoPieces>().BlowUp();
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.transform.root != null && col.transform.root.GetComponent<BulletBehaviour>() != null)
        {
            health -= 1;
        }
    }
}
