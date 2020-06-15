using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UniRx.Async;
using System;
using Unity.RemoteConfig;
using static Enemy;

public class DroneAI : Enemy
{
    [SerializeField] AINavigationType aiNavigationType = AINavigationType.AStarPathfinding;
    [SerializeField] Transform weaponI;
    [SerializeField] Transform weaponD;

    [SerializeField] Transform shootpointI;
    [SerializeField] Transform shootpointD;

    [SerializeField] Transform weaponPivot;
    [SerializeField] GameObject laser;

    private Quaternion _weaponDInitialRotation;
    private Quaternion _weaponIInitialRotation;

    bool _arm1 = false;
    public string assignmentId;

    public struct userAttributes
    {

    }

    public struct appAttributes
    {

    }

    override protected void Awake()
    {
        _defaultCdTimer = attackCd / 2;
        ResetAttackCdToDefeult();
        navigationType = aiNavigationType;
        _weaponDInitialRotation = weaponD.transform.localRotation;
        _weaponIInitialRotation = weaponI.transform.localRotation;
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
                viewRange = ConfigManager.appConfig.GetFloat("droneViewRange");
                attackCd = ConfigManager.appConfig.GetFloat("droneAttackCD");
                damage = ConfigManager.appConfig.GetInt("droneDamage");
                assignmentId = ConfigManager.appConfig.assignmentID;
                break;
        }
    }

    override protected void Update()
    {
        if (!CheckPlayer())
        {
            weaponD.transform.localRotation = _weaponDInitialRotation;
            weaponI.transform.localRotation = _weaponIInitialRotation;
        }
        base.Update();
    }

    override protected void Chase()
    {
        weaponD.transform.rotation = Quaternion.LookRotation((PlayerMovement.instancePlayer.transform.position + (Vector3.up)) - weaponPivot.transform.position, Vector3.up);
        weaponI.transform.rotation = Quaternion.LookRotation((PlayerMovement.instancePlayer.transform.position + (Vector3.up)) - weaponPivot.transform.position, Vector3.up);
        base.Chase();
    }

    override protected void Attack()
    {
        float desviation = UnityEngine.Random.Range(1.75f, 2.75f);
        weaponD.transform.rotation = Quaternion.LookRotation((PlayerMovement.instancePlayer.transform.position + (Vector3.up * desviation)) - weaponPivot.transform.position, Vector3.up);
        weaponI.transform.rotation = Quaternion.LookRotation((PlayerMovement.instancePlayer.transform.position + (Vector3.up * desviation)) - weaponPivot.transform.position, Vector3.up);
        Transform trfmSpawn = shootpointD;
        trfmSpawn =(_arm1 == true) ? shootpointD : shootpointI;
        Instantiate(laser, trfmSpawn.position, trfmSpawn.rotation).GetComponent<Laser>().Damage = damage;
        _arm1 = !_arm1;
    }
}