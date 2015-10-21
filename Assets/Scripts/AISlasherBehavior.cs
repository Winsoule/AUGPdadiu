using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(NavMeshAgent))]
public class AISlasherBehavior : MonoBehaviour
{

    public enum State
    {
        idle,
        patrol,
        chase,
        searching
    };

    //[HideInInspector]
    public GameObject player;
    public float lookDistance = 2;
    public float hearDistance = 1;
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
    private AIShoot _aiShoot;
    public float _tempTime;

    public Transform Blades;

    UnitManager units;


    // Use this for initialization
    void Start()
    {
        units = GameObject.Find("GameManager").GetComponent<UnitManager>();
        _meshAgent = transform.GetComponent<NavMeshAgent>();
        _aiShoot = GetComponent<AIShoot>();
        units = GameObject.Find("GameManager").GetComponent<UnitManager>();
        _meshAgent.updateRotation = false;

    }

    // Update is called once per frame
    void Update()
    {
        Blades.Rotate(transform.up, Time.deltaTime * 200);
        if(_state == State.chase)
            Blades.Rotate(transform.up, Time.deltaTime * 200);
        if (units.player != null)
            player = units.player.gameObject;
        else
            player = null;

        RaycastHit hit;
        switch (_state)
        {
            case State.idle:
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
                if (player == null)
                {
                    _state = State.idle;
                    break;
                }
                if (Vector3.Distance(player.transform.position, transform.position) <= stoppingDistance)
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
                break;
            case State.searching:
                if (Vector3.Distance(_meshAgent.destination, transform.position) <= stoppingDistance)
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
        if (Vector3.Distance(player.transform.position, transform.position) <= lookDistance)
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
        if (obj.GetComponent<BulletBehaviour>() != null && _state != State.chase)
        {
            _meshAgent.SetDestination(transform.position + Vector3.ProjectOnPlane(col.contacts[0].point - transform.position, Vector3.up).normalized);
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
