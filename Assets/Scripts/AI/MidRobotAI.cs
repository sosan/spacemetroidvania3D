using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.RemoteConfig;
using System.Linq;

public class MidRobotAI : Enemy
{
    Animator _anim;
    [SerializeField] AINavigationType aiNavigationType = AINavigationType.UnityNavmesh;
    [SerializeField] float attackDistance = 1f;

    public string assignmentId;
    Vector3 _previousTarget;
    private bool onAttack;

    public struct userAttributes
    {

    }

    public struct appAttributes
    {

    }

    override protected void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
        navigationType = aiNavigationType;
        originOffset = transform.up * 0.5f;
        base.Awake();
        // Add a listener to apply settings when successfully retrieved:
        ConfigManager.FetchCompleted += ApplyRemoteSettings;

        // Set the environment ID:
        ConfigManager.SetEnvironmentID("6e6486d7-889e-40a1-882e-f37c4a7aa3a9");
        ConfigManager.FetchConfigs<userAttributes, appAttributes>(new userAttributes(), new appAttributes());
    }

    void ApplyRemoteSettings(ConfigResponse configResponse)
    {
        // Conditionally update settings, depending on the response's origin:
        switch (configResponse.requestOrigin)
        {
            case ConfigOrigin.Default:
                Debug.Log("No settings loaded this session; using default values.");
                break;
            case ConfigOrigin.Cached:
                Debug.Log("No settings loaded this session; using cached values from a previous session.");
                break;
            case ConfigOrigin.Remote:
                Debug.Log("New settings loaded this session; update values accordingly.");
                viewRange = ConfigManager.appConfig.GetFloat("midrobotViewRange");
                attackDistance = ConfigManager.appConfig.GetFloat("midrobotAttackDistance");
                attackCd = ConfigManager.appConfig.GetFloat("midrobotAttackCD");
                damage = ConfigManager.appConfig.GetInt("midrobotDamage");
                assignmentId = ConfigManager.appConfig.assignmentID;
                break;
        }
    }

    override protected void Update()
    {
        if (!CheckPlayer())
        {
            _anim.ResetTrigger("Attack");
            if (chasing)
            {
                chasing = false;
                _agent.SetDestination(_previousTarget);
            }
        }
        else
        {
            if (!chasing)
                _previousTarget = _agent.destination;
        }
        base.Update();
    }

    override protected void Chase()
    {
        ChangeDirectionToTarget(PlayerMovement.instancePlayer.transform.position);
        if (Vector3.Distance(transform.position, PlayerMovement.instancePlayer.transform.position) <= attackDistance)
        {
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
        else if(!onAttack)
        {
            _anim.ResetTrigger("Attack");
            _agent.isStopped = false;
            _agent.SetDestination(PlayerMovement.instancePlayer.transform.position);
        }
    }

    override protected void Attack()
    {
        _anim.SetTrigger("Attack");
    }

    public void StartAttack()
    {
        ChangeMovility(false);
        onAttack = true;
        
    }

    public void DamagePlayer()
    {
        if (CheckForCollider(PlayerMovement.instancePlayer.GetComponent<Collider>(), Physics.OverlapSphere(transform.position + (transform.right * 0.75f), 1f).ToList()))
        {
            PlayerMovement.instancePlayer.SetHealth(damage);
        }
    }

    public void FinishAttack()
    {
        onAttack = false;
        ChangeMovility(true);
    }

}
