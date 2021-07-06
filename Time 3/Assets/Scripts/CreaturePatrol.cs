using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.AI;

public class CreaturePatrol : MonoBehaviour
{
    public float respawnDistance = 50f;
    public enum AlertnessLevel
    {
        distracted, suspicious, running
    }
    //Dictates whether the aget waits on each note
    [SerializeField]
    bool _patrolwaiting;


    // The total time the creature needs to see the player to flee
    public float maxSeenTime = 3f;
    // The total time the creature needs to hear the player to become alert
    public float maxHearTime = 3f;

    //The probability of switching direction
    [SerializeField]
    float _switchProbability;

    //The total time it wait at each node.
    [SerializeField]
    List <float> _totalWaitTimes;

    //The list of all patrol nodes to visit
    [SerializeField]
    List<Waypoint> _patrolPoints;

    public float walkingSpeed = 3f;
    public float runningSpeed = 5f;
    //place to flee to
    public Transform hideout;
    //Private variables for base behaviour
    public AlertnessLevel alertness = AlertnessLevel.distracted;
    private NavMeshAgent _navMeshAgent;
    private int _currrentPatrolIndex;
    private bool _travelling;
    private bool _waiting;
    private bool _patrolForward = true;
    private float _waitTimer;
    private float _seenTimer = 0f;
    private float _heardTimer = 0f;
    public Animator anim;
    private FOVDetection _fov;
    private Transform _playerTransform;
    private Coroutine _respawnCoroutine = null;
    private GameObject _creatureModel;

    private void Start()
    {
        _navMeshAgent = this.GetComponent<NavMeshAgent>();
        _fov = GetComponent<FOVDetection>();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _creatureModel = transform.GetChild(0).gameObject;
        Assert.IsNotNull(_navMeshAgent, "creature missing navMeshAgent");
        Assert.IsNotNull(_navMeshAgent, "creature missing FOVDetection");
        Assert.IsNotNull(_playerTransform, "couldn't find Player");
        Assert.IsNotNull(_creatureModel, "couldn't Creature Model. Should be child of Creature");

        Assert.IsNotNull(_patrolPoints, "patrol points no set");
        Assert.IsNotNull(anim, "Animator not set!");
        Assert.IsNotNull(hideout, "Hideout location not set!");
        Assert.IsTrue(_patrolPoints.Count >= 2, "creature needs at least 2 patrol points");
        Assert.IsTrue(_patrolPoints.Count == _totalWaitTimes.Count, "waitTimes and patrolPoints dont match!");

        _currrentPatrolIndex = 0;
        SetDestination();

    }

    public void Alert(float value)
    {
        _heardTimer += value;
    }

    private void Update()
    {
        if(_fov.isInFov){
            _seenTimer += Time.deltaTime;
        }

        if(_heardTimer > maxHearTime){
            if(alertness == AlertnessLevel.distracted){
                alertness = AlertnessLevel.suspicious;
            }
            else{
                alertness = AlertnessLevel.running;
            }
            _heardTimer = 0f;
        }

        if(_seenTimer > maxSeenTime){
            alertness = AlertnessLevel.running;
            _seenTimer = 0f;
            _heardTimer = 0f;
        }

        switch(alertness){
            case AlertnessLevel.distracted:
                _navMeshAgent.speed = walkingSpeed;
                RegularPatrol();
            break;
            case AlertnessLevel.running:
                _navMeshAgent.speed=runningSpeed;
                Flee();
            break;
        }
    }

    private void RegularPatrol()
    {
        anim.SetBool("Walk", true);
        if (_travelling && _navMeshAgent.remainingDistance <= 1f)
        {
            _travelling = false;

            //If were going to wait, then wait
            if (_patrolwaiting)
            {
                _waiting = true;
                _waitTimer = 0f;
                anim.SetBool("Walk", false);
            }
            else
            {
                ChangePatrolPoint();
                SetDestination();
                anim.SetBool("Walk", true);
            }
        }

        //Instead if were waiting
        if (_waiting)
        {
            _waitTimer += Time.deltaTime;
            if (_waitTimer >= _totalWaitTimes[_currrentPatrolIndex])
            {
                _waiting = false;

                ChangePatrolPoint();
                SetDestination();
                anim.SetBool("Walk", true);
            }
            else{
                anim.SetBool("Walk",false);
            }
        }
    }

    private void Flee()
    {

        _waiting = false;
        anim.SetBool("Walk", true);

        _navMeshAgent.SetDestination(hideout.position);

        float dist=_navMeshAgent.remainingDistance;
        if (_navMeshAgent.remainingDistance <= 1f){
            _respawnCoroutine = StartCoroutine(waitRespawn());
        }

    }
    private IEnumerator waitRespawn()
    {
        _creatureModel.SetActive(false);
        yield return new WaitUntil(()=>{return Vector3.Distance(_playerTransform.position, transform.position) >= respawnDistance;});
        alertness = AlertnessLevel.distracted;
        _seenTimer = 0f;
        _heardTimer = 0f;
        _creatureModel.SetActive(true);
        _respawnCoroutine = null;
        _travelling = true;
    }



    private void SetDestination()
    {
        if (_patrolPoints != null)
        {
            Vector3 targetVector = _patrolPoints[_currrentPatrolIndex].transform.position;
            _navMeshAgent.SetDestination(targetVector);
            _travelling = true;
        }
    }


    /// <sumary>
    /// Selects a new patrol point in the available list but
    /// also with a small probability allows us to move forward or backwards
    /// </sumary>
    private void ChangePatrolPoint()
    {
        if (UnityEngine.Random.Range(0f, 1f) <= _switchProbability)
        {
            _patrolForward = !_patrolForward;
        }

        if (_patrolForward)
        {
            _currrentPatrolIndex = (_currrentPatrolIndex + 1) % _patrolPoints.Count;
        }
        else
        {
            if (--_currrentPatrolIndex < 0)
            {
                _currrentPatrolIndex = _patrolPoints.Count - 1;
            }
        }
    }
}
