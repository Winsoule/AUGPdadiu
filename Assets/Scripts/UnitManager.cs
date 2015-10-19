using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class UnitManager : MonoBehaviour {

    public int spawnAmount = 10;
    public int level = 1;
    public float playerMaxHealth;
    public float playerHealth;
    public float playerDamage;
    public float playerAttackSpeed;
    public float playerRegen;
    public int money;
    public float movementSpeed;

    public Transform player;
    [HideInInspector]
    public List<Transform> enemies = new List<Transform>();
    UnitManager temp;
    // Use this for initialization
    void Start ()
    {

        temp = Serializer.Load<UnitManager>("UnitInfo");
        print(temp);
        var playerinfo = player.gameObject.GetComponent<PlayerHealth>();

        level = temp.level;
        playerinfo.startHealth = temp.playerMaxHealth;
        playerinfo.health = temp.playerHealth;
    }

    // Update is called once per frame
    void Update ()
    {
        
	}


    public void Save()
    {
        playerMaxHealth = player.GetComponent<PlayerHealth>().startHealth;
        playerHealth = player.GetComponent<PlayerHealth>().health;
        level++;
        Serializer.Save<UnitManager>(this, "UnitInfo");
    }

    public void NewSave()
    {
        spawnAmount = 10;
        level = 1;
        playerMaxHealth = 3;
        playerHealth = 3;
        playerDamage = 2;
        playerAttackSpeed = 2;
        playerRegen = 2;
        money = 0;
        movementSpeed = 1;
        Debug.Log("NewGAME");

        Serializer.Save<UnitManager>(this, "UnitInfo");
    }
}
