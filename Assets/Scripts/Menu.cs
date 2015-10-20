using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent (typeof(UnitManager))]
public class Menu : MonoBehaviour {

    UnitManager manager;

    public List<Transform> Buttons;
    public Text moneyText;

    void Start()
    {

        manager = GetComponent<UnitManager>();
        foreach (Transform t in Buttons)
        {
            Button button = t.GetComponentInChildren<Button>();
            Text buttonText = button.transform.GetComponentInChildren<Text>();
            Text price = t.FindChild("PriceFrame").GetComponentInChildren<Text>();
            AssignRandomButton(button, buttonText, price);
        }
    }

    void Update()
    {
        //manager.money = money;
        moneyText.text = string.Format("Money: {0}", manager.money);
    }

    public void Continue()
    {
        Application.LoadLevel("Jensarea");
    }

    public void ButtonTurnMeOff(Transform t)
    {
        t.gameObject.SetActive(false);
    }

    public void IncreaseMaxHealth(Transform buttonTransform, int price)
    {
        if (price <= manager.money)
        {
            manager.playerMaxHealth += 1f;
            manager.money -= price;
            ButtonTurnMeOff(buttonTransform);
        }
        return;
    }

    public void IncreaseHealthRegen(Transform buttonTransform, int price)
    {
        if (price <= manager.money)
        {
            manager.playerRegen += 0.1f;
            manager.money -= price;
            ButtonTurnMeOff(buttonTransform);
        }
        return;
    }

    public void IncreaseMovementSpeed(Transform buttonTransform, int price)
    {
        if (price <= manager.money)
        {
            manager.movementSpeed += 0.5f;
            manager.money -= price;
            ButtonTurnMeOff(buttonTransform);
        }
        return;
    }

    public void IncreaseDamage(Transform buttonTransform, int price)
    {
        if (price <= manager.money)
        {
            manager.playerDamage += 0.5f;
            manager.money -= price;
            ButtonTurnMeOff(buttonTransform);
        }
        return;
    }

    public void IncreaseFireRate(Transform buttonTransform, int price)
    {
        if (price <= manager.money)
        {
            manager.playerAttackSpeed += 1f;
            manager.money -= price;
            ButtonTurnMeOff(buttonTransform);
        }
        return;
    }

    public void IncreaseMoneyDropping(Transform buttonTransform, int price)
    {
        if (price <= manager.money)
        {
            manager.moneyDroppingModifier += 0.5f;
            manager.money -= price;
            ButtonTurnMeOff(buttonTransform);
        }
        return;
    }

    public void IncreaseLifeLink(Transform buttonTransform, int price)
    {
        if (price <= manager.money)
        {
            manager.playerLifelink += 0.05f;
            manager.money -= price;
            ButtonTurnMeOff(buttonTransform);
        }
        return;
    }

    public void IncreaseBulletSize(Transform buttonTransform, int price)
    {
        if (price <= manager.money)
        {
            manager.bulletSize += 0.2f;
            manager.money -= price;
            ButtonTurnMeOff(buttonTransform);
        }
        return;
    }

    public void IncreaseBulletSpeed(Transform buttonTransform, int price)
    {
        if (price <= manager.money)
        {
            manager.bulletSpeed += 0.2f;
            manager.money -= price;
            ButtonTurnMeOff(buttonTransform);
        }
        return;
    }
    // Use this for initialization
    
	
    void AssignRandomButton(Button button, Text buttonText, Text price)
    {
        int randomIndex = Random.Range(0, 9);
        switch(randomIndex)
        {
            case 0:
                button.onClick.AddListener(() => IncreaseMaxHealth(button.transform.parent, 8));
                buttonText.text = "Max Health: +1";
                price.text = "8$";
                break;
            case 1:
                button.onClick.AddListener(() => IncreaseHealthRegen(button.transform.parent,8));
                buttonText.text = "Health Regeneration: +0.1/s";
                price.text = "8$";
                break;
            case 2:
                button.onClick.AddListener(() => IncreaseMovementSpeed(button.transform.parent,8));
                buttonText.text = "Movement speed: +1";
                price.text = "8$";
                break;
            case 3:
                button.onClick.AddListener(() => IncreaseDamage(button.transform.parent,8));
                buttonText.text = "Damage: +1";
                price.text = "8$";
                break;
            case 4:
                button.onClick.AddListener(() => IncreaseFireRate(button.transform.parent,8));
                buttonText.text = "Fire Rate: +0.2/s";
                price.text = "8$";
                break;
            case 5:
                button.onClick.AddListener(() => IncreaseMoneyDropping(button.transform.parent,8));
                buttonText.text = "Enemy Money Dropped: +1";
                price.text = "8$";
                break;
            case 6:
                button.onClick.AddListener(() => IncreaseLifeLink(button.transform.parent,8));
                buttonText.text = "Life Drain: +0.1/damage";
                price.text = "8$";
                break;
            case 7:
                button.onClick.AddListener(() => IncreaseBulletSize(button.transform.parent,8));
                buttonText.text = "Bullet Size: x2";
                price.text = "8$";
                break;
            case 8:
                button.onClick.AddListener(() => IncreaseBulletSpeed(button.transform.parent,5));
                buttonText.text = "Bullet Speed: +1";
                price.text = "5$";
                break;
        }
    }
}
