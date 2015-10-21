using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HackHackBehavior : MonoBehaviour
{

    public enum State
    {
        idle,
        patrol,
        chase,
        shooting,
        searching
    };

    //[HideInInspector]
    public GameObject player;
    public float lookDistance = 2;
    public float hearDistance = 1;
    public float sightAngle = 45f;
    public float lostHimDistance = 5;
    public float waitTime = 5.0f;
    public float hitDestination = 1.0f;
    public float mapSize = 10;
    public float stoppingDistance = 6f;
    public LayerMask FindPlayerMask;

    public List<Vector3> patrolPos;
    public State _state = State.patrol;
    private float _waitedTime = Mathf.Epsilon;
    private Vector3 _destination = Vector3.zero;
    //private Vector3 _lastSeenPlayerPos = Vector3.zero;
    private NavMeshAgent _meshAgent;
    public float _tempTime;

    UnitManager units;
    HackHackShoot shootScript;


    // Use this for initialization
    void Start()
    {
        units = GameObject.Find("GameManager").GetComponent<UnitManager>();
        _meshAgent = transform.GetComponent<NavMeshAgent>();
        units = GameObject.Find("GameManager").GetComponent<UnitManager>();
        shootScript = GetComponent<HackHackShoot>();
        //_meshAgent.updateRotation = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (units.player != null)
            player = units.player.gameObject;
        else
            player = null;

        RaycastHit hit;
        switch (_state)
        {
            case State.idle:
                print("Idle");
                if (_waitedTime == Mathf.Epsilon)
                {
                    _waitedTime = 0.0f;
                    _tempTime = Time.realtimeSinceStartup;
                }
                else if (_waitedTime >= waitTime)
                {
                    _state = State.patrol;
                    _waitedTime = Mathf.Epsilon;
                    _destination = Vector3.zero;
                }
                else
                {
                    _waitedTime = Time.realtimeSinceStartup - _tempTime;
                }
                FindPlayer();
                break;

            case State.patrol:
                if (_destination == Vector3.zero && patrolPos.Count > 0)
                {
                    _destination = patrolPos[Random.Range(0, patrolPos.Count)];
                    _meshAgent.SetDestination(_destination);
                }
                else if (Vector3.Distance(_destination, transform.position) <= hitDestination)
                {
                    _state = State.idle;
                    _destination = Vector3.zero;
                }
                FindPlayer();
                break;

            case State.chase:
                print("No?");
                if (player == null)
                {
                    _state = State.idle;
                    break;
                }
                if (Vector3.Distance(player.transform.position, transform.position) <= 2)
                {
                    
                    _meshAgent.Stop();
                }
                else
                {
                    _meshAgent.Resume();
                }
                if (Vector3.Distance(player.transform.position, transform.position) > lostHimDistance
                    || (Physics.Raycast(transform.position, player.transform.position - transform.position, out hit, 10, FindPlayerMask)
                    && hit.transform.root.gameObject != player))
                {
                    _state = State.searching;
                    _meshAgent.SetDestination(player.transform.position);
                }
                else
                {
                    _meshAgent.SetDestination(player.transform.position);
                }
                //Shoot him!!
                if (Physics.Raycast(transform.position, player.transform.position - transform.position, out hit, 10, FindPlayerMask))
                {
                    if (hit.transform.root.gameObject == player)
                    {
                        _state = State.shooting;
                        shootScript.Shoot(player.transform);
                        //_meshAgent.Stop();
                        _meshAgent.updateRotation = false;
                    }
                }
                break;
            case State.shooting:
                if (player == null)
                {
                    _state = State.idle;
                    _meshAgent.Resume();
                    _meshAgent.updateRotation = true;
                    break;
                }
                //If the player is either too far away or the robot doesn't have clear sight, it stops shooting.
                if (Vector3.Distance(player.transform.position, transform.position) >= 10
                    || (Physics.Raycast(transform.position, player.transform.position - transform.position, out hit, 10, FindPlayerMask) && hit.transform.root.gameObject != player))
                {
                    _state = State.chase;
                    shootScript.StopShooting();
                    _meshAgent.Resume();
                    _meshAgent.updateRotation = true;
                }
                break;
            case State.searching:
                if (Vector3.Distance(_meshAgent.destination, transform.position) <= 2)
                {
                    _state = State.idle;
                }
                FindPlayer();
                break;
        }
    }

    void FindPlayer()
    {
        if (player != null && (SightRange() || HearRange()))
        {
            //print("Play?");
            RaycastHit hit;
            if (Physics.Raycast(new Ray(transform.position, player.transform.position - transform.position), out hit, FindPlayerMask))
            {
                if (hit.transform.root.gameObject == player)
                {
                    _state = State.chase;
                }
            }
        }
    }

    public bool SightRange()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= lookDistance &&
            Vector3.Angle(player.transform.position - transform.position, transform.forward) <= sightAngle)
        {
            return true;
        }
        return false;
    }
    public bool HearRange()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= hearDistance)
        {
            return true;
        }
        return false;
    }

    void OnCollisionEnter(Collision col)
    {
        Transform obj = col.transform;
        if (col.transform.root != null)
            obj = col.transform.root;
        if (obj.GetComponent<BulletBehaviour>() != null && _state != State.shooting && _state != State.chase)
        {
            Rigidbody colBody = (col.transform.root != null ? col.transform.root.GetComponent<Rigidbody>() : col.transform.GetComponent<Rigidbody>());
            if (colBody != null)
            {
                _meshAgent.SetDestination(transform.position - col.relativeVelocity.normalized * 2);
            }
            else
                _meshAgent.SetDestination(transform.position + Vector3.ProjectOnPlane(col.contacts[0].point - transform.position, Vector3.up).normalized * 2);
            _state = State.searching;
        }
    }

    void OnTriggerStay(Collider col)
    {
        Health enemyHealth = col.gameObject.GetComponent<Health>();
        if (enemyHealth != null)
        {
            enemyHealth.health -= Time.deltaTime * GameObject.Find("GameManager").GetComponent<UnitManager>().slasherDamage;
        }
    }
}

