using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullets : MonoBehaviour {

    public GameObject bulletParent;

    public enum BulletType {normal}

    //Bullet and impact prefabs:
    public GameObject bullet_normal;
    public GameObject impact_normal;

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

    public float Impact(BulletType type, Vector3 pos)
    {
        return Impact(pos, type);
    }

    public float Impact(Vector3 pos, BulletType type)
    {
        switch(type)
        {
            case BulletType.normal:
                Instantiate(impact_normal, pos, Quaternion.identity);
                return 1;
            default:
                return 0;
        }
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
