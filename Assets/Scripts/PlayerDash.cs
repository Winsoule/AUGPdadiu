using UnityEngine;
using System.Collections;
using InControl;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(PlayerShoot))]
[RequireComponent(typeof(PlayerMelee))]
public class PlayerDash : MonoBehaviour {

    Movement movement;
    PlayerShoot playershoot;
    PlayerMelee playerMelee;
    Rigidbody body;
    InputDevice controller;

    // Use this for initialization
    void Start () {
        body = GetComponent<Rigidbody>();
        movement = GetComponent<Movement>();
        playershoot = GetComponent<PlayerShoot>();
        playerMelee = GetComponent<PlayerMelee>();
    }

    // Update is called once per frame
    void Update()
    {
        controller = movement.controller;
        if(controller.LeftBumper.WasPressed)
        {
            //StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        movement.canMove = false;
        movement.canLook = false;
        playerMelee.canMelee = false;
        while(true)
        {
            yield break;
        }
    }
}
