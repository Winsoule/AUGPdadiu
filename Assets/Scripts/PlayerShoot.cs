using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

[RequireComponent(typeof(Movement))]
public class PlayerShoot : MonoBehaviour {

    public List<Transform> nozzles = new List<Transform>();
    public List<Transform> guns = new List<Transform>();
    Bullets bullets;
    Bullets.BulletType currentBulletType = Bullets.BulletType.normal;

    int currentGun = 0;
    float gunCooldownCounter = 0;
    float gunCooldown = 0.1f;
    bool canShoot;
    InputDevice controller;
    Health health;

    public float damage = 1;
    public float bulletSize = 1;
    public float bulletSpeed = 1;

    public float FireRate
    {
        get { return 1 / gunCooldown; }
        set { gunCooldown = 1 / value; }
    }

    public bool CanShoot
    {
        get { return canShoot; }
        set
        {
            if(!value)
            {
                gunCooldownCounter = gunCooldown;
            }
            canShoot = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        controller = InputManager.ActiveDevice;
        bullets = GameObject.Find("GameManager").GetComponent<Bullets>();
        health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        controller = InputManager.ActiveDevice;
        gunCooldownCounter = Mathf.Max(gunCooldownCounter - Time.deltaTime, 0);
        if(gunCooldownCounter == 0)
        {
            canShoot = true;
        }
        if (controller.RightTrigger > 0.2f && canShoot && controller.RightStick.Vector.magnitude > 0.2f)
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        CanShoot = false;
        Quaternion bulletRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(nozzles[currentGun].forward, Vector3.up), nozzles[currentGun].up);
        GameObject bullet = Instantiate(bullets.bulletParent, nozzles[currentGun].position, bulletRotation) as GameObject;
        BulletBehaviour bulletScript = bullet.GetComponent<BulletBehaviour>();
        bulletScript.SetBulletType(currentBulletType);
        bulletScript.SetDamage(damage);
        bulletScript.SetBulletSize(bulletSize);
        bulletScript.SetLifeLink(health);
        bulletScript.SetBulletSpeedScalar(bulletSpeed);
        StartCoroutine(Recoil(currentGun));
        currentGun = (currentGun + 1) % nozzles.Count;
        
    }

    IEnumerator Recoil(int gunID)
    {
        float cooldown = guns.Count * gunCooldown * 0.9f;
        float cooldownCounter = cooldown;

        while(cooldownCounter > 0)
        {
            cooldownCounter -= Time.deltaTime;
            Vector3 gunRot = guns[gunID].localEulerAngles;
            gunRot.x = -45 * (cooldownCounter / cooldown);
            guns[gunID].localEulerAngles = gunRot;
            yield return new WaitForEndOfFrame();
        }
    }
}
