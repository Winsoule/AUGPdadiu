using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class BulletBehaviour : MonoBehaviour {

    Bullets bulletManager;
    Bullets.BulletType bulletType;
    bool bulletTypeSet = false;
    GameObject currentBulletCasing;
    Rigidbody body;
    //Bullets.BulletInfo bulletInfo;


    public void SetBulletType(Bullets.BulletType newBullet)
    {
        bulletType = newBullet;
        CreateBullet();
        bulletTypeSet = true;
        return;
    }

    void CreateBullet()
    {
        if (currentBulletCasing != null)
            Destroy(currentBulletCasing);
        switch(bulletType)
        {
            case Bullets.BulletType.normal:
                currentBulletCasing = Instantiate(bulletManager.GetBullet(bulletType), transform.position, transform.rotation) as GameObject;
                currentBulletCasing.transform.parent = transform;
                break;
            default:
                break;
        }
    }

	// Use this for initialization
	void Awake ()
    {
        bulletManager = GameObject.Find("GameManager").GetComponent<Bullets>();
        if (bulletManager == null)
            Debug.Log("I am sad, because I can't find 'GameManager' object with 'Bullets' component...");
        body = GetComponent<Rigidbody>();
        body.useGravity = false;
        //body.isKinematic = true;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
	    if(bulletTypeSet)
        {
            switch(bulletType)
            {
                case Bullets.BulletType.normal:
                    //BEHAVIOUR, MATHAFACKA!!
                    body.velocity = transform.forward * 20;
                    break;
                default:
                    break;
            }
        }
	}

    void OnCollisionEnter(Collision col)
    {
        float impactForce = bulletManager.Impact(Bullets.BulletType.normal, transform.position);
        if(col.rigidbody != null)
        {
            if(!col.rigidbody.isKinematic)
                col.rigidbody.AddForce(Vector3.ProjectOnPlane(body.velocity.normalized, Vector3.up).normalized * impactForce, ForceMode.Impulse);
            else
            {
                NavMeshAgent agent = col.gameObject.GetComponent<NavMeshAgent>();
                if(agent != null)
                {
                    agent.velocity += Vector3.ProjectOnPlane(body.velocity.normalized, Vector3.up).normalized * impactForce;
                }
            }

        }
        Destroy(gameObject);
    }
}
