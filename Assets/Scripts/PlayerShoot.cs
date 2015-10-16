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
        GameObject bullet = Instantiate(bullets.bulletParent, nozzles[currentGun].position, nozzles[currentGun].rotation) as GameObject;
        BulletBehaviour bulletScript = bullet.GetComponent<BulletBehaviour>();
        bulletScript.SetBulletType(currentBulletType);
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
