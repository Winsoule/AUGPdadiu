using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BlowIntoPieces))]
public class Health : MonoBehaviour {

    [HideInInspector]
    public float startHealth;
    public float health = 1;
    public bool isBoss;
    public Material portalMat;
    MeshMaker meshMaker;

    MakeLevel maker;

    public float lifeLinkScaler = 0;

    // Use this for initialization
    void Start () {
        RunAtStart();
        meshMaker = GetComponent<MeshMaker>();
        maker = GameObject.Find("GameManager").GetComponent<MakeLevel>();
    }

    protected void RunAtStart()
    {
        //startHealth = health;
    }

    public void LifeLink(float gainedLife)
    {
        health = Mathf.Clamp(health + gainedLife * lifeLinkScaler, 0, startHealth);
    }
	
	// Update is called once per frame
	void Update () {
        if (health <= 0)
        {
            if (maker != null)
            {
                if (isBoss)
                {
                    if (maker.bosses.Count == 1)
                    {
                        GameObject go = meshMaker.Torus(portalMat);
                        go.name = "Portal";
                        go.transform.position = transform.position;
                        go.transform.rotation = Quaternion.Euler(50, 225, 0);
                        go.AddComponent<Portal>();
                        go.AddComponent<BoxCollider>();
                        go.GetComponent<BoxCollider>().isTrigger = true;
                    }
                    else
                    {
                        maker.bosses.Remove(gameObject);
                    }
                }
                else if (maker.bosses.Count == 0 && maker.enemys.Count == 1)
                {
                    GameObject go = meshMaker.Torus(portalMat);
                    go.name = "Portal";
                    go.transform.position = transform.position;
                    go.transform.rotation = Quaternion.Euler(50, 225, 0);
                    go.AddComponent<Portal>();
                    go.AddComponent<BoxCollider>();
                    go.GetComponent<BoxCollider>().isTrigger = true;
                }
                else
                {
                    maker.enemys.Remove(gameObject);
                }
            }
            GetComponent<BlowIntoPieces>().BlowUp();
            DropMoney money = GetComponent<DropMoney>();
            if (money != null)
                money.DropSomeMoney();
            Destroy(gameObject);
        }
    }
}
