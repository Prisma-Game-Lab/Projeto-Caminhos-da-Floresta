using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreaturePatrol : MonoBehaviour
{
    //Dictates whether the aget waits on each note
    [SerializeField]
    bool _patrolwaiting;

    //The total time it wait at each node.
    [SerializeField]
    float _totalWaitTime = 3f;

    //The probability of switching direction
    [SerializeField]
    float _switchProbability;

    //The list of all patrol nodes to visit
    [SerializeField]
    List<Waypoint> _patrolPoints;

    //Private variables for base behaviour
    private NavMeshAgent _navMeshAgent;
    private int _currrentPatrolIndex;
    private bool _travelling;
    private bool _waiting;
    private bool _patrolForward;
    private float _waitTimer;

    private void Start()
    {
        _navMeshAgent = this.GetComponent<NavMeshAgent>();

        if (_navMeshAgent == null)
        {
            Debug.LogError(" The NavMeshAgent is not attached to " + gameObject.name);
        }
        else
        {
            if (_patrolPoints != null && _patrolPoints.Count >= 2)
            {
                _currrentPatrolIndex = 0;
                SetDestination();
            }
        }
    }

    private void Update()
    {
        if (_travelling && _navMeshAgent.remainingDistance <= 1f)
        {
            _travelling = false;

            //If were going to wait, then wait
            if (_patrolwaiting)
            {
                _waiting = true;
                _waitTimer = 0f;
            }
            else
            {
                ChangePatrolPoint();
                SetDestination();
            }
        }

        //Instead if were waiting
        if (_waiting)
        {
            _waitTimer += Time.deltaTime;
                if(_waitTimer >= _totalWaitTime)
            {
                _waiting = false;

                ChangePatrolPoint();
                SetDestination();
            }
        }
    }

    private void SetDestination()
    {
        if(_patrolPoints != null)
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
        if(UnityEngine.Random.Range(0f, 1f) <= _switchProbability)
        {
            _patrolForward = !_patrolForward;
        }

        if (_patrolForward)
        {
            _currrentPatrolIndex = (_currrentPatrolIndex + 1) % _patrolPoints.Count;
        }
        else
        {
            if(--_currrentPatrolIndex < 0)
            {
                _currrentPatrolIndex = _patrolPoints.Count - 1;
            }
        }
    }
}
