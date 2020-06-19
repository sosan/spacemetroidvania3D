using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UniRx.Async;
using System;
using UnityEngine.AI;
using Unity.RemoteConfig;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class Enemy : MonoBehaviour
{

    public enum AINavigationType
    {
        UnityNavmesh,
        AStarPathfinding
    }

    protected AINavigationType navigationType;
    protected AIPath _aiPath;
    protected NavMeshAgent _agent;
    [SerializeField] protected Transform startPoint;
    [SerializeField] protected Transform endPoint;


    [SerializeField] protected float viewRange = 5;
    [SerializeField] protected float attackCd = 1f;
    protected float _attackCdTimer;
    protected float _defaultCdTimer = 0f;
    [SerializeField] protected int damage = 100;


    [SerializeField] protected float initialHealth = 5;
    protected float _actualHealth = 1;

    [SerializeField] protected bool _canMove = true;
    protected bool chasing = false;

    protected Vector3 originOffset = Vector3.zero;
    float updateNavigationTimer = 0f;

    protected virtual void Awake()
    {
        switch (navigationType)
        {
            case AINavigationType.UnityNavmesh:
                _agent = GetComponent<NavMeshAgent>();
                break;
            case AINavigationType.AStarPathfinding:
                _aiPath = GetComponent<AIPath>();
                break;
            default:
                break;
        }
        _actualHealth = initialHealth;
    }

    protected virtual void Start()
    {
        switch (navigationType)
        {
            case AINavigationType.UnityNavmesh:
                _agent.SetDestination(startPoint.position);
                break;
            case AINavigationType.AStarPathfinding:
                _aiPath.destination = startPoint.position;
                _aiPath.SearchPath();
                break;
            default:
                break;
        }
    }

    protected virtual void Update()
    {
        if (CheckPlayer())
        {
            chasing = true;
            Chase();
        }
        else if (chasing)
        {
            chasing = false;
            ChangeMovility(true);
        }
        else if (CheckDestinationReached())
        {
            ChangeMovility(false);
            switch (navigationType)
            {
                case AINavigationType.UnityNavmesh:
                    _agent.SetDestination(SetNewTargetWhenPreviousReached(_agent.destination, _agent.stoppingDistance));
                    break;
                case AINavigationType.AStarPathfinding:
                    _aiPath.destination = SetNewTargetWhenPreviousReached(_aiPath.destination, _aiPath.endReachedDistance);
                    _aiPath.SearchPath();
                    break;
                default:
                    break;
            }
            ChangeMovility(true);
            ResetAttackCdToDefeult();
            chasing = false;
        }
        else if (_canMove)
        {
            switch (navigationType)
            {
                case AINavigationType.UnityNavmesh:
                    ChangeDirectionToTarget(_agent.destination);
                    break;
                case AINavigationType.AStarPathfinding:
                    ChangeDirectionToTarget(_aiPath.destination);
                    break;
                default:
                    break;
            }
            ResetAttackCdToDefeult();
            chasing = false;
        }
    }

    protected virtual void FixedUpdate()
    {
        if (updateNavigationTimer <= 0)
        {
            switch (navigationType)
            {
                case AINavigationType.UnityNavmesh:
                    break;
                case AINavigationType.AStarPathfinding:
                    AstarPath.active.Scan();
                    break;
                default:
                    break;
            }
            updateNavigationTimer = 1f;
        }
        else
            updateNavigationTimer -= Time.fixedDeltaTime;
    }

    protected virtual void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.CompareTag("Ataque") == true) HeathChange(-1);
    }



    #region States

    protected virtual void Chase()
    {

        ChangeDirectionToTarget(PlayerMovement.instancePlayer.transform.position);
        ChangeMovility(false);

        if (_attackCdTimer <= 0)
        {
            _attackCdTimer = attackCd;
            Attack();
        }
        else
        {
            _attackCdTimer -= Time.deltaTime;
        }
    }

    protected abstract void Attack();

    protected virtual void Death() => Destroy(gameObject.transform.parent.gameObject);
    
    #endregion



    #region Checkers

    public virtual bool CheckPlayer()
    {
        bool b = false;
        if (Physics.CheckSphere(transform.position, viewRange, LayerMask.GetMask("Player")))
        {
            Ray ray = new Ray(PlayerMovement.instancePlayer.transform.position + (Vector3.up * 0.5f), (transform.position + originOffset) - (PlayerMovement.instancePlayer.transform.position + (Vector3.up * 0.5f)));
            RaycastHit hitInfo; Physics.Raycast(ray, out hitInfo, viewRange);
            if (hitInfo.collider != null && hitInfo.collider.GetComponent<Enemy>() != null && hitInfo.collider.GetComponent<Enemy>() == this)
            {
                b = true;
            }
        }
        return b;
    }

    public virtual bool CheckDestinationReached()
    {
        switch (navigationType)
        {
            case AINavigationType.UnityNavmesh:
                return _agent.remainingDistance <= _agent.stoppingDistance;
            case AINavigationType.AStarPathfinding:
                return _aiPath.remainingDistance <= _aiPath.endReachedDistance;
            default:
                return false;
        }
    }

    public bool CheckForCollider(Collider desiredCollider, List<Collider> collidersToCompare)
    {
        bool b = false;

        for (int i = 0; i < collidersToCompare.Count; i++)
        {
            if (collidersToCompare[i] == desiredCollider)
            {
                b = true;
                break;
            }
        }

        return b;
    }

    #endregion



    #region Setters

    protected void ResetAttackCdToDefeult() => _attackCdTimer = _defaultCdTimer;

    private Vector3 SetNewTargetWhenPreviousReached(Vector3 currentTarget, float stoppingDistance)
    {
        if (currentTarget == endPoint.position || ((endPoint.position - transform.position).magnitude <= stoppingDistance))
        {
            transform.Rotate(0, 180, 0);
            return startPoint.position;
        }
        else if (currentTarget == startPoint.position || ((startPoint.position - transform.position).magnitude <= stoppingDistance))
        {
            transform.Rotate(0, 180, 0);
            return endPoint.position;
        }
        else return currentTarget;
    }

    protected virtual void ChangeDirectionToTarget(Vector3 target)
    {
        if (((target - transform.position).normalized).x < 0 && transform.right.x > 0)
            transform.Rotate(0, 180, 0);
        else if (((target - transform.position).normalized).x > 0 && transform.right.x < 0)
            transform.Rotate(0, 180, 0);
    }

    #endregion



    #region Public Events

    public void SetMovility(bool movility)
    {
        switch (navigationType)
        {
            case AINavigationType.UnityNavmesh:
                _agent.isStopped = !movility;
                if (!movility)
                    _agent.velocity = Vector3.zero;
                break;
            case AINavigationType.AStarPathfinding:
                _aiPath.canMove = movility;
                break;
            default:
                break;
        }
    }

    public virtual void ChangeMovility(bool newValue) => SetMovility(_canMove = newValue);
    public virtual void ChangeMovility() => SetMovility(_canMove = !_canMove);

    public virtual void HeathChange(int amount) {_actualHealth += amount; if(_actualHealth <= 0) Death(); }

    #endregion

    private void OnDrawGizmos()
    {
        if (PlayerMovement.instancePlayer != null)
            Debug.DrawRay(transform.position + originOffset, ((PlayerMovement.instancePlayer.transform.position + Vector3.up * 0.5f) - transform.position).normalized * viewRange, Color.cyan);

#if UNITY_EDITOR
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, Vector3.back, viewRange);
#endif
    }

}