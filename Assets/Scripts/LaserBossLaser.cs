using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[RequireComponent(typeof(AIBehavior))]
public class LaserBossLaser : MonoBehaviour
{

    public enum State { off, deploying, deployed, shooting, retracting }

    //public Transform Gun;
    public List<Transform> Nozzles;
    //public Transform Arm;
    public ParticleSystem laserRampUpParticle;
    public Transform laser;
    public GameObject laserHitParticle;
    ParticleSystem laserHit;
    Vector3 GunOrgLocalPos;
    Vector3 ArmOrgLocalPos;

    CameraShake cameraShake;
    LaserBossBehavior behavior;
    Bullets bullets;
    Transform target;
    public State state = State.off;
    bool shootSomething = false;
    int currentNozzle = 0;
    float deployWeight = 0;
    public float gunCoolDown = 3;
    float gunCooldownCounter = 0;
    public float laserHold = 4;
    float laserHoldCounter = 0;
    bool canShoot = true;
    public LayerMask laserMask;


    // Use this for initialization
    void Start()
    {
        if (Nozzles.Count < 1)
        {
            Debug.Log("No nozzles assigned for '" + gameObject.name + "'!");
            enabled = false;
        }

        cameraShake = Camera.main.GetComponent<CameraShake>();
        laserHit = Instantiate(laserHitParticle).GetComponent<ParticleSystem>();
        laserHit.enableEmission = false;
        laserHit.gameObject.SetActive(true);
        laserHit.transform.parent = transform;
        gunCooldownCounter = gunCoolDown;
        laserHoldCounter = laserHold;
        behavior = GetComponent<LaserBossBehavior>();
        bullets = GameObject.Find("GameManager").GetComponent<Bullets>();
        //target = behavior.player.transform;
    }

    // Update is called once per frame
    void Update()
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
                    laserRampUpParticle.startSize = Mathf.Lerp(0, 0.07f, deployWeight);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Vector3.ProjectOnPlane(target.position - Nozzles[currentNozzle].position, Vector3.up).normalized), 20 * Time.deltaTime);
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
                else
                    state = State.shooting;
                break;
            case State.shooting:
                if (!shootSomething)
                {
                    state = State.deployed;
                    laser.localScale = new Vector3(1, 1, 0);
                    laserHit.enableEmission = false;
                    break;
                }
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Vector3.ProjectOnPlane(target.position - Nozzles[currentNozzle].position, Vector3.up).normalized), 20 * Time.deltaTime);
                if (canShoot)
                {
                    laserHit.enableEmission = true;
                    laser.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane((target.position - laser.position), transform.right));
                    RaycastHit laserRayHit;
                    if (Physics.Raycast(laser.position, laser.forward, out laserRayHit, Mathf.Infinity, laserMask))
                    {
                        laser.localScale = new Vector3(1, 1, Vector3.Distance(laserRayHit.point, laser.position) * 0.5f + 0.5f);
                        laserHit.transform.position = laserRayHit.point;
                    }
                    else
                    {
                        laser.localScale = new Vector3(1, 1, 10);
                        laserHit.enableEmission = false;
                    }
                    if (cameraShake != null)
                    {
                        Vector3 cameraShakeVector = Vector3.Project(target.position - laser.position, laser.forward);
                        cameraShakeVector = cameraShakeVector.normalized * Mathf.Clamp(cameraShakeVector.magnitude, 0, laser.localScale.z * 2);
                        float shakeDistance = 10;
                        cameraShake.Shake(Mathf.Max(shakeDistance - Vector3.Distance(target.position, laser.position + cameraShakeVector), 0) / shakeDistance * 0.5f);
                    }
                }
                break;
            case State.retracting:
                deployWeight = Mathf.Clamp(deployWeight - Time.deltaTime, 0, 1);
                laserRampUpParticle.startSize = Mathf.Lerp(0, 0.07f, deployWeight);
                if (deployWeight == 0)
                    state = State.off;
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

    void ShootBullets(Transform exit)
    {
        canShoot = false;
        gunCooldownCounter = gunCoolDown;
        GameObject bullet = Instantiate(bullets.bulletParent, exit.position, exit.rotation) as GameObject;
        BulletBehaviour bulletScript = bullet.GetComponent<BulletBehaviour>();
        bulletScript.SetBulletType(Bullets.BulletType.normal);
        currentNozzle = (currentNozzle + 1) % Nozzles.Count;
    }

    void OnTriggerStay(Collider col)
    {
        
        if (state == State.shooting)
        {
            Health enemyHealth = col.gameObject.GetComponent<Health>();
            if(enemyHealth != null)
            {
                enemyHealth.health -= Time.deltaTime * GameObject.Find("GameManager").GetComponent<UnitManager>().bossDamage;
            }
        }
    }
}
