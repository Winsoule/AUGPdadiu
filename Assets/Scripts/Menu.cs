using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    public List<Transform> Buttons;
    public Transform Button2;
    public Transform Button3;
    public Transform Button4;

    public void ButtonTurnMeOff(Transform t)
    {
        t.gameObject.SetActive(false);
    }

    public void IncreaseMaxHealth(Transform buttonTransform)
    {
        //Increase MaxHealth!!
        return;
    }

    public void IncreaseHealthRegen(Transform buttonTransform)
    {
        //Increase HealthRegen!!
        return;
    }

    public void IncreaseMovementSpeed(Transform buttonTransform)
    {
        //Increase MovementSpeed!!
        return;
    }

    public void IncreaseDamage(Transform buttonTransform)
    {
        //Increase Damage!!
        return;
    }

    public void IncreaseFireRate(Transform buttonTransform)
    {
        //Increase MaxHealth!!
        return;
    }

    public void IncreaseMoneyDropping(Transform buttonTransform)
    {
        //Increase MaxHealth!!
        return;
    }

    public void IncreaseLifeLink(Transform buttonTransform)
    {
        //Increase LifeLink!!
        return;
    }

    public void IncreaseBulletSize(Transform buttonTransform)
    {
        //Increase BulletSize!!
        return;
    }

    public void IncreaseBulletSpeed(Transform buttonTransform)
    {
        //Increase MaxHealth!!
        return;
    }
    // Use this for initialization
    void Start ()
    {
        foreach(Transform t in Buttons)
        {
            Button button = t.GetComponentInChildren<Button>();
            Text buttonText = button.transform.GetComponentInChildren<Text>();
            Text price = t.FindChild("PriceFrame").GetComponentInChildren<Text>();
            AssignRandomButton(button, buttonText, price);
        }
	}
	
    void AssignRandomButton(Button button, Text buttonText, Text price)
    {
        int randomIndex = Random.Range(0, 9);
        switch(randomIndex)
        {
            case 0:
                button.onClick.AddListener(() => IncreaseMaxHealth(button.transform.parent));
                button.onClick.AddListener(() => ButtonTurnMeOff(button.transform.parent));
                buttonText.text = "Max Health: +1";
                break;
            case 1:
                button.onClick.AddListener(() => IncreaseHealthRegen(button.transform.parent));
                button.onClick.AddListener(() => ButtonTurnMeOff(button.transform.parent));
                buttonText.text = "Health Regeneration: +0.1/s";
                break;
            case 2:
                button.onClick.AddListener(() => IncreaseMovementSpeed(button.transform.parent));
                button.onClick.AddListener(() => ButtonTurnMeOff(button.transform.parent));
                buttonText.text = "Movement speed: +1";
                break;
            case 3:
                button.onClick.AddListener(() => IncreaseDamage(button.transform.parent));
                button.onClick.AddListener(() => ButtonTurnMeOff(button.transform.parent));
                buttonText.text = "Damage: +1";
                break;
            case 4:
                button.onClick.AddListener(() => IncreaseFireRate(button.transform.parent));
                button.onClick.AddListener(() => ButtonTurnMeOff(button.transform.parent));
                buttonText.text = "Fire Rate: +0.2/s";
                break;
            case 5:
                button.onClick.AddListener(() => IncreaseMoneyDropping(button.transform.parent));
                button.onClick.AddListener(() => ButtonTurnMeOff(button.transform.parent));
                buttonText.text = "Enemy Money Dropped: +1";
                break;
            case 6:
                button.onClick.AddListener(() => IncreaseLifeLink(button.transform.parent));
                button.onClick.AddListener(() => ButtonTurnMeOff(button.transform.parent));
                buttonText.text = "Life Drain: +0.1/damage";
                break;
            case 7:
                button.onClick.AddListener(() => IncreaseBulletSize(button.transform.parent));
                button.onClick.AddListener(() => ButtonTurnMeOff(button.transform.parent));
                buttonText.text = "Bullet Size: x2";
                break;
            case 8:
                button.onClick.AddListener(() => IncreaseBulletSpeed(button.transform.parent));
                button.onClick.AddListener(() => ButtonTurnMeOff(button.transform.parent));
                buttonText.text = "Bullet Speed: +1";
                break;
        }
    }

	// Update is called once per frame
	void Update () {
	
	}
}
