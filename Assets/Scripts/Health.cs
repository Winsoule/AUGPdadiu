using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BlowIntoPieces))]
public class Health : MonoBehaviour {

    public float health = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.root != null && col.transform.root.GetComponent<BulletBehaviour>() != null)
        {
            health -= 1;
            if (health == 0)
            {
                GetComponent<BlowIntoPieces>().BlowUp();
                Destroy(gameObject);
            }
        }
    }
}
