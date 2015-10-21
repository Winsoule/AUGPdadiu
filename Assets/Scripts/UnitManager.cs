using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

[System.Serializable]
public class UnitManager : MonoBehaviour {

    public int maxLevels;
    public int spawnAmount;
    public int level;
    public int levelSize, levelProcent, levelIncreaser;
    public int shopMod;
    public float playerMaxHealth;
    public float playerHealth;
    public float playerDamage;
    public float playerAttackSpeed;
    public float playerRegen;
    public float playerLifelink;
    public float bulletSize;
    public float bulletSpeed;
    public int money;
    public float moneyDroppingModifier;
    public float movementSpeed;
    public float aiDamage, slasherDamage, bossDamage;
    public int bossDelay, slasherDelay, iaDelay;


   
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
        levelSize = temp.levelSize;
        levelProcent = temp.levelProcent;
        levelIncreaser = temp.levelIncreaser;
        shopMod = temp.shopMod;
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
        moneyDroppingModifier = temp.moneyDroppingModifier;
        money = temp.money;
        aiDamage = temp.aiDamage;
        slasherDamage = temp.slasherDamage;
        bossDamage = temp.bossDamage;
        bossDelay = temp.bossDelay;
        slasherDelay = temp.slasherDelay;
        iaDelay = temp.iaDelay;
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.R) || InputManager.ActiveDevice.MenuWasPressed)
        {
            InputManager.ActiveDevice.Vibrate(0f);
            Application.LoadLevel("MainMenu");
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            InputManager.ActiveDevice.Vibrate(0f);
            Application.Quit();
        }
    }


    public void Save()
    {
        Serializer.Save<UnitManager>(this, "UnitInfo");
    }

    public void NewSave()
    {
        maxLevels = 8;
        spawnAmount = 4;
        level = 1;
        levelSize = 25;
        levelProcent = 25;
        levelIncreaser = 5;
        shopMod = 9;

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
        moneyDroppingModifier = 1;

        aiDamage = 0.5f;
        slasherDamage = 1;
        bossDamage = 1;
        bossDelay = 1;
        slasherDelay = spawnAmount;
        iaDelay = 0;
        Debug.Log("New Save");

        Serializer.Save<UnitManager>(this, "UnitInfo");
    }
}
