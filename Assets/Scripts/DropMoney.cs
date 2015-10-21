using UnityEngine;
using System.Collections;

public class DropMoney : MonoBehaviour {

    public GameObject moneyPrefab;
    public int money;
    [HideInInspector]
    float moneyModifier = 1;
    UnitManager manager;

    public void DropSomeMoney()
    {
        for(int i = 0; i < (int)(money*moneyModifier); i++)
        {
            GameObject moneyObj = Instantiate(moneyPrefab, transform.position, Quaternion.identity) as GameObject;
            Vector3 throwDir = Random.onUnitSphere * 5;
            throwDir = new Vector3(throwDir.x, Mathf.Abs(throwDir.y) + 5, throwDir.z);
            moneyObj.GetComponent<Rigidbody>().AddForce(throwDir, ForceMode.VelocityChange);
        }
    }

	// Use this for initialization
	void Start ()
    {
        manager = Serializer.Load<UnitManager>("UnitInfo");
        moneyModifier = manager.moneyDroppingModifier;
	}
}
