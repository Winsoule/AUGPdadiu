using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

[RequireComponent(typeof(Movement))]
public class PlayerShoot : MonoBehaviour {

    public List<Transform> nozzles = new List<Transform>();
    Bullets bullets;
    Bullets.BulletType currentBulletType = Bullets.BulletType.normal;

    int currentGun = 0;
    float gunCooldownCounter = 0;
    float gunCooldown = 0.1f;
    bool canShoot;
    InputDevice controller;

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
        if (controller.RightTrigger > 0.2f && canShoot)
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        canShoot = false;
        gunCooldownCounter = gunCooldown;
        GameObject bullet = Instantiate(bullets.bulletParent, nozzles[currentGun].position, nozzles[currentGun].rotation) as GameObject;
        BulletBehaviour bulletScript = bullet.GetComponent<BulletBehaviour>();
        bulletScript.SetBulletType(currentBulletType);
        currentGun = (currentGun + 1) % nozzles.Count;
    }
}
