using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {
    public float rotateSpeed = 360f;
	// Use this for initialization
	void Start () {
       /*
        var temp = Serializer.Load<Portal>("Poop");
        if (temp != null)
        {
            //this = temp;
        }
        else
        {
            Serializer.Save<Portal>(this, "Poop");
        }*/
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0, Space.World);
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.name == "Player")
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }
}
