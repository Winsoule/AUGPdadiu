using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class UnitManager : MonoBehaviour {

    public int maxLevels = 4;
    public int spawnAmount = 10;
    public int level = 1;
    public float playerMaxHealth;
    public float playerHealth;
    public float playerDamage;
    public float playerAttackSpeed;
    public float playerRegen;
    public float playerLifelink;
    public float bulletSize;
    public float bulletSpeed;
    public int money;
    public float movementSpeed;

    [HideInInspector]
    public Transform player;
    UnitManager temp;
    // Use this for initialization
    void Start ()
    {

        temp = Serializer.Load<UnitManager>("UnitInfo");
        if(player != null)
        {
            var playerinfo = player.GetComponent<PlayerHealth>();
            var playerMovement = player.GetComponent<Movement>();
            var playerShoot = player.GetComponent<PlayerShoot>();


            level = temp.level;
            playerinfo.startHealth = temp.playerMaxHealth;
            playerinfo.healthRegen = temp.playerRegen;
            playerinfo.health = temp.playerHealth;
            playerMovement.movementSpeed = temp.movementSpeed;
            playerShoot.damage = temp.playerDamage;
            playerShoot.FireRate = temp.playerAttackSpeed;
            playerinfo.lifeLinkScaler = temp.playerLifelink;
            playerShoot.bulletSize = temp.bulletSize;
            playerShoot.bulletSpeed = temp.bulletSpeed;

        }

        spawnAmount = temp.spawnAmount;
        level = temp.level;
        playerMaxHealth = temp.playerMaxHealth;
        playerRegen = temp.playerRegen;
        playerHealth = temp.playerHealth;
        movementSpeed = temp.movementSpeed;
        playerDamage = temp.playerDamage;
        playerAttackSpeed = temp.playerAttackSpeed;
        playerLifelink = temp.playerLifelink;
        bulletSize = temp.bulletSize;
        bulletSpeed = temp.bulletSpeed;
        maxLevels = temp.maxLevels;

    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Application.LoadLevel("MainMenu");
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }


    public void Save()
    {
        
        Serializer.Save<UnitManager>(this, "UnitInfo");
    }

    public void NewSave()
    {
        maxLevels = 4;
        spawnAmount = 10;
        level = 1;
        playerMaxHealth = 3;
        playerHealth = 3;
        playerDamage = 1;
        playerAttackSpeed = 3;
        playerRegen = 0;
        money = 0;
        movementSpeed = 3.5f;
        playerLifelink = 0;
        bulletSize = 1;
        bulletSpeed = 1;
    Debug.Log("NewGAME");

        Serializer.Save<UnitManager>(this, "UnitInfo");
    }
}
