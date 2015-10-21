using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HackHackShoot : MonoBehaviour {

    public enum State {off, deploying, deployed, shooting, retracting}
    public enum EnemyType {wheely, wheelyBoss}
    public enum CurrentWeapon { left, right}

    //public Transform Gun;
    //public List<Transform> Nozzles;
    //public Transform Arm;
    //Vector3 GunOrgLocalPos;
    //Vector3 ArmOrgLocalPos;

    public Transform leftArm;
    public Transform rightArm;
    public Transform leftBlade;
    public Transform rightBlade;
    public Transform leftBladePos;
    public Transform rightBladePos;

    HackHackBehavior behavior;
    Bullets bullets;
    Transform target;
    public State state = State.off;
    public EnemyType enemyType = EnemyType.wheely;
    CurrentWeapon currentWeapon = CurrentWeapon.left;
    bool shootSomething = false;
    int currentNozzle = 0;
    float deployWeight = 0;
    public float gunCoolDown = 1;
    float gunCooldownCounter = 0;
    bool canShoot = true;
    bool leftBladeHome = true;
    bool rightBladeHome = true;


    // Use this for initialization
    void Start ()
    {
        if(leftArm == null || rightArm == null || leftBlade == null || rightBlade == null)
        {
            Debug.Log("Missing assignments for " + gameObject.name + "'!");
            enabled = false;
        }
        Physics.IgnoreCollision(GetComponentInChildren<Collider>(), leftBlade.GetComponent<Collider>(), true);
        Physics.IgnoreCollision(GetComponentInChildren<Collider>(), rightBlade.GetComponent<Collider>(), true);

        gunCooldownCounter = gunCoolDown;
        behavior = GetComponent<HackHackBehavior>();
        bullets = GameObject.Find("GameManager").GetComponent<Bullets>();
        //target = behavior.player.transform;
	}
	
	// Update is called once per frame
	void Update ()
    {
        leftArm.rotation = Quaternion.LookRotation(leftBlade.position - leftArm.position);
        rightArm.rotation = Quaternion.LookRotation(rightBlade.position - rightArm.position);
        if (behavior == null || behavior.player == null)
        {
            shootSomething = false;
        }
        gunCooldownCounter = Mathf.Max(gunCooldownCounter - Time.deltaTime, 0);
        if (gunCooldownCounter == 0)
        {
            canShoot = true;
        }
        switch (state)
        {
            case State.off:
                if (shootSomething)
                    state = State.deploying;
                break;
            case State.deploying:

                if (shootSomething)
                {
                    deployWeight = Mathf.Clamp(deployWeight + Time.deltaTime, 0, 1);
                    if (deployWeight == 1)
                        state = State.deployed;
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.ProjectOnPlane(target.position - transform.position, Vector3.up).normalized), Time.deltaTime * 4);
                }
                else
                {
                    state = State.retracting;
                }
                break;
            case State.deployed:
                if (!shootSomething)
                {
                    state = State.retracting;
                    break;
                }
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.ProjectOnPlane(target.position - transform.position, Vector3.up).normalized), Time.deltaTime * 4);              
                state = State.shooting;
                break;
            case State.shooting:
                if (!shootSomething)
                {
                    state = State.deployed;
                    break;
                }
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.ProjectOnPlane(target.position - transform.position, Vector3.up).normalized), Time.deltaTime * 4);
                if (canShoot)
                {
                    ThrowBlade();
                }
                break;
            case State.retracting:
                deployWeight = Mathf.Clamp(deployWeight - Time.deltaTime, 0, 1);
                if (deployWeight == 0)
                    state = State.off;
                if (enemyType == EnemyType.wheely)
                {
                }
                else if(enemyType == EnemyType.wheelyBoss)
                {
                }
                if (shootSomething)
                    state = State.deploying;
                break;
            default:
                break;
        }
	}

    public void Shoot(Transform newTarget)
    {
        target = newTarget;
        shootSomething = true;
    }

    public void StopShooting()
    {
        shootSomething = false;
    }

    void ShootBullets (Transform exit)
    {
        canShoot = false;
        gunCooldownCounter = gunCoolDown;
        GameObject bullet = Instantiate(bullets.bulletParent, exit.position, exit.rotation) as GameObject;
        BulletBehaviour bulletScript = bullet.GetComponent<BulletBehaviour>();
        bulletScript.SetBulletType(Bullets.BulletType.normal);
        bulletScript.SetDamage(GameObject.Find("GameManager").GetComponent<UnitManager>().aiDamage);
    }

    void ThrowBlade()
    {
        print("Yes?");
        canShoot = false;
        gunCooldownCounter = 2;
        if (currentWeapon == CurrentWeapon.left && leftBladeHome)
        {
            leftBlade.parent = null;
            leftBladeHome = false;
            leftBlade.GetComponent<HackHackBladeScript>().Throw(target.position, leftBladePos);
            currentWeapon = CurrentWeapon.right;
        }
        else if( currentWeapon == CurrentWeapon.right && rightBladeHome)
        {
            rightBlade.parent = null;
            rightBladeHome = false;
            rightBlade.GetComponent<HackHackBladeScript>().Throw(target.position, rightBladePos);
            currentWeapon = CurrentWeapon.left;
        }
        else
            gunCooldownCounter = 1;
    }

    public void BladeReturned (Transform blade)
    {
        if (blade == leftBlade)
            leftBladeHome = true;
        else
            rightBladeHome = true;
    }
}
