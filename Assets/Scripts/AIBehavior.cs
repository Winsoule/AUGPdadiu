using UnityEngine;
using System.Collections;


[RequireComponent (typeof(NavMeshAgent))]
[RequireComponent(typeof(AIShoot))]
public class AIBehavior : MonoBehaviour {

    enum State
    {
        idle,
        patrol,
        chase,
        shooting
    };

    [HideInInspector]
    public GameObject player;
    public float lookDistance = 2;
    public float hearDistance = 1;
    public float sightAngle = 35f;
    public float lostHimDistance = 5;
    public float waitTime = 5.0f;
    public float hitDestination = 1.0f;
    public float mapSize = 10;
    public float stoppingDistance = 6f;
    public LayerMask FindPlayerMask;

    private State _state = State.patrol;
    private float _waitedTime = Mathf.Epsilon;
    private Vector3 _destination = Vector3.zero;
    private NavMeshAgent _meshAgent;
    private AIShoot _aiShoot;
    public float _tempTime;

    UnitManager units;


    // Use this for initialization
    void Start ()
    {
        units = GameObject.Find("GameManager").GetComponent<UnitManager>();
        _meshAgent = transform.GetComponent<NavMeshAgent>();
        _aiShoot = GetComponent<AIShoot>();
        units = GameObject.Find("GameManager").GetComponent<UnitManager>();
        units.enemies.Add(transform);
    }
	
	// Update is called once per frame
	void Update ()
    {
        player = units.player.gameObject;
        switch (_state)
        {
            case State.chase:
                break;
            case State.idle:
                break;
            case State.patrol:
                break;
            case State.shooting:
                break;
            default:
                break;
        }



        RaycastHit hit;
        switch (_state)
        {
            case State.idle:
                if(_waitedTime == Mathf.Epsilon)
                {
                    _waitedTime = 0.0f;
                    _tempTime = Time.realtimeSinceStartup;
                }
                else if(_waitedTime >= waitTime)
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
                if(_destination == Vector3.zero)
                {
                    _destination = new Vector3(Random.Range(-mapSize, mapSize), 0.0f, Random.Range(-mapSize, mapSize));
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
                if (Vector3.Distance(player.transform.position, transform.position) <= stoppingDistance)
                {
                    _meshAgent.Stop();
                }
                else
                {
                    _meshAgent.Resume();
                }
                if (Vector3.Distance(player.transform.position, transform.position)> lostHimDistance)
                {
                    _state = State.idle;
                }
                else
                {
                    _meshAgent.SetDestination(player.transform.position);
                }

                //Shoot him!!
                if(Physics.Raycast(transform.position, player.transform.position - transform.position, out hit, 10, FindPlayerMask))
                {
                    if(hit.transform.root.gameObject == player)
                    {
                        _state = State.shooting;
                        _aiShoot.Shoot(player.transform);
                        _meshAgent.Stop();
                    }
                }
                break;
            case State.shooting:
                //If the player is either too far away or the robot doesn't have clear sight, it stops shooting.
                if (Vector3.Distance(player.transform.position, transform.position) >= 10
                    || (Physics.Raycast(transform.position, player.transform.position - transform.position, out hit, 10, FindPlayerMask) && hit.transform.root.gameObject != player))
                {
                    _state = State.chase;
                    _aiShoot.StopShooting();
                    _meshAgent.Resume();
                }
                break;
        }    
	}

    void FindPlayer()
    {
        if (SightRange() || HearRange())
        {
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
        if(Vector3.Distance(player.transform.position, transform.position) <= lookDistance && 
            Vector3.Angle(player.transform.position - transform.position, transform.forward) <= sightAngle)
        {
            return true;
        }
        return false;
    }
     public bool HearRange()
    {
        if(Vector3.Distance(player.transform.position, transform.position) <= hearDistance)
        {
            return true;
        }
        return false;
    }
}
