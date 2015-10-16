using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AIBehavior))]
public class AIShoot : MonoBehaviour {

    enum State {off, deploying, deployed, shooting, retracting}

    public Transform Gun;
    public Transform Nozzle;
    public Transform Arm;
    Vector3 GunOrgLocalPos;
    Vector3 ArmOrgLocalPos;


    AIBehavior behavior;
    Bullets bullets;
    Transform target;
    State state = State.off;
    bool shootSomething = false;
    float deployWeight = 0;
    float gunCooldownCounter = 0;
    bool canShoot = true;


    // Use this for initialization
    void Start ()
    {
        behavior = GetComponent<AIBehavior>();
        bullets = GameObject.Find("GameManager").GetComponent<Bullets>();
        target = behavior.player.transform;
        GunOrgLocalPos = Gun.localPosition;
        ArmOrgLocalPos = Arm.localPosition;
	}
	
	// Update is called once per frame
	void Update ()
    {
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
                    Gun.localPosition = Vector3.Lerp(GunOrgLocalPos, GunOrgLocalPos + Gun.InverseTransformDirection(Vector3.up * 0.5f), deployWeight);
                    Arm.localScale = new Vector3(1, Vector3.Distance(Gun.position, Arm.position), 1);
                    Arm.up = (Gun.position - Arm.position).normalized;
                }
                else
                {
                    state = State.retracting;
                }
                break;
            case State.deployed:
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.ProjectOnPlane(target.position - transform.position, Vector3.up).normalized), Time.deltaTime * 4);
                Gun.rotation = Quaternion.Lerp(Gun.rotation, Quaternion.LookRotation(target.position - Gun.position, Vector3.up), Time.deltaTime * 10);
                state = State.shooting;
                break;
            case State.shooting:
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.ProjectOnPlane(target.position - transform.position, Vector3.up).normalized), Time.deltaTime * 4);
                Gun.rotation = Quaternion.Lerp(Gun.rotation, Quaternion.LookRotation(target.position - Gun.position, Vector3.up), Time.deltaTime * 10);
                if (canShoot)
                    ShootBullets(Nozzle );
                if (!shootSomething)
                    state = State.retracting;
                break;
            case State.retracting:
                deployWeight = Mathf.Clamp(deployWeight - Time.deltaTime, 0, 1);
                if (deployWeight == 0)
                    state = State.off;
                Gun.localPosition = Vector3.Lerp(GunOrgLocalPos, GunOrgLocalPos + Gun.InverseTransformDirection(Vector3.up * 0.5f), deployWeight);
                Arm.localScale = new Vector3(1, Vector3.Distance(Gun.position, Arm.position), 1);
                Arm.up = (Gun.position - Arm.position).normalized;
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
        gunCooldownCounter = 1;
        GameObject bullet = Instantiate(bullets.bulletParent, Nozzle.position, Nozzle.rotation) as GameObject;
        BulletBehaviour bulletScript = bullet.GetComponent<BulletBehaviour>();
        bulletScript.SetBulletType(Bullets.BulletType.normal);
    }
}
