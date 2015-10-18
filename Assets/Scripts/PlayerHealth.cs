using UnityEngine;
using System.Collections;

public class PlayerHealth : Health {

    public RectTransform healthBar;
    public GameObject hitBlood;
    float width;
    bool theresAHealthBar = true;

	// Use this for initialization
	void Start () {
        RunAtStart();
        if (healthBar != null)
            width = healthBar.sizeDelta.x - float.Epsilon;
        else
            theresAHealthBar = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(theresAHealthBar)
            healthBar.sizeDelta = new Vector2(width * (health / startHealth) + float.Epsilon, healthBar.sizeDelta.y);
	}

    void OnCollisionEnter(Collision col)
    {
        if (col.transform.root != null && col.transform.root.GetComponent<BulletBehaviour>() != null)
        {
            Instantiate(hitBlood, col.contacts[0].point, Quaternion.LookRotation(-col.contacts[0].normal));
            health -= 1;
            if(theresAHealthBar)
                healthBar.sizeDelta = new Vector2(width * (health / startHealth) + float.Epsilon, healthBar.sizeDelta.y);
            if (health == 0)
            {
                GetComponent<BlowIntoPieces>().BlowUp();
                Destroy(gameObject);
            }
        }
    }
}
