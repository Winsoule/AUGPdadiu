using UnityEngine;
using System.Collections;

public class DropMoney : MonoBehaviour {

    public GameObject moneyPrefab;
    public int money;
    UnitManager manager;

    public void DropSomeMoney()
    {
        for(int i = 0; i < (int)(money*manager.moneyDroppingModifier); i++)
        {
            GameObject moneyObj = Instantiate(moneyPrefab, transform.position, Quaternion.identity) as GameObject;
            Vector3 throwDir = Random.onUnitSphere * 5 + Vector3.up * 5;
            throwDir = new Vector3(throwDir.x, Mathf.Abs(throwDir.y), throwDir.z);
            moneyObj.GetComponent<Rigidbody>().AddForce(throwDir, ForceMode.VelocityChange);
        }
    }

	// Use this for initialization
	void Start ()
    {
        manager = Serializer.Load<UnitManager>("UnitInfo");
	}
}
