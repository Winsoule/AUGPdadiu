using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BlowIntoPieces))]
public class Health : MonoBehaviour {

    [HideInInspector]
    public float startHealth;
    public float health = 1;

	// Use this for initialization
	void Start () {
        RunAtStart();
	}

    protected void RunAtStart()
    {
        startHealth = health;
    }
	
	// Update is called once per frame
	void Update () {
        if (health <= 0)
        {
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
