using UnityEngine;
using System.Collections;


[RequireComponent (typeof(NavMeshAgent))]
public class AIBehavior : MonoBehaviour {

    enum State
    {
        idle,
        patrol,
        chase
    };

    public GameObject player;
    public float lookDistance = 2;
    public float hearDistance = 1;
    public float sightAngle = 35f;
    public float lostHimDistance = 5;
    public float waitTime = 5.0f;
    public float hitDestination = 1.0f;
    public float mapSize = 10;
    public float stoppingDistance = 6f;

    private State _state = State.patrol;
    private float _waitedTime = Mathf.Epsilon;
    private Vector3 _destination = Vector3.zero;
    private NavMeshAgent _meshAgent;
    public float _tempTime;
    

	// Use this for initialization
	void Start ()
    {
        _meshAgent = transform.GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (SightRange() || HearRange())
        {
            if(Physics.Raycast(new Ray(transform.position, player.transform.position)))
            {
                _state = State.chase;
                Debug.Log("Chaseing");
            }
        }

        if(Vector3.Distance(player.transform.position, transform.position) <= stoppingDistance)
        {
            _meshAgent.Stop();
        }
        else
        {
            _meshAgent.Resume();
        }

	    switch(_state)
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
                    Debug.Log("patroling");
                }
                else
                {
                    _waitedTime = Time.realtimeSinceStartup - _tempTime;
                }
                break;

            case State.patrol:
                if(_destination == Vector3.zero)
                {
                    Debug.Log("Getting New Destination");
                    _destination = new Vector3(Random.Range(-mapSize, mapSize), 0.0f, Random.Range(-mapSize, mapSize));
                    _meshAgent.SetDestination(_destination);
                }
                else if (Vector3.Distance(_destination, transform.position) <= hitDestination)
                {
                    _state = State.idle;
                    Debug.Log("idleing");
                    _destination = Vector3.zero;
                }
                break;

            case State.chase:
                if(Vector3.Distance(player.transform.position, transform.position)> lostHimDistance)
                {
                    _state = State.idle;
                    Debug.Log("idleing");
                }
                else
                {
                    _meshAgent.SetDestination(player.transform.position);
                }
                break;
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
