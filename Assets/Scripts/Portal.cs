using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {
    public float rotateSpeed = 360f;
    UnitManager manager;
	// Use this for initialization
	void Start () {
        manager = GameObject.Find("GameManager").GetComponent<UnitManager>();
    }
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0, Space.World);
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.name == "Player")
        {
            Debug.Log(manager.level);
            if(manager.level >= manager.maxLevels)
            {
                Application.LoadLevel("End");
            }
            else
            {
                manager.playerHealth = col.GetComponent<PlayerHealth>().health;
                manager.Save();
                Application.LoadLevel("Menu");
            }
            
        }
    }
}
