using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullets : MonoBehaviour {

    public GameObject bulletParent;

    public enum BulletType {normal}
    public GameObject bullet_normal;

    /*
    public class BulletInfo
    {
        public float speed;
        public float damage;
        public Owners.Type owner;

        public virtual void Fired(BulletBehaviour bullet)
        {
            
        } 

        BulletInfo(float lol, float rofl, Owners.Type blue)
        {
            speed = 
        }
    }

    public BulletInfo normalBullet;
    */
    public GameObject GetBullet(BulletType bulletType)
    {
        switch(bulletType)
        {
            case BulletType.normal:
                return bullet_normal;
            default:
                Debug.Log("Ain't no bullet of that type, foo'!");
                return bullet_normal;
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /////////////
    // BULLETS://
    /////////////
    /*
    public class BulletInfo_Normal : BulletInfo
    {
        
    }
    */
}
