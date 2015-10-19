using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AIBehavior))]
public class AIShoot : MonoBehaviour {

    public enum State {off, deploying, deployed, shooting, retracting}
    public enum EnemyType {wheely, wheelyBoss}

    public Transform Gun;
    public List<Transform> Nozzles;
    public Transform Arm;
    Vector3 GunOrgLocalPos;
    Vector3 ArmOrgLocalPos;


    AIBehavior behavior;
    Bullets bullets;
    Transform target;
    public State state = State.off;
    public EnemyType enemyType = EnemyType.wheely;
    bool shootSomething = false;
    int currentNozzle = 0;
    float deployWeight = 0;
    public float gunCoolDown = 1;
    float gunCooldownCounter = 0;
    bool canShoot = true;


    // Use this for initialization
    void Start ()
    {
        if(Nozzles.Count < 1)
        {
            Debug.Log("No nozzles assigned for '" + gameObject.name + "'!");
            enabled = false;
        }

        gunCooldownCounter = gunCoolDown;
        behavior = GetComponent<AIBehavior>();
        bullets = GameObject.Find("GameManager").GetComponent<Bullets>();
        //target = behavior.player.transform;
        GunOrgLocalPos = Gun.localPosition;
        ArmOrgLocalPos = Arm.localPosition;
	}
	
	// Update is called once per frame
	void Update ()
    {
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
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.ProjectOnPlane(target.position - Gun.position, Vector3.up).normalized), Time.deltaTime * 4);
                    if (enemyType == EnemyType.wheely)
                    {
                        Gun.localPosition = Vector3.Lerp(GunOrgLocalPos, GunOrgLocalPos + Gun.InverseTransformDirection(Vector3.up * 0.5f), deployWeight);
                        Arm.localScale = new Vector3(Arm.localScale.x, Vector3.Distance(Gun.position, Arm.position), Arm.localScale.z);
                        Arm.up = (Gun.position - Arm.position).normalized;
                    }
                    else if (enemyType == EnemyType.wheelyBoss)
                    {
                        Gun.localPosition = Vector3.Lerp(GunOrgLocalPos, GunOrgLocalPos + Gun.InverseTransformDirection(Vector3.up * 0.5f) - Vector3.ProjectOnPlane(transform.forward, Gun.parent.up) * 0.3f, deployWeight);
                        Arm.localScale = new Vector3(Arm.localScale.x, Vector3.Distance(Gun.position, Arm.position) * 0.5f, Arm.localScale.z);
                        Arm.up = (Gun.position - Arm.position).normalized;
                    }
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
                Gun.rotation = Quaternion.Lerp(Gun.rotation, Quaternion.LookRotation(target.position - Gun.position, Vector3.up), Time.deltaTime * 10);
                state = State.shooting;
                break;
            case State.shooting:
                if (!shootSomething)
                {
                    state = State.deployed;
                    break;
                }
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.ProjectOnPlane(target.position - transform.position, Vector3.up).normalized), Time.deltaTime * 4);
                Gun.rotation = Quaternion.Lerp(Gun.rotation, Quaternion.LookRotation(target.position - Gun.position, Vector3.up), Time.deltaTime * 10);
                if (canShoot)
                    ShootBullets(Nozzles[currentNozzle]);
                break;
            case State.retracting:
                deployWeight = Mathf.Clamp(deployWeight - Time.deltaTime, 0, 1);
                if (deployWeight == 0)
                    state = State.off;
                if (enemyType == EnemyType.wheely)
                {
                    Gun.localPosition = Vector3.Lerp(GunOrgLocalPos, GunOrgLocalPos + Gun.InverseTransformDirection(Vector3.up * 0.5f), deployWeight);
                    Arm.localScale = new Vector3(1, Vector3.Distance(Gun.position, Arm.position), 1);
                    Arm.up = (Gun.position - Arm.position).normalized;
                }
                else if(enemyType == EnemyType.wheelyBoss)
                {
                    Gun.localPosition = Vector3.Lerp(GunOrgLocalPos, GunOrgLocalPos + Gun.InverseTransformDirection((Vector3.up + Vector3.forward)*0.5f), deployWeight);
                    Arm.localScale = new Vector3(1, Vector3.Distance(Gun.position, Arm.position)*0.5f, 1);
                    Arm.up = (Gun.position - Arm.position).normalized;
                }
                Gun.rotation = Quaternion.Lerp(Gun.rotation, Gun.parent.rotation, Time.deltaTime * 10);
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
        currentNozzle = (currentNozzle + 1) % Nozzles.Count;
    }
}
