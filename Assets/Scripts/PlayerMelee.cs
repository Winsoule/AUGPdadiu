using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(PlayerShoot))]
public class PlayerMelee : MonoBehaviour {

    public Transform SwordHandler;
    public Transform Sword;
    public Transform swordPositions;
    List<Transform> listOfSwordPositions = new List<Transform>();
    int lastSwordID = 0;
    [HideInInspector]
    public bool canMelee = true;

    enum MeleeState {sheathed, swinging, posing}
    MeleeState currentMeleeState = MeleeState.sheathed;

    Movement movement;
    PlayerShoot playershoot;
    Rigidbody body;
    InputDevice controller;

    MeleeState CurrentMeleeState
    {
        get { return currentMeleeState;}
        set
        {
            if(value != MeleeState.sheathed)
            {
                movement.canMove = false;
            }
            else
            {
                movement.canMove = true;
            }
            currentMeleeState = value;
        }
    }

    // Use this for initialization
    void Start () {
        foreach(Transform t in swordPositions)
        {
            listOfSwordPositions.Add(t);
        }
        body = GetComponent<Rigidbody>();
        movement = GetComponent<Movement>();
        playershoot = GetComponent<PlayerShoot>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        controller = movement.controller;
        if (controller.RightBumper.WasPressed)
        {
            if(canMelee)
            {
                StartCoroutine(SwingSword());
            }
        }
	}

    IEnumerator SwingSword()
    {
        float cooldown = 0.5f;
        while(cooldown > 0)
        {
            if (currentMeleeState != MeleeState.swinging || !canMelee)
                break;
            cooldown -= Time.deltaTime;
            if (cooldown <= 0)
                yield break;
            yield return new WaitForEndOfFrame();
        }
        CurrentMeleeState = MeleeState.swinging;
        movement.HolsterGuns(true);
        movement.canMove = false;
        movement.canLook = false;
        
        print(CurrentMeleeState);
        cooldown = 0.3f;
        SwordHandler.gameObject.SetActive(true);
        lastSwordID = (lastSwordID + 1) % listOfSwordPositions.Count;
        Sword.position = listOfSwordPositions[lastSwordID].position;
        Sword.rotation = listOfSwordPositions[lastSwordID].rotation;
        Vector3 dir = Vector3.ProjectOnPlane(movement.cameraTransform.TransformDirection(new Vector3(controller.RightStickX, 0, controller.RightStickY)), Vector3.up);
        while (cooldown > 0)
        {
            if (!canMelee)
                yield break;
            playershoot.CanShoot = false;
            RaycastHit hit;
            if (controller.RightStick.Vector.magnitude < 0.2f)
            {
                if (Physics.Raycast(body.position, movement.travelDirection.normalized, out hit, cooldown + 0.25f))
                {
                    body.MovePosition(hit.point + hit.normal*0.25f);
                }
                else
                {
                    body.MovePosition(body.position + movement.travelDirection.normalized * cooldown);
                    
                }
                SwordHandler.rotation = Quaternion.LookRotation(movement.travelDirection.normalized);
            }
            else
            {
                if (Physics.Raycast(body.position, dir.normalized, out hit, cooldown + 0.25f))
                {
                    body.MovePosition(hit.point + hit.normal * 0.25f);
                }
                else
                {
                    body.MovePosition(body.position + dir.normalized * cooldown);
                }
                SwordHandler.rotation = Quaternion.LookRotation(dir.normalized);
            }
            cooldown -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        CurrentMeleeState = MeleeState.posing;
        print(CurrentMeleeState);
        cooldown = 0.5f;
        while (cooldown > 0)
        {
            playershoot.CanShoot = false;
            if (currentMeleeState == MeleeState.swinging)
                yield break;
            cooldown -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        CurrentMeleeState = MeleeState.sheathed;
        movement.HolsterGuns(false);
        movement.canMove = true;
        movement.canLook = true;
        playershoot.CanShoot = true;
        SwordHandler.gameObject.SetActive(false);
        print(CurrentMeleeState);
    }
}
